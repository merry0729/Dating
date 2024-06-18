// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

[Serializable]
public partial class CharacterSettingData
{
    public int Id;
    public string Name = "";
    public int Type;
    public float [][] Vec;
    
    public static CharacterSettingTable Table;
    public CharacterSettingData ShallowCopy()
    {
    	return (CharacterSettingData) this.MemberwiseClone();
    }
}

public class CharacterSettingTable : TemplateTable<int, CharacterSettingData> { }
