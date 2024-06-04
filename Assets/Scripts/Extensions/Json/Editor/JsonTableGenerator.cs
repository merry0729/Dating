using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodeWriter;
using UnityEditor;
using UnityEngine;

public partial class JsonTableGenerator : ScriptableObject
{
	[MenuItem("DS/Generate Json Tables")]
    static void GenerateJsonTables()
    {
		Generate();
        EditorUtility.DisplayDialog("Result", "Json Table Generate Complete!", "Ok");
    }
    public static void Generate()
    {
        List<string> generatedClasses = new List<string>();
        GenerateDataTables(generatedClasses);
        GenerateLoadPart(generatedClasses);
		//GenerateDataChecker();

		AssetDatabase.Refresh();
    }

    static void ConsumeArrayDatas(string line, StreamReader reader, int arrayOpenCount)
	{
		int startIndex = 0;
		int arrayOpenIndex = line.IndexOf('[', startIndex);
		while (-1 != arrayOpenIndex) {
			++arrayOpenCount;
			startIndex = arrayOpenIndex + 1;
			arrayOpenIndex = line.IndexOf('[', startIndex);
		}

		startIndex = 0;
		int arrayCloseIndex = line.IndexOf(']', startIndex);
		while (-1 != arrayCloseIndex) {
			--arrayOpenCount;
			startIndex = arrayCloseIndex + 1;
			arrayCloseIndex = line.IndexOf(']', startIndex);
		}

		if (0 < arrayOpenCount)
			ConsumeArrayDatas(reader.ReadLine(), reader, arrayOpenCount);
	}

	static void GenerateDataTables(List<string> generatedClasses)
	{
		var files = GetFiles();
		_dicTableSizes.Clear();

		List<string> variableNames = new List<string>();
		foreach (var file in files) {
			if (false == file.Name.EndsWith(".json"))
				continue;

			variableNames.Clear();

			StringBuilder newName = new StringBuilder(file.Name);

			newName.Remove(newName.Length - 5, 5);
			//Debug.Log($"Generating Table: {newName}.json");
			string fileName = newName.ToString();

			newName.Append("Data");
			string className = newName.ToString();

			newName.Remove(newName.Length - 4, 4);
			newName.Append("Table");
			string tableClassName = newName.ToString();

			StreamReader reader = new StreamReader($"{GetTableDirectory}/{file.Name}");
			Dictionary<string, string> _dicEnumType = new Dictionary<string, string>();
			_dicTableSizes.Add(file.Name.Substring(0, file.Name.Length - 5), reader.BaseStream.Length);

			if (File.Exists($"{GetTableDataSourceDirectory}/{fileName}Table.cs")) {
				using (StreamReader csReader = new StreamReader($"{GetTableDataSourceDirectory}/{fileName}Table.cs")) {
					bool startCsCheck = false;
					while (false == csReader.EndOfStream) {
						string line = csReader.ReadLine();
						line = line.Trim();
						if (line.StartsWith("public class")|| line.StartsWith("public partial class ")) {
							startCsCheck = true;
							continue;
						}

						if (false == startCsCheck)
							continue;

						var strs = line.Split(' ');
						if (strs.Length >= 2 && strs[0].Equals("public") && strs[1].Equals("static")) {
							startCsCheck = false;
							continue;
						}

						if (strs.Length < 3)
							continue;

						switch (strs[1]) {
							case "int":
							case "float":
							case "bool":
							case "string":
							case "using":
								break;
							default:
								int index = 2;
								if (strs[2] == "[]")
									index = 3;
								_dicEnumType.Add(strs[index].Replace(";", ""), strs[1].Replace("[]", ""));
								break;
						}
					}
				}
			}

			var writer = new CodeWriter.CodeWriter(CodeWriterSettings.CSharpDefault);
			writer._($"// Auto Generated Code By JsonTableGenerator.",
				$"using System;",
				$"using UnityEngine;",
				$"using TemplateTable;",
				$"");

			using (writer.B($"public partial class {className}")) {
				while (false == reader.EndOfStream) {
					string line = reader.ReadLine();
					if (false == line.Contains("\""))
						continue;

					int firstOffset = line.IndexOf('\"', 0) + 1;
					int endOffset = line.IndexOf('\"', firstOffset + 1);
					string varName = line.Substring(firstOffset, endOffset - firstOffset);
					if (variableNames.Contains(varName)) {
						// 여러줄에 걸쳐 기록된 배열 데이터를 미리 읽어들여서 다음 라인 처리 때 해당 데이터가 변수로 취급되는것을 막는다.
						ConsumeArrayDatas(line, reader, 0);
						continue;
					}

					// 여러줄에 걸쳐서 데이터가 기록되는 경우가 있음 ㅠㅠ
					string dataType = GetDataType(line, reader);
					if (_dicEnumType.ContainsKey(varName)) {
						dataType = dataType.Replace("string", _dicEnumType[varName]);
					}

					if (dataType.Equals("string"))
						writer._($"public {dataType} {varName} = \"\";");
					else
						writer._($"public {dataType} {varName};");
					variableNames.Add(varName);
				}

				writer._("", $"public static {tableClassName} Table;");

				// 단순 복사 작업을 수행 하는 메서드를 정의
				writer._($"public {className} ShallowCopy()");
				writer._("{");
				writer._($"\treturn ({className}) this.MemberwiseClone();");
				writer._("}");
			}

			writer._($"public class {tableClassName} : TemplateTable<int, {className}> " + "{ }");

			var fileStream = new StreamWriter($"{GetTableDataSourceDirectory}/{fileName}Table.cs", false);
			fileStream.Write(writer.ToString());
			fileStream.Close();
			fileStream.Dispose();
			reader.Close();
			reader.Dispose();

			generatedClasses.Add(className);
		}
	}

	static Dictionary<string, long> _dicTableSizes = new Dictionary<string, long>();
	static string [] _dependancyTables = { "SettingPropertyData", "StringData", "LanguageFontData" };

	static void GenerateLoadPart(List<string> generatedClasses)
	{
		var writer = new CodeWriter.CodeWriter(CodeWriterSettings.CSharpDefault);
		writer._("// Auto Generated Code By JsonTableGenerator.",
			"using System;",
			"using UnityEngine;",
			"using Cysharp.Threading.Tasks;",
            "");

		const int HEAVY_SIZE = 1024 * 1024;

		void WriteProcessException(Action writeBody)
		{
			writer.WriteRaw("#if LIVE_BUILD");
			writer._("");
			writeBody();
			writer.WriteRaw("#else");
			writer._("", "try {");
			using (writer.i(null, null)) {
				writeBody();
			}

			writer._("} catch (Exception e) {");
			using (writer.i()) {
				writer._("Debug.LogError($\"TableLoader get exception {e}\");",
					"return false;");
			}

			writer._("}");
			writer.WriteRaw("#endif");
			writer._("");
		}

		//using (writer.B("namespace SHTableData")) {
			using (writer.B("public partial class TableLoader")) {
				using (writer.B("public async UniTask<bool> LoadHeavySize(bool delayLoad)")) {
					// 본문은 여기 로컬 메서드에 추가. 메서드 외부는 #if~#else~#end 처리임.
					void WriteLoadHeavySize()
					{
						writer._("_tableLoadingCount = 0;");
						writer._("_loadedTable.Clear();");
						int countForInsert = 0;
						foreach (var className in generatedClasses) {
							var jsonName = className.Substring(0, className.Length - 4);
							if (_dependancyTables.Contains(className))
								continue;

							var fileSize = _dicTableSizes[jsonName];
							if (fileSize < HEAVY_SIZE)
								continue;

							++countForInsert;
							writer._($"LoadTableGenericClient<{className}>(delayLoad);");
							
							if (countForInsert == 5) {
								countForInsert = 0;
								writer._($"await UniTask.Delay(20);");
							}
						}

						writer._("", "await UniTask.WaitUntil(() => 0 == _tableLoadingCount);");
						writer._("OnPostLoad();");
						writer._("return true;");
					}

					WriteProcessException(WriteLoadHeavySize);
				}

				using (writer.B("public async UniTask<bool> LoadNormalSize(bool delayLoad)")) {
					// 본문은 여기 로컬 메서드에 추가. 메서드 외부는 #if~#else~#end 처리임.
					void WriteLoadNormalSize()
					{
						writer._("_tableLoadingCount = 0;");
						writer._("_loadedTable.Clear();");
						int countForInsert = 0;
						foreach (var className in generatedClasses) {
							var jsonName = className.Substring(0, className.Length - 4);
							if (_dependancyTables.Contains(className))
								continue;

							var fileSize = _dicTableSizes[jsonName];
							if (fileSize >= HEAVY_SIZE)
								continue;

							++countForInsert;
							writer._($"LoadTableGenericClient<{className}>(delayLoad);");
							
							if (countForInsert == 5) {
								countForInsert = 0;
								writer._($"await UniTask.Delay(20);");
							}
						}

						writer._("", "await UniTask.WaitUntil(() => 0 == _tableLoadingCount);");
						writer._("OnPostLoad();");
						writer._("return true;");
					}

					WriteProcessException(WriteLoadNormalSize);
				}

				using (writer.B("public async UniTask<bool> LoadDependanciesOnly(bool delayLoad)")) {
					// 본문은 여기 로컬 메서드에 추가. 메서드 외부는 #if~#else~#end 처리임.
					void WriteLoadDependanciesOnly()
					{
						writer._("_tableLoadingCount = 0;");
						writer._("_loadedTable.Clear();");
						int countForInsert = 0;
						foreach (var className in generatedClasses) {
							var jsonName = className.Substring(0, className.Length - 4);
							if (false == _dependancyTables.Contains(className))
								continue;

							++countForInsert;
							writer._($"LoadTableGenericClient<{className}>(delayLoad);");
							
							if (countForInsert == 5) {
								countForInsert = 0;
								writer._($"await UniTask.Delay(20);");
							}
						}

						writer._("", "await UniTask.WaitUntil(() => 0 == _tableLoadingCount);");
						writer._("OnPostLoad();");
						writer._("return true;");
					}
					
					WriteProcessException(WriteLoadDependanciesOnly);
				}

				using (writer.B("public async UniTask<bool> LoadAllWithoutDependancies(bool delayLoad)")) {
					// 본문은 여기 로컬 메서드에 추가. 메서드 외부는 #if~#else~#end 처리임.
					void WriteLoadAllWithoutDependancies()
					{
						writer._("_tableLoadingCount = 0;");
						writer._("_loadedTable.Clear();");
						int countForInsert = 0;
						foreach (var className in generatedClasses) {
							var jsonName = className.Substring(0, className.Length - 4);
							if (_dependancyTables.Contains(className))
								continue;

							++countForInsert;
							writer._($"LoadTableGenericClient<{className}>(delayLoad);");
							
							if (countForInsert == 5) {
								countForInsert = 0;
								writer._($"await UniTask.Delay(20);");
							}
						}

						writer._("", "await UniTask.WaitUntil(() => 0 == _tableLoadingCount);");
						writer._("OnPostLoad();");
						writer._("return true;");
					}
					
					WriteProcessException(WriteLoadAllWithoutDependancies);
				}

				using (writer.B("public async UniTask<bool> LoadAll(bool delayLoad)")) {
					// 본문은 여기 로컬 메서드에 추가. 메서드 외부는 #if~#else~#end 처리임.
					void WriteLoadAll()
					{
						writer._("_tableLoadingCount = 0;");
						writer._("_loadedTable.Clear();");
						int countForInsert = 0;
						foreach (var className in generatedClasses) {
							++countForInsert;
							writer._($"LoadTableGenericClient<{className}>(delayLoad);");
							
							if (countForInsert == 5) {
								countForInsert = 0;
								writer._($"await UniTask.Delay(20);");
							}
						}

						writer._("", "await UniTask.WaitUntil(() => 0 == _tableLoadingCount);");
						writer._("OnPostLoad();");
						writer._("return true;");
					}
					
					WriteProcessException(WriteLoadAll);
				}

				using (writer.B("public async UniTask<bool> LoadAllCompletion(bool delayLoad)")) {
					// 본문은 여기 로컬 메서드에 추가. 메서드 외부는 #if~#else~#end 처리임.
					void WriteLoadAllCompletion()
					{
						writer._("_tableLoadingCount = 0;");
						writer._("_loadedTable.Clear();");
						int countForInsert = 0;
						foreach (var className in generatedClasses) {
							++countForInsert;
							writer._($"LoadTableGenericClient<{className}>(delayLoad, true);");
							
							if (countForInsert == 5) {
								countForInsert = 0;
								writer._($"await UniTask.Delay(20);");
							}
						}

						writer._("", "await UniTask.WaitUntil(() => 0 == _tableLoadingCount);");
						writer._("OnPostLoad();");
						writer._("return true;");
					}
					
					WriteProcessException(WriteLoadAllCompletion);
				}

				using (writer.B("public async UniTask<bool> LoadForServer(bool delayLoad)")) {
					// 본문은 여기 로컬 메서드에 추가. 메서드 외부는 #if~#else~#end 처리임.
					void WriteLoadForServer()
					{
						writer._("_loadedTable.Clear();", "");
						foreach (var className in generatedClasses)
							writer._($"await LoadTableGenericServer<{className}>(delayLoad);");

						writer._("", "OnPostLoad();");
						writer._("return true;");
					}

					WriteProcessException(WriteLoadForServer);
				}

				using (writer.B("public bool LoadOverrides(bool delayLoad)")) {
					void WriteLoadOverrides()
					{
						writer._("_loadedTable.Clear();", "");
						foreach (var className in generatedClasses)
							writer._($"LoadTableGenericOverride<{className}>(delayLoad);");

						writer._("", "OnPostLoad();");
						writer._("return true;");
					}
					
					WriteProcessException(WriteLoadOverrides);
				}
			}
		//}
        //writer._("}"); // namespace SHTableData

        var fileStream = new StreamWriter($"{GetSourceDirectory}/Extensions/Json/TableLoaderAutoGenerated.cs", false);
        fileStream.Write(writer.ToString());
        fileStream.Close();
        fileStream.Dispose();
    }

	static string GetDataType(string line, StreamReader reader)
	{
		// 자료형 체크
		// 쌍따옴표가 있으면 string
		// true / false 면 bool
		// 소숫점이 있으면 float
		// 아니면 int
		// [ 가 존재하면 배열 , ] 개수가 맞을 때 까지 파싱해야한다. [ 가 닫히기 전에 또 열리면 [][]. 배열 카운팅 체크도 필요.

		//string 
		int startIndex = line.IndexOf("\":") + 2;
		int openBracket = line.IndexOf("[", startIndex);
		
		int bracketCount = 0;
		int bracketDepth = 0;
		int dataDoubleQuote = -1;	// 변수명 다음에 데이터 쪽의 쌍따옴표.
		string dataType = "";

		// 원래는 -1 == openBracket 만으로 테스트 했지만, Text Data 내부에 배열용인 [ ] 값을 사용하는 경우도 있어서, 배열로 잘못 인식되는것을 막아야 하므로..
		// 텍스트인 경우에는 쌍따옴표 다음에 bracket 이 나타난다면 이것은 텍스트로 인식해줘야 한다.
		dataDoubleQuote = line.IndexOf("\"", startIndex);
		if (-1 != dataDoubleQuote && -1 != openBracket) {
			// 첫번째 openBracket 이 데이터의 쌍따옴표 다음에 나온다면, 이것은 string 내부의 것이므로 이 데이터는 openBracket 이 없는것으로 간주해줘야 한다.
			if (openBracket > dataDoubleQuote)
				openBracket = -1;
		}

		// 배열 여부 먼저 확인. 배열의 경우, 다음 라인에 데이터가 존재하는 경우도 있다.
		if (-1 == openBracket)
			return CheckDataType(line, startIndex);
		else {
			int strIndex = startIndex;

			try {
				SetDataTypeWithBracketDepth(ref bracketDepth, ref bracketCount, ref dataType, line, strIndex, reader);
			} catch (Exception e) {
				Debug.Log($"Catch Exception : {e}");
			}

			return ReturnWithBracket(dataType);
		}

		string ReturnWithBracket(string dataTypeWithBracket)
		{
			if (0 == bracketCount)
				return dataTypeWithBracket;

			var builder = new StringBuilder(dataTypeWithBracket);
			builder.Append(" ");
			for (int i = 0; i < bracketCount; ++i) {
				builder.Append("[]");
			}

			return builder.ToString();
		}
	}

	static void SetDataTypeWithBracketDepth(ref int depth, ref int bracketCount, ref string dataType, string str, int strIndex2, StreamReader reader)
	{
		int openBracket = str.IndexOf("[", strIndex2);
		if (-1 != openBracket) {
			depth++;
			if (depth > bracketCount)
				bracketCount = depth;
			strIndex2 = openBracket + 1;
		}

		// Bracket과 Data 사이의 공백 제거.
		str = TrimSpacesBeforeRealData(str.Substring(strIndex2));
		strIndex2 = 0;
		if (string.IsNullOrEmpty(str)) {
			// 현재 라인은 Bracket 밖에 없는 경우.
			str = reader.ReadLine();
			SetDataTypeWithBracketDepth(ref depth, ref bracketCount, ref dataType, str, strIndex2, reader);
			return;
		}

		// 공백 제거 후 나오는것이 Bracket이 아니라면 Data 다. dataType이 채워지지 않았다면 채워주자.
		if (string.IsNullOrEmpty(dataType) && str[0] != '[' && str[0] != ']') {
			dataType = CheckDataType(str, strIndex2);
		}

		int closeBracket = str.IndexOf("]", strIndex2);
		int openBracket2 = str.IndexOf("[", strIndex2);
		if (openBracket2 == -1) {
			if (-1 != closeBracket) {
				--depth;
				strIndex2 = closeBracket + 1;
				if (0 != depth) {
					if (closeBracket + 1 == str.Length) {
						// 닫기 Bracket 다음에 아무 텍스트도 없다면.
						str = reader.ReadLine();
						strIndex2 = 0;
					}
					SetDataTypeWithBracketDepth(ref depth, ref bracketCount, ref dataType, str, strIndex2, reader);
					return;
				}
			} else {
				// 이번 줄에 닫는 Bracket이 없다. 다음줄에서 탐색.
				strIndex2 = 0;
				str = reader.ReadLine();
				SetDataTypeWithBracketDepth(ref depth, ref bracketCount, ref dataType, str, strIndex2, reader);
			}
		} else {
			if (openBracket2 < closeBracket) {
				// 열린 Bracket이 있다. 처리.
				SetDataTypeWithBracketDepth(ref depth, ref bracketCount, ref dataType, str, openBracket2, reader);
			} else {
				// 닫는 Bracket과 Open이 둘 다 존재하므로 닫기 처리 후 열기 Bracket 다시 처리.
				--depth;
				SetDataTypeWithBracketDepth(ref depth, ref bracketCount, ref dataType, str, openBracket2, reader);
			}
		}
	}

	static string TrimSpacesBeforeRealData(string str)
	{
		int strIndex = 0;
		int spaceIndex = 0;
		while (strIndex < str.Length) {
			if (str[strIndex] == '\r' || str[strIndex] == '\n')
				return "";

			if (str[strIndex] == ' ' || str[strIndex] == '\t') {
				++spaceIndex;
				++strIndex;
			} else break;
		}

		return str.Substring(spaceIndex, str.Length - spaceIndex);
	}

	static string CheckDataType(string parseLine, int parseStartIndex)
	{
		int doubleQuotes = parseLine.IndexOf('\"', parseStartIndex);
		int dot = parseLine.IndexOf('.', parseStartIndex);

		if (-1 != doubleQuotes)
			return "string";

		if (-1 != dot)
			return "float";

		// 변수명과 Data 사이의 공백 제거.
		var builder = new StringBuilder(parseLine.Substring(parseStartIndex, parseLine.Length - parseStartIndex));
		while (builder.Length > 0) {
			if (builder[0] == ' ' || builder[0] == '\t')
				builder.Remove(0, 1);
			else
				break;
		}

		var data = builder.ToString();
		if (data.Equals("true") || data.Equals("true,") || data.Equals("false") || data.Equals("false,"))
			return "bool";

		return "int";
	}

	static string curDir = "";

	static string GetTableDirectory { get { 
		if (string.IsNullOrEmpty(curDir))
			curDir = Directory.GetCurrentDirectory();
			return $"{curDir}/Assets/Resources/Data";
		} 
	}

	static string GetTableDataSourceDirectory {
		get {
			if (string.IsNullOrEmpty(curDir))
				curDir = Directory.GetCurrentDirectory();
			return $"{curDir}/Assets/Scripts/DataTables";
		}
	}

	static string GetSourceDirectory {
		get {
			if (string.IsNullOrEmpty(curDir))
				curDir = Directory.GetCurrentDirectory();
			return $"{curDir}/Assets/Scripts";
		}
	}

	public static string GetCurrentDirectory {
		get {
			if (string.IsNullOrEmpty(curDir))
				curDir = Directory.GetCurrentDirectory();
			return curDir;
		}
	}

	static FileInfo [] GetFiles()
	{
		DirectoryInfo di = new DirectoryInfo(GetTableDirectory);
		return di.GetFiles();
	}
}
