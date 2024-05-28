using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : Singleton<ConversationManager>
{
    Vector3 topPos = new Vector3(0f, 440f, 0f);
    Vector3 middlePos = Vector3.zero;
    Vector3 bottomPos = new Vector3(0f, -440f, 0f);

    Vector3 downScale = new Vector3(0.5f, 0.5f, 1f);
    Vector3 normalScale = Vector3.one;
    Vector3 upScale = new Vector3(2f, 2f, 1f);

    Dictionary<PoolingObject, UIConversation> conversationDic = new Dictionary<PoolingObject, UIConversation>();

    Queue<UIConversation> conversationQueue = new Queue<UIConversation>();
    UIConversation currentUICon;

    ConversationTable conTable;
    ConversationData currentConData;

    int currentId = 1;

    private void Awake()
    {

    }

    private void Start()
    {
        conTable = ConversationData.Table;

        Debug.Log($"conTable Start : {conTable}");
        currentConData = conTable.TryGet(currentId);
    }

    public void StartConversation()
    {
        Debug.Log($"conTable : {conTable}");
        currentConData = conTable.TryGet(currentId);
        PoolConversation().UpdateConversation(currentConData);
    }

    public void ShowConversation(int index)
    {
        currentConData = conTable.TryGet(index);
        PoolConversation().UpdateConversation(currentConData);
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
                return normalScale;
        }
    }

    public Vector3 GetScale(ConScaleType scaleType)
    {
        switch (scaleType)
        {
            case ConScaleType.Down:
                return downScale;
            case ConScaleType.Normal:
                return normalScale;
            case ConScaleType.Up:
                return upScale;
            default:
                return normalScale;
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

    ConversationTable table;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PoolConversation();

        if (Input.GetMouseButtonDown(0))
            EnableConversation();

        if (Input.GetMouseButtonDown(1))
        {
            table = ConversationData.Table;


            Debug.Log($"conver id : {table}");
            Debug.Log($"conver id : {table.Count}");

            Debug.Log($"conver id : {table.TryGet(1).Id}");
            Debug.Log($"conver id : {table.TryGet(1).Text}");
            Debug.Log($"conver id : {table.TryGet(2).Id}");
        }
    }
}
