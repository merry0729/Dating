// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

public partial class SettingData
{
    public int Id;
    public string Name = "";
    public int Type;
    public string [] MainOption;
    public string [][] OptionDetails;
    
    public static SettingTable Table;
    public SettingData ShallowCopy()
    {
    	return (SettingData) this.MemberwiseClone();
    }
}

public class SettingTable : TemplateTable<int, SettingData> { }
