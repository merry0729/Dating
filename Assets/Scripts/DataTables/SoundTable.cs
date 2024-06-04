// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

public partial class SoundData
{
    public int Id;
    public string Name = "";
    public int Type;
    public string SoundType = "";
    public string SoundFileName = "";
    
    public static SoundTable Table;
    public SoundData ShallowCopy()
    {
    	return (SoundData) this.MemberwiseClone();
    }
}

public class SoundTable : TemplateTable<int, SoundData> { }
