using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    public static ConversationManager Instance;

    Vector3 topVec = new Vector3(0f, 440f, 0f);
    Vector3 middleVec = Vector3.zero;
    Vector3 bottomVec = new Vector3(0f, -440f, 0f);

    Dictionary<PoolingObject, UIConversation> conversationDic = new Dictionary<PoolingObject, UIConversation>();

    Queue<UIConversation> conversationQueue = new Queue<UIConversation>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public Vector3 GetVec(TransformType transformType)
    {
        switch (transformType)
        {
            case TransformType.Top:
                return transform.localPosition = topVec;
            case TransformType.Middle:
                return transform.localPosition = middleVec;
            case TransformType.Bottom:
                return transform.localPosition = bottomVec;
            default:
                return transform.localPosition = middleVec;
        }
    }

    void PoolConversation()
    {
        int ran = Random.Range(0, 3);

        TransformType transformType = (TransformType)ran;

        PoolingObject poolingObject = ObjectPoolManager.Instance.PoolObject(PoolType.Conversation);
        UIConversation uiConversation = null;

        if (!conversationDic.ContainsKey(poolingObject))
        {
            uiConversation = poolingObject.GetComponent<UIConversation>();
            uiConversation.SetTransformType(transformType);
            conversationDic.Add(poolingObject, uiConversation);
        }
        else
        {
            uiConversation = conversationDic[poolingObject];
            uiConversation.SetTransformType(transformType);
        }

        conversationQueue.Enqueue(uiConversation);
    }

    void EnableConversation()
    {
        if (conversationQueue.Count > 0)
            conversationQueue.Dequeue().EndConversation();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PoolConversation();

        if (Input.GetMouseButtonDown(0))
            EnableConversation();
    }
}
