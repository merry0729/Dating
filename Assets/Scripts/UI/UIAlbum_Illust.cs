using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAlbum_Illust : MonoBehaviour
{
    public Image illustImg;

    string illustFilePath;

    public AlbumData albumData;

    public void SetAlbumIllust(int index)
    {
        albumData = AlbumData.Table.TryGet(index);

        SetAlbumIllustUI();
        UpdateAlbumIllustUI();
    }

    void SetAlbumIllustUI()
    {
        if(illustImg == null)
            illustImg = GetComponent<Image>();
    }

    void UpdateAlbumIllustUI()
    {
        Debug.Log($"albumData.Album_Illust_FileName : {albumData.Album_Illust_FileName}");
        //illustImg.sprite = Resources.Load<Sprite>(illustFilePath + albumData.Album_Illust_FileName);
    }
}
