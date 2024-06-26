// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

[Serializable]
public partial class ConversationData
{
    public int Id;
    public string Name = "";
    public int Who;
    public int ConPos;
    public int CharPos;
    public int CharScale;
    public string Text = "";
    public int Next;
    public bool Hold;
    public bool RemoveHold;
    public int Opiton;
    
    public static ConversationTable Table;
    public ConversationData ShallowCopy()
    {
    	return (ConversationData) this.MemberwiseClone();
    }
}

public class ConversationTable : TemplateTable<int, ConversationData> { }
