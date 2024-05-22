using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("[ Setting ]")]
    public GameObject settingBox;

    [Header("[ Button ]")]
    private UIButton startButton;
    private UIButton settingButton;
    private UIButton exitButton;

    private string playSceneName = "PlayScene";

    void Awake()
    {
        IntroInit();
    }

    private void OnDestroy()
    {
        startButton.OnClick -= OnClickStart;
        settingButton.OnClick -= OnClickSetting;
        exitButton.OnClick -= OnClickExit;
    }

    void IntroInit()
    {
        if (startButton == null)
            startButton = GameObject.Find("StartBtn").GetComponent<UIButton>();

        if (settingButton == null)
            settingButton = GameObject.Find("SettingBtn").GetComponent<UIButton>();

        if (exitButton == null)
            exitButton = GameObject.Find("ExitBtn").GetComponent<UIButton>();

        startButton.OnClick += OnClickStart;
        settingButton.OnClick += OnClickSetting;
        exitButton.OnClick += OnClickExit;
    }

    void OnClickStart()
    {
        Debug.Log($"OnClickStart");
        SceneControlManager.Instance.LoadSceneType(SceneType.PlayScene);
    }

    void OnClickSetting()
    {
        Debug.Log($"OnClickSetting");

        if (settingBox == null)
        {
            Debug.Log($"SettingBox is Null");
            return;
        }

        settingBox.SetActive(!settingBox.activeSelf);
    }

    void OnClickExit()
    {
        Debug.Log($"OnClickExit");
        Application.Quit();
    }
}
