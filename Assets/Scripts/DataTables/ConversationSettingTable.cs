// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

[Serializable]
public partial class ConversationSettingData
{
    public int Id;
    public string Name = "";
    public float [][] ConPos;
    
    public static ConversationSettingTable Table;
    public ConversationSettingData ShallowCopy()
    {
    	return (ConversationSettingData) this.MemberwiseClone();
    }
}

public class ConversationSettingTable : TemplateTable<int, ConversationSettingData> { }
