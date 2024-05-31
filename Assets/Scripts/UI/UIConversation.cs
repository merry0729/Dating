using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConversation : MonoBehaviour
{
    ConPosType conPosType = ConPosType.Middle;

    public Image conImage;
    public TextMeshProUGUI conName;
    public TextMeshProUGUI conText;
    StringBuilder sbText = new StringBuilder();

    PoolingObject poolingObject;
    RectTransform rectTransform;
    ConversationData currentConData;

    bool isHold;

    bool isTyping;
    int textLength;
    int currentIndex;
    float elapsedTime = 0f;
    float textTerm = 0.05f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTransformType(ConPosType posType)
    {
        conPosType = posType;
        SetTransform();
    }

    void SetTransform()
    {
        rectTransform.anchoredPosition = ConversationManager.Instance.GetPos(conPosType);
    }

    public void Hold(bool isOn)
    {
        isHold = isOn;
    }

    public void EndConversation()
    {
        if (isTyping)
        {
            TypingEnd();
            return;
        }

        if (poolingObject == null)
            poolingObject = GetComponent<PoolingObject>();

        ConversationManager.Instance.DeqConversation();

        if (!isHold)
            EnablePoolObject();
    }

    public void EnablePoolObject()
    {
        ObjectPoolManager.Instance.EnablePoolObject(PoolType.Conversation, poolingObject);
    }

    public void UpdateConversation(ConversationData conversationData)
    {
        conName.text = ConversationManager.Instance.charTypeDic[(CharType)conversationData.Who];
        conText.text = "";
        sbText.Clear();

        currentConData = conversationData;

        textLength = currentConData.Text.Length;

        Typing();

        SetTransformType((ConPosType)conversationData.ConPos);
    }

    void Typing()
    {
        isTyping = true;
        
        sbText.Append(currentConData.Text[currentIndex]);
        conText.text = sbText.ToString();
        currentIndex++;
        elapsedTime = 0f;

        if (currentIndex >= textLength)
            TypingEnd();
    }

    void TypingEnd()
    {
        isTyping = false;
        conText.text = currentConData.Text;
        currentIndex = 0;
    }

    private void Update()
    {
        if (isTyping)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > textTerm)
                Typing();
        }
    }
}
