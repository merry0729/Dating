// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

[Serializable]
public partial class MessageData
{
    public int Id;
    public int Speaker_Heroin1;
    public bool Continue_Heroin1;
    public string Message_Heroin1 = "";
    public int Speaker_Heroin2;
    public bool Continue_Heroin2;
    public string Message_Heroin2 = "";
    public int Speaker_Heroin3;
    public bool Continue_Heroin3;
    public string Message_Heroin3 = "";
    
    public static MessageTable Table;
    public MessageData ShallowCopy()
    {
    	return (MessageData) this.MemberwiseClone();
    }
}

public class MessageTable : TemplateTable<int, MessageData> { }
