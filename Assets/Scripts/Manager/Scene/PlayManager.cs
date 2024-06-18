using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

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
    public Transform rightMenuParent;

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
    public OptionsData optionsData;
    public OptionsTable optionsTable;
    

    public GameObject uiOptionPrefab;
    private Transform optionParent;
    private Transform optionBackground;
    public List<UIOptions> optionsList = new List<UIOptions>();

    int currentOptionIndex = 0;

    #endregion

    #region

    [Header(" [ Phone ] ")]
    public GameObject phoneParent;

    #endregion


    private void Awake()
    {
        optionsTable = OptionsData.Table;

        SetPlayObject();
        SetCharacterSettingData();
    }

    private void Start()
    {
        
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
        rightMenuParent = UIManager.Instance.GetCurrentSceneUI().transform.Find("Menu").Find("RightMenu");
        SetMenus();

        optionParent = UIManager.Instance.GetCurrentSceneUI().transform.Find("Options");
        optionBackground = optionParent.Find("Option_Background").transform;

        phoneParent = UIManager.Instance.GetCurrentSceneUI().transform.Find("Phone").gameObject;
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

    #region [ Character ]

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

    #region [ Option ]

    void SetOptions(int optionIndex)
    {
        Debug.Log($"optionIndex : {optionIndex}");

        optionParent.gameObject.SetActive(true);

        optionsData = optionsTable.TryGet(optionIndex);
        int optionsCount = optionsData.Options.Length;

        if(optionsList.Count > 0)
        {
            for(int index = 0; index < optionsList.Count; index++)
            {
                if (optionsList[index].gameObject.activeSelf)
                    optionsList[index].gameObject.SetActive(false);
            }
        }

        if (optionsList.Count < optionsCount)
        {
            int createCount = optionsCount - optionsList.Count;

            for(int index = 0; index < createCount; index++)
            {
                optionsList.Add(Instantiate(uiOptionPrefab, optionBackground).GetComponent<UIOptions>());
            }
        }

        for (int index = 0; index < optionsCount; index++)
        {
            optionsList[index].gameObject.SetActive(true);
            optionsList[index].SetOptionData(optionsData, optionsList.Count, index);
        }
    }

    #endregion

    #region [ Phone ]

    void OpenPhone()
    {
        phoneParent.SetActive(true);
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            PlayStart();

        if (Input.GetKeyDown(KeyCode.O))
            SetOptions(currentOptionIndex++);

        if (Input.GetKeyDown(KeyCode.P))
            OpenPhone();
    }
}
