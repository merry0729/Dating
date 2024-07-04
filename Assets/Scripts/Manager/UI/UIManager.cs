using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    SerializableDictionary<SceneType, GameObject> sceneUIPrefab = new SerializableDictionary<SceneType, GameObject>();

    [Header("[ UI Root ]")]
    public GameObject UICanvasPrefab;
    private GameObject UICanvas;
    public Transform sceneUI;
    public Transform commonUI;

    [Header("[ UI Play ]")]
    public GameObject playUI;
    public GameObject optionsUI;
    public GameObject characterPlayingUI;

    [Header("[ UI Resolution ]")]
    public List<RectTransform> resolutionRect;

    [Header("[ UI Common ]")]
    public GameObject settingUIObj;
    public GameObject confirmUIObj;

    UIConfirm uiConfirm;


    [Header("[ UI Prefab ]")]
    public GameObject settingUIPrefab;
    public GameObject confirmUIPrefab;

    [Header("[ Scene UI Prefab ]")]
    public GameObject introUIPrefab;
    public GameObject playUIPrefab;

    public GameObject currentSceneUI;

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

        if (commonUI == null)
        {
            commonUI = UICanvas.transform.Find("CommonUI").transform;
        }


        sceneUIPrefab.Add(SceneType.IntroScene, introUIPrefab);
        sceneUIPrefab.Add(SceneType.PlayScene, playUIPrefab);

        settingUIObj = LoadUI(settingUIPrefab);
        confirmUIObj = LoadUI(confirmUIPrefab);
        uiConfirm = confirmUIObj.GetComponent<UIConfirm>();
        //DontDestroyOnLoad(settingUIObj);
    }

    public void ActiveWindowUI(WindowUIType windowUIType, bool isOn)
    {
        switch (windowUIType)
        {
            case WindowUIType.SettingUI:
                settingUIObj.SetActive(isOn);
                break;
            case WindowUIType.ConfirmUI:
                confirmUIObj.SetActive(isOn);
                break;
        }

        Debug.Log($"windowUIType : {windowUIType} / isOn : {isOn}");
        WindowOpenAction.Invoke(windowUIType, isOn);
    }

    public bool GetActiveWindowUI(WindowUIType windowUIType)
    {
        switch (windowUIType)
        {
            case WindowUIType.SettingUI:
                return settingUIObj.activeSelf;
            case WindowUIType.ConfirmUI:
                return confirmUIObj.activeSelf;
            default:
                return false;
        }
    }

    public void ActiveUI(GameObject uiObject, bool isOn)
    {
        uiObject.SetActive(isOn);
    }

    public GameObject LoadUI(GameObject uiObject)
    {
        GameObject loadedUI = Instantiate(uiObject, commonUI);
        return loadedUI;
    }

    public void LoadSceneUI(SceneType sceneType)
    {
        if (currentSceneUI != null)
            Destroy(currentSceneUI);

        currentSceneUI = Instantiate(sceneUIPrefab[sceneType], sceneUI);

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
            Debug.Log($"Play Complete");

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
