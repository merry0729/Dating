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
    public Transform messageContent;

    public GameObject messagePrefab_Sender;
    public GameObject messagePrefab_Mine;

    public Vector2 messagePosOffset = new Vector2(50, 50);

    #region [ Messenger Sender ]

    private void Awake()
    {
        messageQueueDic.Add(PoolType.Message_Mine, new Queue<UIMessage>());
        messageQueueDic.Add(PoolType.Message_Sender, new Queue<UIMessage>());
    }

    public void SetMessengerUI()
    {
        messengerParent = PhoneManager.Instance.messengerParent;
        messengerScrollRect = messengerParent.Find("Messenger_Scroll").GetComponent<ScrollRect>();
        messengerContent = messengerScrollRect.transform.Find("Viewport").Find("Content");

        messageScrollRect = messengerParent.Find("Message_Scroll").GetComponent<ScrollRect>();
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
        messengerScrollRect.gameObject.SetActive(false);
        messageScrollRect.gameObject.SetActive(true);
    }

    public void CloseMessage()
    {
        messengerScrollRect.gameObject.SetActive(true);
        messageScrollRect.gameObject.SetActive(false);
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

        return uiMessage;
    }

    void SetMessagePos(PoolType poolType)
    {
        UIMessage uiMessage = null;
        uiMessage = messageQueueDic[poolType].Peek();

        uiMessage.transform.SetParent(messageContent);

        int messageCount =
            messageQueueDic[PoolType.Message_Mine].Count + messageQueueDic[PoolType.Message_Sender].Count;

        uiMessage.SetPos(poolType, messagePosOffset, messageCount);
    }

    #endregion

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                PoolMessage(PoolType.Message_Sender);
            }
            else if(Input.GetKeyDown(KeyCode.M))
            {
                PoolMessage(PoolType.Message_Mine);
            }
        }
    }
}
