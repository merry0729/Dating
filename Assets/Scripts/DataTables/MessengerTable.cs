// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

public partial class MessengerData
{
    public int Id;
    public string Name = "";
    public int Who;
    public string Text = "";
    
    public static MessengerTable Table;
    public MessengerData ShallowCopy()
    {
    	return (MessengerData) this.MemberwiseClone();
    }
}

public class MessengerTable : TemplateTable<int, MessengerData> { }
