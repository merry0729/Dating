// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

[Serializable]
public partial class CharacterData
{
    public int Id;
    public string Name = "";
    public int Type;
    public string ImgFileName = "";
    
    public static CharacterTable Table;
    public CharacterData ShallowCopy()
    {
    	return (CharacterData) this.MemberwiseClone();
    }
}

public class CharacterTable : TemplateTable<int, CharacterData> { }
