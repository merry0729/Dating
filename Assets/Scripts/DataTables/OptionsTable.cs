// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

[Serializable]
public partial class OptionsData
{
    public int Id;
    public string Name = "";
    public int [] OptionType;
    public string [] Options;
    
    public static OptionsTable Table;
    public OptionsData ShallowCopy()
    {
    	return (OptionsData) this.MemberwiseClone();
    }
}

public class OptionsTable : TemplateTable<int, OptionsData> { }
