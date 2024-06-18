// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

[Serializable]
public partial class MenuData
{
    public int Id;
    public int StatusType;
    public string MenuFileName = "";
    
    public static MenuTable Table;
    public MenuData ShallowCopy()
    {
    	return (MenuData) this.MemberwiseClone();
    }
}

public class MenuTable : TemplateTable<int, MenuData> { }
