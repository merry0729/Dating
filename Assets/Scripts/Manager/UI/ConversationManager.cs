using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : Singleton<ConversationManager>
{
    Vector3 topPos;
    Vector3 middlePos;
    Vector3 bottomPos;

    Dictionary<PoolingObject, UIConversation> conversationDic = new Dictionary<PoolingObject, UIConversation>();

    Queue<UIConversation> conversationQueue = new Queue<UIConversation>();
    UIConversation currentUICon;

    ConversationTable conTable;
    ConversationData currentConData;

    ConversationSettingTable conSetTable;
    ConversationSettingData conSetData;

    int currentId = 1;

    private void Awake()
    {

    }

    private void Start()
    {
        ConversationDataSetting();
    }

    public void StartConversation()
    {
        currentConData = conTable.TryGet(currentId);
        PoolConversation().UpdateConversation(currentConData);
        PlayManager.Instance.SetChracter(currentConData.Who, currentConData.CharPos, currentConData.CharScale);
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

        return uiConversation;
    }

    void EnableConversation()
    {
        if (conversationQueue.Count > 0)
            conversationQueue.Dequeue().EndConversation();
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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PoolConversation();

        if (Input.GetMouseButtonDown(0))
            EnableConversation();
    }
}
