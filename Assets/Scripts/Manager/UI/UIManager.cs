using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    SerializableDictionary<SceneType, GameObject> sceneUIPrefab = new SerializableDictionary<SceneType, GameObject>();

    [Header("[ UI Root ]")]
    public GameObject UICanvasPrefab;
    private GameObject UICanvas;
    public Transform sceneUI;

    [Header("[ UI Play ]")]
    public GameObject playUI;
    public GameObject optionsUI;
    public GameObject characterPlayingUI;

    [Header("[ UI Resolution ]")]
    public List<RectTransform> resolutionRect;

    public GameObject settingUIObj;
    public GameObject currentSceneUI;

    [Header("[ UI Prefab ]")]
    public GameObject settingUIPrefab;

    [Header("[ Scene UI Prefab ]")]
    public GameObject introUIPrefab;
    public GameObject playUIPrefab;

    public Action<WindowUIType, bool> WindowOpenAction;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            ScreenTransform();
    }

    private void Init()
    {
        if (UICanvas == null)
        {
            UICanvas = Instantiate(UICanvasPrefab);
            DontDestroyOnLoad(UICanvas);
        }

        if (sceneUI == null)
        {
            sceneUI = UICanvas.transform.Find("SceneUI").transform;
            
        }


        sceneUIPrefab.Add(SceneType.IntroScene, introUIPrefab);
        sceneUIPrefab.Add(SceneType.PlayScene, playUIPrefab);

        settingUIObj = LoadUI(settingUIPrefab);
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

        Debug.Log($"windowUIType : {windowUIType} / isOn : {isOn}");
        WindowOpenAction.Invoke(windowUIType, isOn);
    }

    public void ActiveUI(GameObject uiObject, bool isOn)
    {
        uiObject.SetActive(isOn);
    }

    public GameObject LoadUI(GameObject uiObject)
    {
        GameObject loadedUI = Instantiate(uiObject, UICanvas.transform);

        return loadedUI;
    }

    public void LoadSceneUI(SceneType sceneType)
    {
        if (currentSceneUI != null)
            Destroy(currentSceneUI);

        currentSceneUI = Instantiate(sceneUIPrefab[sceneType], sceneUI.transform);

        if(resolutionRect.Count > 0)
            resolutionRect.Clear();

        resolutionRect.Add(sceneUI.GetComponent<RectTransform>());
        resolutionRect.Add(currentSceneUI.GetComponent<RectTransform>());

        if (sceneType == SceneType.IntroScene)
        {
            IntroManager.Instance.SetIntroUI();

            //resolutionRect.Add();
        }
        else if(sceneType == SceneType.PlayScene)
        {
            PlayManager.Instance.SetPlayUI();
            PhoneManager.Instance.SetPhoneUI();

            resolutionRect.Add(currentSceneUI.transform.Find("Options").GetComponent<RectTransform>());
            resolutionRect.Add(currentSceneUI.transform.Find("Menu").GetComponent<RectTransform>());
        }
    }

    public GameObject GetCurrentSceneUI()
    {
        return currentSceneUI;
    }

    public void ScreenTransform()
    {
        //UIParentRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        //UIParentRect.sizeDelta = new Vector2(2560, 1920);
    }
}
