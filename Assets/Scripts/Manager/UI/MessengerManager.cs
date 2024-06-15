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

    public CharType currentCharType;

    [Header("[ Message ]")]
    SerializableDictionary<PoolingObject, UIMessage> messageDic = new SerializableDictionary<PoolingObject, UIMessage>();
    SerializableDictionary<PoolType, Queue<UIMessage>> messageQueueDic = new SerializableDictionary<PoolType, Queue<UIMessage>>();
    SerializableDictionary<CharType, HeroinMessage> messageDataDic = new SerializableDictionary<CharType, HeroinMessage>();

    public ScrollRect messageScrollRect;
    public RectTransform messageScrollRectTransform;
    public Transform messageContent;

    public GameObject messagePrefab_Sender;
    public GameObject messagePrefab_Mine;

    public Vector2 messagePosOffset = new Vector2(50, 50);

    public MessageTable messageTable;
    public MessageData messageData;

    Coroutine messageCor;

    #region [ Messenger Sender ]

    private void Awake()
    {
        messageTable = MessageData.Table;

        messageDataDic.Add(CharType.Heroin_1, new HeroinMessage());
        messageDataDic.Add(CharType.Heroin_2, new HeroinMessage());
        messageDataDic.Add(CharType.Heroin_3, new HeroinMessage());

        foreach(var data in messageTable.Values)
        {
            messageDataDic[CharType.Heroin_1].speaker.Add((CharType)data.Speaker_Heroin1);
            messageDataDic[CharType.Heroin_1].messageContinue.Add(data.Continue_Heroin1);
            messageDataDic[CharType.Heroin_1].messageText.Add(data.Message_Heroin1);

            messageDataDic[CharType.Heroin_2].speaker.Add((CharType)data.Speaker_Heroin2);
            messageDataDic[CharType.Heroin_2].messageContinue.Add(data.Continue_Heroin2);
            messageDataDic[CharType.Heroin_2].messageText.Add(data.Message_Heroin2);

            messageDataDic[CharType.Heroin_3].speaker.Add((CharType)data.Speaker_Heroin3);
            messageDataDic[CharType.Heroin_3].messageContinue.Add(data.Continue_Heroin3);
            messageDataDic[CharType.Heroin_3].messageText.Add(data.Message_Heroin3);
        }

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

    public void ShowMessage(CharType speakerType, bool isEnter = true)
    {
        PhoneManager.Instance.AddAppStack(messageScrollRect.gameObject, messengerScrollRect.gameObject);

        messageCor = StartCoroutine(ShowingMessage(speakerType, isEnter));
    }

    IEnumerator ShowingMessage(CharType speakerType, bool isEnter = true)
    {
        UIMessage uiMessage;

        int messageIndex = isEnter ? 0 : messageDataDic[speakerType].lastIndex + 1;

        for (int index = messageIndex; index < messageDataDic[speakerType].messageText.Count; index++)
        {
            if (messageDataDic[speakerType].speaker[index] == CharType.Main)
            {
                if (MessageControl(PoolType.Message_Mine, index) == false)
                    break;
            }
            else
            {
                if (MessageControl(PoolType.Message_Sender, index) == false)
                    break;
            }

            if (index >= messageDataDic[speakerType].lastIndex)
                yield return new WaitForSeconds(0.2f);

        }

        bool MessageControl(PoolType poolType, int index)
        {
            uiMessage = PoolMessage(poolType);
            uiMessage.SetMessage();
            uiMessage.UpdateText(messageDataDic[speakerType].messageText[index]);

            if (messageDataDic[speakerType].messageContinue[index] == false &&
                messageDataDic[speakerType].lastIndex < index)
            {
                messageDataDic[speakerType].lastIndex = index;
                Debug.Log($"{speakerType} Last Index : {messageDataDic[speakerType].lastIndex}");
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    void OnBack(GameObject closeObject)
    {
        Debug.Log($"OnBack : {closeObject.name}");

        if (closeObject == messageScrollRect.gameObject)
        {
            if (messageCor != null)
                StopCoroutine(messageCor);

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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowMessage(currentCharType, false);
            }
        }
    }
}

public class HeroinMessage
{
    public List<CharType> speaker = new List<CharType>();
    public List<bool> messageContinue = new List<bool>();
    public List<string> messageText = new List<string>();
    public int lastIndex = 0;
}