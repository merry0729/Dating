using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TemplateTable;
using UnityEngine;
using System.Threading.Tasks;

    public partial class TableLoader
    {
        public static bool IsReserved { get; private set; } = false;

        private static readonly MethodInfo _loadTableGenericMethodInfo =
            typeof(TableLoader).GetMethod("LoadTableGeneric", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo _loadTableGenericOverrideMethodInfo =
            typeof(TableLoader).GetMethod("LoadTableGenericOverride", BindingFlags.Instance | BindingFlags.NonPublic);

        public static Action OnTableLoadComplete;

        List<string> _loadedTable = new List<string>();

        // 모든 Load 시리즈 - LoadHeavySize, LoadNormalSize, LoadDependanciesOnly,... 의 마지막에 호출된다.
        void OnPostLoad()
        {
            Debug.Log($"{GetType()} - OnPostLoad()");

            Debug.Log($"{GetType()} - OnPostLoad() IsReserved = true");
            IsReserved = true;

            OnTableLoadComplete?.Invoke();
        }

        //Dictionary<string, bool> _dicLoadComplete = new Dictionary<string, bool>();
        int _tableLoadingCount = 0;
        public async void LoadTableGenericClient<T>(bool delayLoad, bool waitForCompletion = false) where T : class, new()
        {
            Debug.Log($"{typeof(T).Name}.Table load start....");

            var tableField = typeof(T).GetField("Table", BindingFlags.Static | BindingFlags.Public);
            if (null == tableField)
                throw new Exception($"{typeof(T).Name}.Table is not Exist");

            var table = (TemplateTable<int, T>)Activator.CreateInstance(tableField.FieldType);
            var tableName = typeof(T).Name.Substring(0, typeof(T).Name.Length - 4);

            ++_tableLoadingCount;

            TextAsset asset = null;
        //#if UNITY_EDITOR
        //if (!Application.isPlaying)
        //asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Resources/Data/{tableName}.json");
        asset = Resources.Load<TextAsset>($"Data/{tableName}");
        Debug.Log($"asset.text : {asset.text}");

//#endif

        table.Load(new TemplateTableJsonLoader<int, T>(
                new JsonTextReader(new StringReader(asset.text)), JsonSerializer.Create(), delayLoad));

        

            tableField.SetValue(null, table);
            _loadedTable.Add(tableName);
            --_tableLoadingCount;

            Debug.Log($"{typeof(T).Name}.Table loaded.");
        }

        public async UniTask LoadTableGenericServer<T>(bool delayLoad) where T : class, new()
        {

            Debug.Log($"{typeof(T).Name}.Table load start....");

            var tableField = typeof(T).GetField("Table", BindingFlags.Static | BindingFlags.Public);
            if (null == tableField)
            {
                Debug.Log($"{typeof(T).Name}.LoadData failed. #1....");
                throw new Exception($"{typeof(T).Name}.Table is not Exist");
            }            

            var table = (TemplateTable<int, T>)Activator.CreateInstance(tableField.FieldType);
            var tableName = typeof(T).Name.Substring(0, typeof(T).Name.Length - 4);            

            TextAsset asset = null;
#if UNITY_EDITOR 
            if (!Application.isPlaying)
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Resources/Data/{tableName}.json");
            
#else
#endif            

            if (null == asset)
            {                
                throw new Exception(string.Format("{0} LoadData failed.", tableName));
            }

            //if (null != SHGeneral.G.Instance && SHGeneral.G.Instance.isConsoleServer) 
            //{                
            //    // Dedi Server 는 켜질 때 기존에 돌아가고 있던 Dedi Server 의 CPU 자원을 최대한 덜 뺏아야 한다.                
            //    await UniTask.NextFrame();                
            //    var strReader = new JsonTextReader(new StringReader(asset.text));                
            //    await UniTask.NextFrame();                
            //    var jsonSerializer = JsonSerializer.Create();                
            //    await UniTask.NextFrame();                
            //    var templateLoader = new TemplateTableJsonLoader<int, T>(strReader, jsonSerializer, delayLoad);                
            //    await UniTask.NextFrame();                
            //    table.Load(templateLoader);                
            //    await UniTask.NextFrame();                
            //    tableField.SetValue(null, table);                
            //    await UniTask.NextFrame();
                
            //} 
            //else 
            //{                
                table.Load(new TemplateTableJsonLoader<int, T>(
                    new JsonTextReader(new StringReader(asset.text)), JsonSerializer.Create(), delayLoad));                
                tableField.SetValue(null, table);
            //}            

            _loadedTable.Add(tableName);
            //if (SHGeneral.G.Instance.isConsoleServer)
            //    await UniTask.Delay(10);
            Debug.Log($"{typeof(T).Name}.Table loaded.");
        }

        public void LoadTableGenericOverride<T>(bool delayLoad) where T : class, new()
        {
            var tableField = typeof(T).GetField("Table", BindingFlags.Static | BindingFlags.Public);
            if (null == tableField)
                throw new Exception($"{typeof(T).Name}.Table is not Exist");

            var table = (TemplateTable<int, T>)Activator.CreateInstance(tableField.FieldType);
            var tableName = typeof(T).Name.Substring(0, typeof(T).Name.Length - 4);
            string path = $"{Application.streamingAssetsPath}/Table/{tableName}.json";
            if (false == File.Exists(path))
                return;

            string jsonStr = File.ReadAllText(path);
            table.Load(new TemplateTableJsonLoader<int, T>(
                new JsonTextReader(new StringReader(jsonStr)), JsonSerializer.Create(), delayLoad));

            tableField.SetValue(null, table);
            _loadedTable.Add(tableName);
            //Debug.Log($"Table Overrided - [{tableName}]");
        }

        /// <summary>
        /// Web HTTP 에서 받은 Json String을 변환하기 위함
        /// </summary>
        public static void LoadTableGenericString<T>(string jsonStr, bool delayLoad) where T : class, new()
        {
            var tableField = typeof(T).GetField("Table", BindingFlags.Static | BindingFlags.Public);
            if (null == tableField)
                throw new Exception($"{typeof(T).Name}.Table is not Exist");

            var table = (TemplateTable<int, T>)Activator.CreateInstance(tableField.FieldType);
            var tableName = typeof(T).Name.Substring(0, typeof(T).Name.Length - 4);

            table.Load(new TemplateTableJsonLoader<int, T>(
                new JsonTextReader(new StringReader(jsonStr)), JsonSerializer.Create(), delayLoad));

            tableField.SetValue(null, table);

            Debug.Log($"Table GenericString - [{tableName}]");
        }

#if UNITY_EDITOR

        public static void LoadTableGenericClientEditor<T>() where T : class, new()
        {         
            var tableField = typeof(T).GetField("Table", BindingFlags.Static | BindingFlags.Public);
            if (null == tableField)
                throw new Exception($"{typeof(T).Name}.Table is not Exist");

            var table = (TemplateTable<int, T>)Activator.CreateInstance(tableField.FieldType);
            var tableName = typeof(T).Name.Substring(0, typeof(T).Name.Length - 4);
            
            TextAsset file = null;

            file = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Resources_moved/Table/{tableName}.json");
            //file = Addressables.LoadAssetAsync<TextAsset>($"Table/{tableName}.json").WaitForCompletion();

            table.Load(new TemplateTableJsonLoader<int, T>( new JsonTextReader(new StringReader(file.text)), JsonSerializer.Create(), false));
            tableField.SetValue(null, table);
            //var file = Resources.Load<TextAsset>("Table/" + tableName);
        }
#endif
    }

