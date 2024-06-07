using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : Singleton<IntroManager>
{
    [Header("[ Button ]")]
    private UIButton startButton;
    private UIButton settingButton;
    private UIButton exitButton;

    private string playSceneName = "PlayScene";

    private void OnDestroy()
    {
        startButton.OnClick -= OnClickStart;
        settingButton.OnClick -= OnClickSetting;
        exitButton.OnClick -= OnClickExit;
    }

    public void SetIntroUI()
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
        UIManager.Instance.ActiveWindowUI(WindowUIType.SettingUI, true);
    }

    void OnClickExit()
    {
        Debug.Log($"OnClickExit");
        Application.Quit();
    }
}
