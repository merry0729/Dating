using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : Singleton<ConversationManager>
{
    Vector3 topPos;
    Vector3 middlePos;
    Vector3 bottomPos;

    SerializableDictionary<PoolingObject, UIConversation> conversationDic = new SerializableDictionary<PoolingObject, UIConversation>();
    public SerializableDictionary<CharType, string> charTypeDic = new SerializableDictionary<CharType, string>();

    public Queue<UIConversation> conversationQueue = new Queue<UIConversation>();
    public UIConversation holdConversation;
    UIConversation currentUICon;

    ConversationTable conTable;
    public ConversationData currentConData;

    ConversationSettingTable conSetTable;
    ConversationSettingData conSetData;

    int currentId = 0;

    #region [ Options Declare ]

    [Header(" [ Options ] ")]
    public OptionsData optionsData;
    public OptionsTable optionsTable;

    public GameObject uiOptionPrefab;
    public List<UIOptions> optionsList = new List<UIOptions>();

    int currentOptionIndex = 1;

    #endregion


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        optionsTable = OptionsData.Table;

        for (int index = 0; index < Enum.GetValues(typeof(CharType)).Length; index++)
            charTypeDic.Add((CharType)index, ((CharType)index).ToString());
    }

    private void Start()
    {
        ConversationDataSetting();
    }

    public void StartConversation()
    {
        ShowConversation(currentId);
    }

    public void ShowConversation(int index)
    {
        currentConData = conTable.TryGet(index);
        PoolConversation().UpdateConversation(currentConData);
        PlayManager.Instance.SetChracter(currentConData.Who, currentConData.CharPos, currentConData.CharScale);
    }

    public Vector3 GetPos(ConPosType posType)
    {
        switch (posType)
        {
            case ConPosType.Top:
                return topPos;
            case ConPosType.Middle:
                return middlePos;
            case ConPosType.Bottom:
                return bottomPos;
            default:
                return middlePos;
        }
    }

    UIConversation PoolConversation()
    {
        PoolingObject poolingObject = ObjectPoolManager.Instance.PoolObject(PoolType.Conversation);
        UIConversation uiConversation = null;

        if (!conversationDic.ContainsKey(poolingObject))
        {
            uiConversation = poolingObject.GetComponent<UIConversation>();
            conversationDic.Add(poolingObject, uiConversation);
        }
        else
        {
            uiConversation = conversationDic[poolingObject];
        }

        conversationQueue.Enqueue(uiConversation);

        if (currentConData.Hold)
        {
            holdConversation = uiConversation;
            holdConversation.Hold(true);
        }

        if (currentConData.RemoveHold)
        {
            holdConversation.EnablePoolObject();
            holdConversation.Hold(false);
        }

        return uiConversation;
    }

    void EnableConversation()
    {
        if (conversationQueue.Count > 0)
            conversationQueue.Peek().EndConversation();
    }

    public void DeqConversation()
    {
        Debug.Log($"Dequeue: {conversationQueue.Peek().conText.text} / {currentConData.Next}");
        conversationQueue.Dequeue();
        ShowConversation(currentConData.Next);
    }

    void ConversationDataSetting()
    {
        // Conversation Table.
        conTable = ConversationData.Table;
        //// Conversation Data.
        //currentConData = conTable.TryGet(currentId);

        // Conversation Setting Data.
        conSetTable = ConversationSettingData.Table;

        // Conversation Setting Data Pos.
        conSetData = conSetTable.TryGet((int)ConSetType.Pos);


        topPos = new Vector3(conSetData.ConPos[(int)ConPosType.Top][0],
                             conSetData.ConPos[(int)ConPosType.Top][1],
                             conSetData.ConPos[(int)ConPosType.Top][2]);

        middlePos = new Vector3(conSetData.ConPos[(int)ConPosType.Middle][0],
                             conSetData.ConPos[(int)ConPosType.Middle][1],
                             conSetData.ConPos[(int)ConPosType.Middle][2]);

        bottomPos = new Vector3(conSetData.ConPos[(int)ConPosType.Bottom][0],
                             conSetData.ConPos[(int)ConPosType.Bottom][1],
                             conSetData.ConPos[(int)ConPosType.Bottom][2]);
    }


    #region [ Option ]

    public void SetOptions(int optionIndex)
    {
        Debug.Log($"optionIndex : {optionIndex}");

        ActiveOption(true);

        optionsData = optionsTable.TryGet(optionIndex);
        int optionsCount = optionsData.Options.Length;

        if (optionsList.Count > 0)
        {
            for (int index = 0; index < optionsList.Count; index++)
            {
                if (optionsList[index].gameObject.activeSelf)
                    optionsList[index].gameObject.SetActive(false);
            }
        }

        if (optionsList.Count < optionsCount)
        {
            int createCount = optionsCount - optionsList.Count;

            for (int index = 0; index < createCount; index++)
            {
                optionsList.Add(Instantiate(uiOptionPrefab, PlayManager.Instance.optionBackground).GetComponent<UIOptions>());
            }
        }

        for (int index = 0; index < optionsCount; index++)
        {
            optionsList[index].gameObject.SetActive(true);
            optionsList[index].SetOptionData(optionsData, optionsList.Count, index);
        }
    }

    public void ActiveOption(bool isOn)
    {
        PlayManager.Instance.optionParent.gameObject.SetActive(isOn);
    }

    #endregion


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            EnableConversation();


        if (Input.GetKeyDown(KeyCode.O))
            SetOptions(currentOptionIndex++);
    }
}
