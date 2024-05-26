using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConversation : MonoBehaviour
{
    TransformType transformType = TransformType.Middle;
    PoolingObject poolingObject;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTransformType(TransformType type)
    {
        transformType = type;
        SetTransform();
    }

    void SetTransform()
    {
        rectTransform.anchoredPosition = ConversationManager.Instance.GetVec(transformType);
        transform.localScale = Vector3.one;
    }

    public void EndConversation()
    {
        if(poolingObject == null)
            poolingObject = GetComponent<PoolingObject>();

        ObjectPoolManager.Instance.EnablePoolObject(PoolType.Conversation, poolingObject);
    }
}
