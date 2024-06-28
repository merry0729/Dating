// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using TemplateTable;

[Serializable]
public partial class AlbumData
{
    public int Id;
    public string Album_Illust_FileName = "";
    public string Album_Character_FileName = "";
    
    public static AlbumTable Table;
    public AlbumData ShallowCopy()
    {
    	return (AlbumData) this.MemberwiseClone();
    }
}

public class AlbumTable : TemplateTable<int, AlbumData> { }
