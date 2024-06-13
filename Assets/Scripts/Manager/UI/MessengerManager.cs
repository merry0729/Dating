using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessengerManager : Singleton<MessengerManager>
{
    [Header("[ Messenger ]")]
    public Transform messengerParent;
    public ScrollRect messengerScrollRect;
    public Transform messengerContent;

    public GameObject messengerSenderPrefab;

    [Header("[ Message ]")]
    SerializableDictionary<PoolingObject, UIMessage> messageDic = new SerializableDictionary<PoolingObject, UIMessage>();
    SerializableDictionary<PoolType, Queue<UIMessage>> messageQueueDic = new SerializableDictionary<PoolType, Queue<UIMessage>>();

    public ScrollRect messageScrollRect;
    public RectTransform messageScrollRectTransform;
    public Transform messageContent;

    public GameObject messagePrefab_Sender;
    public GameObject messagePrefab_Mine;

    public Vector2 messagePosOffset = new Vector2(50, 50);

    #region [ Messenger Sender ]

    private void Awake()
    {
        PhoneManager.Instance.backAction += OnBack;

        messageQueueDic.Add(PoolType.Message_Mine, new Queue<UIMessage>());
        messageQueueDic.Add(PoolType.Message_Sender, new Queue<UIMessage>());
    }

    private void OnDestroy()
    {
        PhoneManager.Instance.backAction -= OnBack;
    }

    public void SetMessengerUI()
    {
        messengerParent = PhoneManager.Instance.messengerParent;
        messengerScrollRect = messengerParent.Find("Messenger_Scroll").GetComponent<ScrollRect>();
        messengerContent = messengerScrollRect.transform.Find("Viewport").Find("Content");

        messageScrollRect = messengerParent.Find("Message_Scroll").GetComponent<ScrollRect>();
        messageScrollRectTransform = messageScrollRect.GetComponent<RectTransform>();
        messageContent = messageScrollRect.transform.Find("Viewport").Find("Content");


        LoadMessenger();
    }

    void LoadMessenger()
    {
        UIMessageSender uiMessageSender = null;

        for (int index = 0; index < ConversationManager.Instance.charTypeDic.Count; index++)
        {
            uiMessageSender = Instantiate(messengerSenderPrefab, messengerContent).GetComponent<UIMessageSender>();
            uiMessageSender.SetMessageSender(index);
        }
    }

    #endregion

    #region [ Message ]

    public void ShowMessage()
    {
        PhoneManager.Instance.AddAppStack(messageScrollRect.gameObject, messengerScrollRect.gameObject);

    }
    
    void OnBack(GameObject closeObject)
    {
        Debug.Log($"OnBack : {closeObject.name}");

        if (closeObject == messageScrollRect.gameObject)
        {
            Debug.Log($"OnBack : same");
            ObjectPoolManager.Instance.EnableAllPoolObject(PoolType.Message_Sender);
            ObjectPoolManager.Instance.EnableAllPoolObject(PoolType.Message_Mine);
            messageQueueDic[PoolType.Message_Sender].Clear();
            messageQueueDic[PoolType.Message_Mine].Clear();
        }
    }

    UIMessage PoolMessage(PoolType poolType)
    {
        PoolingObject poolingObject = ObjectPoolManager.Instance.PoolObject(poolType);
        UIMessage uiMessage = null;

        if (!messageDic.ContainsKey(poolingObject))
        {
            uiMessage = poolingObject.GetComponent<UIMessage>();
            messageDic.Add(poolingObject, uiMessage);
        }
        else
        {
            uiMessage = messageDic[poolingObject];
        }

        messageQueueDic[poolType].Enqueue(uiMessage);

        SetMessagePos(poolType, uiMessage);

        

        return uiMessage;
    }

    void SetMessagePos(PoolType poolType, UIMessage message)
    {
        message.transform.SetParent(messageContent);

        int messageCount =
            messageQueueDic[PoolType.Message_Mine].Count + messageQueueDic[PoolType.Message_Sender].Count;

        Debug.Log($"messageQueueDic[PoolType.Message_Mine].Count : {messageQueueDic[PoolType.Message_Mine].Count} + messageQueueDic[PoolType.Message_Sender].Count : {messageQueueDic[PoolType.Message_Sender].Count} = {messageCount}");

        message.SetPos(poolType, messagePosOffset, messageCount);

        //messageScrollRect.verticalNormalizedPosition = 0;
    }

    public void UpdateScrollRectPos()
    {
        messageScrollRect.verticalNormalizedPosition = 0;
    }

    #endregion



    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log($"Sender");

                PoolMessage(PoolType.Message_Sender);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log($"Mine");

                PoolMessage(PoolType.Message_Mine);
            }
        }
    }
}
