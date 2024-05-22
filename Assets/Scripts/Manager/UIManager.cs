using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    Dictionary<SceneType, GameObject> sceneUIPrefab = new Dictionary<SceneType, GameObject>();

    [Header("[ UI Root ]")]
    public Transform UICanvas;
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
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(this);

        Init();
    }

    private void Init()
    {
        if (UICanvas == null)
            UICanvas = GameObject.Find("Canvas").transform;

        if (UIParent == null)
            UIParent = GameObject.Find("CommonUI").transform;

        sceneUIPrefab.Add(SceneType.IntroScene, introUIPrefab);
        sceneUIPrefab.Add(SceneType.PlayScene, playUIPrefab);

        settingUIObj = LoadUI(settingUIPrefab);
        settingUIObj.SetActive(false);
        DontDestroyOnLoad(settingUIObj);
    }

    public void ActiveUI(GameObject uiObject, bool isOn)
    {
        uiObject.SetActive(isOn);
    }

    public GameObject LoadUI(GameObject uiObject)
    {
        GameObject loadedUI = Instantiate(uiObject, UICanvas);
        loadedUI.SetActive(false);

        return loadedUI;
    }

    public void LoadSceneUI(SceneType sceneType)
    {
        currentSceneUI = Instantiate(sceneUIPrefab[sceneType], UIParent);
    }
}
