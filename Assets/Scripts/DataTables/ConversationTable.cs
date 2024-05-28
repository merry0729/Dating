// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

[Serializable]
public partial class ConversationData
{
    public int Id;
    public string Name = "";
    public string Type = "";
    public int Pos;
    public int Scale;
    public string Text = "";
    public int Next;
    
    public static ConversationTable Table;
    public ConversationData ShallowCopy()
    {
    	return (ConversationData) this.MemberwiseClone();
    }
}

public class ConversationTable : TemplateTable<int, ConversationData> { }
