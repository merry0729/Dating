using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class PlayManager : Singleton<PlayManager>
{
    #region [ Play Object ]

    [Header(" [ Play Object ] ")]
    public GameObject playObjectPrefab;
    private GameObject playObject;

    private Background illust_Background;

    #endregion

    #region [ Play UI Object ]

    [Header(" [ Play UI Object ] ")]
    public GameObject rightMenuPrefab;
    public Transform menuParent;
    public Transform rightMenuParent;

    #endregion

    #region [ Status ]

    [Header(" [ Play UI Object ] ")]
    public GameObject statusPrefab_Info;
    public GameObject statusPrefab_Char;

    public Transform statusParent;
    public Transform statusParent_Info;
    public Transform statusParent_Char;

    public SerializableDictionary<InfoStatusType, UIStatus> infoStatusDic = new SerializableDictionary<InfoStatusType, UIStatus>();
    public SerializableDictionary<CharStatusType, UIStatus> charStatusDic = new SerializableDictionary<CharStatusType, UIStatus>();

    #endregion

    #region [ Conversation Character Declare ]

    [Header(" [ Conversation Character ] ")]
    public Character illust_Character_Main;

    public CharacterSettingTable characterSettingTable;
    public CharacterSettingData characterSettingData;

    List<Vector3> charPosList = new List<Vector3>();
    List<Vector3> charScaleList = new List<Vector3>();

    #endregion

    #region [ Options Declare ]

    [Header(" [ Options ] ")]

    public Transform optionParent;
    public Transform optionBackground;

    #endregion

    #region [ Phone ]

    [Header(" [ Phone ] ")]
    public GameObject phoneParent;

    #endregion

    #region [ Common ]

    public UIButton emptyBtn;
    public SerializableDictionary<PlayUIType, GameObject> parentDic = new SerializableDictionary<PlayUIType, GameObject>();
    public SerializableDictionary<PlayUIType, CanvasGroup> canvasGroupDic = new SerializableDictionary<PlayUIType, CanvasGroup>();

    #endregion


    private void Awake()
    {
        SetPlayObject();
        SetCharacterSettingData();
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        emptyBtn.OnClick -= OnClickEmpty;
    }

    public void PlayStart()
    {
        ConversationManager.Instance.StartConversation();
    }

    void SetPlayObject()
    {
        playObject = Instantiate(playObjectPrefab);
        illust_Background = playObject.transform.Find("Illust_Background").GetComponent<Background>();
        illust_Character_Main = playObject.transform.Find("Illust_Character_Main").GetComponent<Character>();
    }

    public void SetPlayUI()
    {
        // Menu 오브젝트 등록
        menuParent = UIManager.Instance.GetCurrentSceneUI().transform.Find("Menu");
        rightMenuParent = menuParent.Find("RightMenu");
        parentDic.Add(PlayUIType.Menu, menuParent.gameObject);
        canvasGroupDic.Add(PlayUIType.Menu, menuParent.GetComponent<CanvasGroup>());

        // Status 오브젝트 등록
        statusParent = UIManager.Instance.GetCurrentSceneUI().transform.Find("Status");
        statusParent_Info = statusParent.transform.Find("Status_Back").transform.Find("Status_Info");
        statusParent_Char = statusParent.transform.Find("Status_Back").transform.Find("Status_Char");
        parentDic.Add(PlayUIType.Status, statusParent.gameObject);
        canvasGroupDic.Add(PlayUIType.Status, statusParent.GetComponent<CanvasGroup>());

        // Option 오브젝트 등록
        optionParent = UIManager.Instance.GetCurrentSceneUI().transform.Find("Options");
        optionBackground = optionParent.Find("Option_Background").transform;
        parentDic.Add(PlayUIType.Option, optionParent.gameObject);
        canvasGroupDic.Add(PlayUIType.Option, optionParent.GetComponent<CanvasGroup>());

        // Phone 오브젝트 등록
        phoneParent = UIManager.Instance.GetCurrentSceneUI().transform.Find("Phone").gameObject;
        parentDic.Add(PlayUIType.Phone, phoneParent);
        canvasGroupDic.Add(PlayUIType.Phone, phoneParent.GetComponent<CanvasGroup>());

        // Common 오브젝트 등록
        emptyBtn = UIManager.Instance.GetCurrentSceneUI().transform.Find("EmptyPlace").GetComponent<UIButton>();
        emptyBtn.OnClick += OnClickEmpty;

        //----------------
        // Set Method
        SetMenus();
        SetStatus();

        // Update Method
        UpdateStatus(InfoStatusType.Time, 50f);
        UpdateStatus(InfoStatusType.Cash, 30f);

        UpdateStatus(CharStatusType.Health, 100f);
        UpdateStatus(CharStatusType.Stress, 5f);
        UpdateStatus(CharStatusType.Hungry, 25f);
    }

    #region [ Background ]

    public void SetBackground()
    {

    }

    #endregion

    #region [ Menu ]

    void SetMenus()
    {
        for (int index = 0; index < Enum.GetValues(typeof(MenuType)).Length; index++)
        {
            UIMenu menu = Instantiate(rightMenuPrefab, rightMenuParent).GetComponent<UIMenu>();
            menu.transform.SetAsFirstSibling();
            menu.SetMenu((MenuType)index);
        }
    }

    #endregion

    #region [ Character Play ]

    public void SetChracter(int charIndex, int posIndex, int scaleIndex)
    {
        switch(charIndex)
        {
            case (int)CharType.Main:
                illust_Character_Main.UpdateTrans(charPosList[posIndex], charScaleList[scaleIndex]);
                break;
            case (int)CharType.Heroin_1:
                illust_Character_Main.UpdateTrans(charPosList[posIndex], charScaleList[scaleIndex]);
                break;
            case (int)CharType.Heroin_2:
                illust_Character_Main.UpdateTrans(charPosList[posIndex], charScaleList[scaleIndex]);
                break;
            case (int)CharType.Heroin_3:
                illust_Character_Main.UpdateTrans(charPosList[posIndex], charScaleList[scaleIndex]);
                break;
        }
    }


    void SetCharacterSettingData()
    {
        // CharacterSetting Table.
        characterSettingTable = CharacterSettingData.Table;
        // CharacterSetting Pos.
        characterSettingData = characterSettingTable.TryGet((int)CharDataType.Pos);

        // CharacterSetting Pos Data.
        charPosList.Add(new Vector3(characterSettingData.Vec[(int)CharPosType.Left][0],
                                    characterSettingData.Vec[(int)CharPosType.Left][1],
                                    characterSettingData.Vec[(int)CharPosType.Left][2]));

        charPosList.Add(new Vector3(characterSettingData.Vec[(int)CharPosType.Center][0],
                                    characterSettingData.Vec[(int)CharPosType.Center][1],
                                    characterSettingData.Vec[(int)CharPosType.Center][2]));

        charPosList.Add(new Vector3(characterSettingData.Vec[(int)CharPosType.Right][0],
                                    characterSettingData.Vec[(int)CharPosType.Right][1],
                                    characterSettingData.Vec[(int)CharPosType.Right][2]));


        // CharacterSetting Scale.
        characterSettingData = characterSettingTable.TryGet((int)CharDataType.Scale);

        // CharacterSetting Scale Data.
        charScaleList.Add(new Vector3(characterSettingData.Vec[(int)CharScaleType.Down][0],
                                      characterSettingData.Vec[(int)CharScaleType.Down][1],
                                      characterSettingData.Vec[(int)CharScaleType.Down][2]));

        charScaleList.Add(new Vector3(characterSettingData.Vec[(int)CharScaleType.Normal][0],
                                      characterSettingData.Vec[(int)CharScaleType.Normal][1],
                                      characterSettingData.Vec[(int)CharScaleType.Normal][2]));

        charScaleList.Add(new Vector3(characterSettingData.Vec[(int)CharScaleType.Up][0],
                                      characterSettingData.Vec[(int)CharScaleType.Up][1],
                                      characterSettingData.Vec[(int)CharScaleType.Up][2]));
    }

    #endregion

    #region [ Status ]

    void SetStatus()
    {
        UIStatus status;

        for (int index = 0; index < Enum.GetValues(typeof(InfoStatusType)).Length; index++)
        {
            status = Instantiate(statusPrefab_Info, statusParent_Info).GetComponent<UIStatus>();
            status.SetStatus();
            infoStatusDic.Add((InfoStatusType)index, status);
        }

        for (int index = 0; index < Enum.GetValues(typeof(CharStatusType)).Length; index++)
        {
            status = Instantiate(statusPrefab_Char, statusParent_Char).GetComponent<UIStatus>();
            status.SetStatus();
            charStatusDic.Add((CharStatusType)index, status);
        }

        Debug.Log($"CreateStatus");
    }

    public void UpdateStatus(InfoStatusType type, float value)
    {
        infoStatusDic[type].UpdateStatus(type, value);
    }

    public void UpdateStatus(CharStatusType type, float value)
    {
        charStatusDic[type].UpdateStatus(type, value);
    }

    #endregion

    #region [ Phone ]

    void OpenPhone()
    {
        phoneParent.SetActive(true);
    }

    #endregion

    #region [ Common ]

    void OnClickEmpty()
    {
        if (UIManager.Instance.GetActiveWindowUI(WindowUIType.SettingUI))
            UIManager.Instance.ActiveWindowUI(WindowUIType.SettingUI, false);
        else if (phoneParent.gameObject.activeSelf)
            ReverseActivePlayUI(PlayUIType.Phone);
        else if (statusParent.gameObject.activeSelf)
            ReverseActivePlayUI(PlayUIType.Status);
        else if (!menuParent.gameObject.activeSelf)
            ReverseActivePlayUI(PlayUIType.Menu);
    }

    public void ActivePlayUI(GameObject playUI)
    {
        playUI.SetActive(!playUI.activeSelf);
    }

    Tween fadeTween;

    public async void ReverseActivePlayUI(PlayUIType playUIType)
    {
        if (fadeTween != null && fadeTween.IsPlaying())
        {
            Debug.Log($"tween.IsComplete() : {fadeTween.IsPlaying()}");
            return;
        }

        if (parentDic[playUIType].activeSelf)
            ActivePlayUI(playUIType, false);
        else
            ActivePlayUI(playUIType, true);
    }
    public async void ActivePlayUI(PlayUIType playUIType, bool fade)
    {
        if (!fade)
        {
            canvasGroupDic[playUIType].alpha = 1;
            fadeTween = canvasGroupDic[playUIType].DOFade(0, 0.5f).SetEase(Ease.Linear);
            await UniTask.WaitUntil(() => !fadeTween.IsPlaying());
            parentDic[playUIType].SetActive(false);
        }
        else
        {
            parentDic[playUIType].SetActive(true);
            canvasGroupDic[playUIType].alpha = 0;
            fadeTween = canvasGroupDic[playUIType].DOFade(1, 0.5f).SetEase(Ease.Linear);
            await UniTask.WaitUntil(() => !fadeTween.IsPlaying());
        }
    }

    public void ActiveAllUI(bool isOn)
    {
        for (int index = 0; index < Enum.GetValues(typeof(PlayUIType)).Length; index++)
            ActivePlayUI((PlayUIType)index, isOn);
    }



    #endregion


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            PlayStart();


        if (Input.GetKeyDown(KeyCode.P))
            OpenPhone();
    }
}