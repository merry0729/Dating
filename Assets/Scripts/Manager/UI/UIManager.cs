using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<SceneType, GameObject> sceneUIPrefab = new Dictionary<SceneType, GameObject>();

    [Header("[ UI Root ]")]
    public GameObject UICanvasPrefab;
    private GameObject UICanvas;
    public Transform UIParent;
    
    private GameObject settingUIObj;
    private GameObject currentSceneUI;

    [Header("[ UI Prefab ]")]
    public GameObject settingUIPrefab;

    [Header("[ Scene UI Prefab ]")]
    public GameObject introUIPrefab;
    public GameObject playUIPrefab;

    private void Awake()
    {
        if (_instance == null)
        {
            //DontDestroyOnLoad(this);
            Init();
        }
        else
        {
            Destroy(UICanvas);
            Destroy(this);
        }
    }

    private void Init()
    {
        if (UICanvas == null)
        {
            UICanvas = Instantiate(UICanvasPrefab);
            DontDestroyOnLoad(UICanvas);
        }

        if (UIParent == null)
            UIParent = UICanvas.transform.Find("SceneUI").transform;

        sceneUIPrefab.Add(SceneType.IntroScene, introUIPrefab);
        sceneUIPrefab.Add(SceneType.PlayScene, playUIPrefab);

        settingUIObj = LoadUI(settingUIPrefab);
        settingUIObj.SetActive(false);
        //DontDestroyOnLoad(settingUIObj);
    }

    public void ActiveWindowUI(WindowUIType windowUIType, bool isOn)
    {
        switch (windowUIType)
        {
            case WindowUIType.SettingUI:
                settingUIObj.SetActive(isOn);
                break;
        }
    }

    public void ActiveUI(GameObject uiObject, bool isOn)
    {
        uiObject.SetActive(isOn);
    }

    public GameObject LoadUI(GameObject uiObject)
    {
        GameObject loadedUI = Instantiate(uiObject, UICanvas.transform);
        loadedUI.SetActive(false);

        return loadedUI;
    }

    public void LoadSceneUI(SceneType sceneType)
    {
        if (currentSceneUI != null)
            Destroy(currentSceneUI);

        currentSceneUI = Instantiate(sceneUIPrefab[sceneType], UIParent.transform);
    }

    public GameObject GetCurrentSceneUI()
    {
        return currentSceneUI;
    }
}
