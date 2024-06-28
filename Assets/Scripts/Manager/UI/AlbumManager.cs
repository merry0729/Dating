using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlbumManager : Singleton<AlbumManager>
{
    public Transform albumParent;
    public ScrollRect albumScrollRect;
    public Transform albumContent;

    public GameObject albumIllustPrefab;

    AlbumTable albumTable;

    public void SetAlbumUI()
    {
        albumTable = AlbumData.Table;

        albumParent = PhoneManager.Instance.albumParent;
        albumScrollRect = albumParent.Find("Album_Scroll").GetComponent<ScrollRect>();
        albumContent = albumScrollRect.transform.Find("Viewport").Find("Content");

        LoadAlbum();
    }

    void LoadAlbum()
    {
        for (int index = 0; index < 10; index++)
        {
            UIAlbum_Illust albumIllust;

            albumIllust = Instantiate(albumIllustPrefab, albumContent).GetComponent<UIAlbum_Illust>();
            albumIllust.SetAlbumIllust(index);
        }
    }
}
