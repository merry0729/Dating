using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlbumManager : Singleton<AlbumManager>
{
    public Transform albumParent;
    public ScrollRect albumScrollRect;
    public Transform albumContent;

    public GameObject albumPrefab;

    public void SetAlbumUI()
    {
        albumParent = PhoneManager.Instance.albumParent;
        albumScrollRect = albumParent.Find("Album_Scroll").GetComponent<ScrollRect>();
        albumContent = albumScrollRect.transform.Find("Viewport").Find("Content");

        LoadAlbum();
    }

    void LoadAlbum()
    {

    }

}
