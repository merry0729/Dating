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

    public SerializableDictionary<SettingType, GameObject> settingWindowDic = new SerializableDictionary<SettingType, GameObject>();

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

    TextMeshProUGUI masterVolumeText;
    Slider masterVolumeSlider;

    TextMeshProUGUI bgmVolumeText;
    Slider bgmVolumeSlider;

    TextMeshProUGUI voiceVolumeText;
    Slider voiceVolumeSlider;

    TextMeshProUGUI sfxVolumeText;
    Slider sfxVolumeSlider;

    #endregion

    SettingTable settingTable;
    SettingData videoData;
    SettingData soundData;

    private void Awake()
    {
        Init();
        SetTable();
        SetVideoOption();
        SetSoundOption();
    }

    private void Start()
    {
  
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

        #region [ Video Setting Init ]

        masterVolumeText = soundWindow.Find("Master Volume").Find($"Text (TMP)").GetComponent<TextMeshProUGUI>();
        masterVolumeSlider = soundWindow.Find("Master Volume").Find($"Master Slider").GetComponent<Slider>();

        bgmVolumeText = soundWindow.Find("BGM Volume").Find($"Text (TMP)").GetComponent<TextMeshProUGUI>();
        bgmVolumeSlider = soundWindow.Find("BGM Volume").Find($"BGM Slider").GetComponent<Slider>();

        voiceVolumeText = soundWindow.Find("Voice Volume").Find($"Text (TMP)").GetComponent<TextMeshProUGUI>();
        voiceVolumeSlider = soundWindow.Find("Voice Volume").Find($"Voice Slider").GetComponent<Slider>();

        sfxVolumeText = soundWindow.Find("SFX Volume").Find($"Text (TMP)").GetComponent<TextMeshProUGUI>();
        sfxVolumeSlider = soundWindow.Find("SFX Volume").Find($"SFX Slider").GetComponent<Slider>();

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

    void SetVideoOption()
    {
        Debug.Log($"videoData.OptionDetails[(int)SettingType.Video] : {videoData.OptionDetails[(int)SettingType.Video].Length}");
        Debug.Log($"videoData.OptionDetails[(int)SettingType.Video] : {videoData.OptionDetails[(int)SettingType.Video][0]}");

        resolutionText.text = videoData.MainOption[(int)DropDownType.Resolution];
        resolutionDrop.onValueChanged.AddListener(delegate { OnDropDownValueChanged(resolutionDrop, DropDownType.Resolution); });
        AddOptions(resolutionDrop, videoData.OptionDetails[(int)DropDownType.Resolution]);
        
        screenModeText.text = videoData.MainOption[(int)DropDownType.ScreenMode];
        screenModeDrop.onValueChanged.AddListener(delegate { OnDropDownValueChanged(screenModeDrop, DropDownType.ScreenMode); });
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

    void SetSoundOption()
    {
        masterVolumeText.text = soundData.MainOption[(int)SoundType.Master];
        masterVolumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(masterVolumeSlider, SoundType.Master); });

        bgmVolumeText.text = soundData.MainOption[(int)SoundType.BGM];
        bgmVolumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(bgmVolumeSlider, SoundType.BGM); });

        voiceVolumeText.text = soundData.MainOption[(int)SoundType.Voice];
        voiceVolumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(voiceVolumeSlider, SoundType.Voice); });

        sfxVolumeText.text = soundData.MainOption[(int)SoundType.SFX];
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(sfxVolumeSlider, SoundType.SFX); });
    }

    void OnDropDownValueChanged(TMP_Dropdown dd, DropDownType ddType)
    {
        switch(ddType)
        {
            case DropDownType.Resolution:
                if (dd.value == (int)ResolutionType.W800_H600)
                    Screen.SetResolution(800, 600, Screen.fullScreen);
                else if (dd.value == (int)ResolutionType.W1920_H1080)
                    Screen.SetResolution(1920, 1080, Screen.fullScreen);
                else if (dd.value == (int)ResolutionType.W2560_H1440)
                    Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;

            case DropDownType.ScreenMode:
                if (dd.value == (int)ScreenType.FullScreen)
                    Screen.fullScreen = true;
                else if (dd.value == (int)ScreenType.WindowScreen)
                    Screen.fullScreen = false;
                break;
        }

        Debug.Log($"dd : {dd.name} / {ddType}");
        Debug.Log($"resolution : {Screen.width} / {Screen.height}");
        Debug.Log($"resolution : {Screen.currentResolution}");
    }

    void OnSliderValueChanged(Slider slider, SoundType type)
    {
        SoundManager.Instance.VolumeControl(type, slider.value);
        Debug.Log($"{type} / {slider.value}");
    }
}