using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingManager : Singleton<SettingManager>
{

    #region [ Setting Base ]
    SettingType currentSettingType = SettingType.Video;

    GameObject settingUI;
    Transform settingButtonParent;
    Transform settingWindowParent;

    public UIButton videoBtn;
    public UIButton soundBtn;
    public UIButton exitBtn;

    Dictionary<SettingType, GameObject> settingWindowDic = new Dictionary<SettingType, GameObject>();

    public Transform videoWindow;
    public Transform soundWindow;

    #endregion

    #region [ Video Setting ]

    TextMeshProUGUI resolutionText;
    TMP_Dropdown resolutionDrop;

    TextMeshProUGUI screenModeText;
    TMP_Dropdown screenModeDrop;

    #endregion

    #region [ Sound Setting ]

    TextMeshProUGUI soundText;

    #endregion

    SettingTable settingTable;
    SettingData videoData;
    SettingData soundData;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SetTable();
        SetOption();
    }

    private void OnDestroy()
    {
        videoBtn.OnClick -= () => OnClickSetting(SettingType.Video);
        soundBtn.OnClick -= () => OnClickSetting(SettingType.Sound);
        exitBtn.OnClick -= OnClickExit;
    }

    void Init()
    {
        #region [ Setting Base Init ]

        settingUI = UIManager.Instance.settingUIObj;
        settingButtonParent = settingUI.transform.Find("SettingButton");
        settingWindowParent = settingUI.transform.Find("SettingWindow");

        videoBtn = settingButtonParent.Find("Video").GetComponent<UIButton>();
        soundBtn = settingButtonParent.Find("Sound").GetComponent<UIButton>();
        exitBtn = settingButtonParent.Find("Exit").GetComponent<UIButton>();

        videoWindow = settingWindowParent.Find("Video");
        soundWindow = settingWindowParent.Find("Sound");

        settingWindowDic.Add(SettingType.Video, videoWindow.gameObject);
        settingWindowDic.Add(SettingType.Sound, soundWindow.gameObject);

        videoBtn.OnClick += () => OnClickSetting(SettingType.Video);
        soundBtn.OnClick += () => OnClickSetting(SettingType.Sound);
        exitBtn.OnClick += OnClickExit;

        UIManager.Instance.WindowOpenAction += (type, isOn) =>
        {
            if (type == WindowUIType.SettingUI && isOn)
                OnClickSetting(SettingType.Video);
        };

        #endregion

        #region [ Video Setting Init ]

        resolutionText = videoWindow.Find($"Resolution").Find($"Text (TMP)").GetComponent<TextMeshProUGUI>();
        resolutionDrop = videoWindow.Find($"Resolution").Find($"Resolution_Dropdown").GetComponent<TMP_Dropdown>();

        screenModeText = videoWindow.Find($"ScreenMode").Find($"Text (TMP)").GetComponent<TextMeshProUGUI>();
        screenModeDrop = videoWindow.Find($"ScreenMode").Find($"ScreenMode_Dropdown").GetComponent<TMP_Dropdown>();

        #endregion

        UIManager.Instance.ActiveWindowUI(WindowUIType.SettingUI, false);
    }

    void SetTable()
    {
        settingTable = SettingData.Table;
        videoData = settingTable.TryGet((int)SettingType.Video);
        soundData = settingTable.TryGet((int)SettingType.Sound);
    }

    void OpenWindow(SettingType settingType)
    {
        Debug.Log($"settingType : {settingType}");

        if (settingType != currentSettingType)
        {
            settingWindowDic[currentSettingType].SetActive(false);
            settingWindowDic[settingType].SetActive(true);
            currentSettingType = settingType;
        }
        else
            return;
    }

    void OnClickSetting(SettingType settingType)
    {
        Debug.Log($"OnClickSetting");
        OpenWindow(settingType);
    }

    void OnClickExit()
    {
        UIManager.Instance.ActiveWindowUI(WindowUIType.SettingUI, false);
    }

    void SetOption()
    {
        Debug.Log($"videoData.OptionDetails[(int)SettingType.Video] : {videoData.OptionDetails[(int)SettingType.Video].Length}");
        Debug.Log($"videoData.OptionDetails[(int)SettingType.Video] : {videoData.OptionDetails[(int)SettingType.Video][0]}");

        resolutionText.text = videoData.MainOption[(int)DropDownType.Resolution];
        resolutionDrop.onValueChanged.AddListener(delegate { OnDropDownValueChanged(DropDownType.Resolution); });
        AddOptions(resolutionDrop, videoData.OptionDetails[(int)DropDownType.Resolution]);
        
        screenModeText.text = videoData.MainOption[(int)DropDownType.ScreenMode];
        screenModeDrop.onValueChanged.AddListener(delegate { OnDropDownValueChanged(DropDownType.ScreenMode); });
        AddOptions(screenModeDrop, videoData.OptionDetails[(int)DropDownType.ScreenMode]);
    }

    void AddOptions(TMP_Dropdown dropDown, string[] optionTexts)
    {
        foreach (string optionText in optionTexts)
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
            newOption.text = optionText;
            dropDown.options.Add(newOption);
        }
    }

    void OnDropDownValueChanged(DropDownType ddType)
    {
        TMP_Dropdown dd = null;

        switch(ddType)
        {
            case DropDownType.Resolution:
                dd = resolutionDrop;

                if (dd.value == (int)ResolutionType.W800_H600)
                    Screen.SetResolution(800, 600, true);
                else if (dd.value == (int)ResolutionType.W1920_H1080)
                    Screen.SetResolution(1920, 1080, true);
                else if (dd.value == (int)ResolutionType.W2560_H1440)
                    Screen.SetResolution(2560, 1440, true);
                break;

            case DropDownType.ScreenMode:
                if (dd.value == (int)ScreenType.FullScreen)
                    Screen.fullScreen = true;
                else if (dd.value == (int)ScreenType.WindowScreen)
                    Screen.fullScreen = false;
                break;
        }
    }
}
