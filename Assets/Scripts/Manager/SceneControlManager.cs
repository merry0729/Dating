using System;
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

    Dictionary<SceneType, string> sceneTypeDic = new Dictionary<SceneType, string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        DontDestroyOnLoad(this);

        Init();
    }

    private void Init()
    {
        foreach (var value in Enum.GetValues(typeof(SceneType)))
        {
            SceneType sceneType = (SceneType)value;

            if (!sceneTypeDic.ContainsKey(sceneType))
                sceneTypeDic.Add(sceneType, sceneType.ToString());
        }

        //foreach (var dic in sceneTypeDic)
        //    Debug.Log($"{dic.Key}");

        SceneControl(SceneType.IntroScene);
    }

    public void LoadSceneType(SceneType sceneType)
    {
        SceneManager.LoadScene(sceneTypeDic[sceneType]);
        SceneControl(sceneType);
    }

    void SceneControl(SceneType sceneType)
    {
        UIManager.Instance.LoadSceneUI(sceneType);

        switch(sceneType)
        {
            case SceneType.IntroScene:
                break;

            case SceneType.PlayScene:
                break;
        }
    }
}
