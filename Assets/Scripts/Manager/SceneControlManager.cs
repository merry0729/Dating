using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    IntroScene,
    PlayScene,
}

public class SceneControlManager : MonoBehaviour
{
    public static SceneControlManager Instance = null;

    [Header("[ Manager Obj ]")]
    public List<GameObject> managerList = new List<GameObject>();

    private GameObject currentSceneManager;

    Dictionary<SceneType, GameObject> sceneManagerDic = new Dictionary<SceneType, GameObject>();
    Dictionary<SceneType, string> sceneTypeStrDic = new Dictionary<SceneType, string>();
   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            Init();
        }
    }

    private void Init()
    {
        for(int index = 0; index < Enum.GetValues(typeof(SceneType)).Length; index++)
        {
            SceneType sceneType = (SceneType)index;

            if (!sceneManagerDic.ContainsKey(sceneType))
                sceneManagerDic.Add(sceneType, managerList[index]);

            if (!sceneTypeStrDic.ContainsKey(sceneType))
                sceneTypeStrDic.Add(sceneType, sceneType.ToString());
        }
        

        //foreach (var value in Enum.GetValues(typeof(SceneType)))
        //{
        //    SceneType sceneType = (SceneType)value;

        //    //if(!sceneManagerDic.ContainsKey(sceneType)) 
        //    //    sceneManagerDic.Add(sceneType, managerList[])

        //    if (!sceneTypeStrDic.ContainsKey(sceneType))
        //        sceneTypeStrDic.Add(sceneType, sceneType.ToString());
        //}

        //foreach (var dic in sceneTypeDic)
        //    Debug.Log($"{dic.Key}");

        SceneControl(SceneType.IntroScene);
    }

    public void LoadSceneType(SceneType sceneType)
    {
        Destroy(currentSceneManager);

        SceneManager.LoadScene(sceneTypeStrDic[sceneType]);
        SceneControl(sceneType);
    }

    void SceneControl(SceneType sceneType)
    {
        UIManager.Instance.LoadSceneUI(sceneType);
        LoadSceneManager(sceneType);
    }

    void LoadSceneManager(SceneType sceneType)
    {
        currentSceneManager = Instantiate(sceneManagerDic[sceneType], GameManager.Instance.transform);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            LoadSceneType(SceneType.IntroScene);
    }
}
