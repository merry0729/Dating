using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConversation : MonoBehaviour
{
    ConPosType conPosType = ConPosType.Middle;
    ConScaleType conScaleType = ConScaleType.Normal;

    public Image conImage;
    public TextMeshProUGUI conText;

    PoolingObject poolingObject;
    RectTransform rectTransform;
    ConversationData currentConData;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTransformType(ConPosType posType, ConScaleType scaleType)
    {
        conPosType = posType;
        conScaleType = scaleType;
        SetTransform();
    }

    void SetTransform()
    {
        rectTransform.anchoredPosition = ConversationManager.Instance.GetPos(conPosType);
        transform.localScale = ConversationManager.Instance.GetScale(conScaleType);
    }

    public void EndConversation()
    {
        if(poolingObject == null)
            poolingObject = GetComponent<PoolingObject>();

        ObjectPoolManager.Instance.EnablePoolObject(PoolType.Conversation, poolingObject);
        ConversationManager.Instance.ShowConversation(currentConData.Next);
    }

    public void UpdateConversation(ConversationData conversationData)
    {
        currentConData = conversationData;
        conText.text = $"{currentConData.Text}";

        SetTransformType((ConPosType)conversationData.Pos, (ConScaleType)conversationData.Scale);
    }
}
