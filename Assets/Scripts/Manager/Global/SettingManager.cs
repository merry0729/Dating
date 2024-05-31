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
        settingTable = SettingData.Table;
        videoData = settingTable.TryGet((int)SettingType.Video);
        soundData = settingTable.TryGet((int)SettingType.Sound);
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

        resolutionDrop = videoWindow.Find($"Resolution").Find($"Resolution_Dropdown").GetComponent<TMP_Dropdown>();
        resolutionDrop.onValueChanged.AddListener(OnDropDownValueChanged);

        #endregion

        UIManager.Instance.ActiveWindowUI(WindowUIType.SettingUI, false);
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

    void AddOptions(TMP_Dropdown dropDown, string[] optionTexts)
    {
        foreach (string optionText in optionTexts)
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
            newOption.text = optionText;
            dropDown.options.Add(newOption);
        }

    }

    void OnDropDownValueChanged(int value)
    {

    }
}
