using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UIMessage : MonoBehaviour
{
    PoolType poolType;
    RectTransform rectTransform;

    HorizontalLayoutGroup hor;

    TextMeshProUGUI messageText;

    int maxLength = 30;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        hor = GetComponent<HorizontalLayoutGroup>();
    }

    public void SetPos(PoolType type, Vector2 offset, int messageCount)
    {
        rectTransform.anchoredPosition = Vector2.one;

        Canvas.ForceUpdateCanvases();

        MessengerManager.Instance.UpdateScrollRectPos();

        //poolType = type;

        //float x = offset.x; 
        //float y = offset.y + (rectTransform.rect.height * (messageCount - 1)) + (20 * (messageCount - 1));

        //if(poolType == PoolType.Message_Mine)
        //{
        //    //rectTransform.anchoredPosition = new Vector2(-x, -y);
        //    rectTransform.anchoredPosition = new Vector2(0, 0);
        //}
        //else if (poolType == PoolType.Message_Sender)
        //{
        //    //rectTransform.anchoredPosition = new Vector2(x, -y);
        //    rectTransform.anchoredPosition = new Vector2(0, 0);
        //}
    }

    public void SetMessage()
    {
        messageText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(string text)
    {
        StringBuilder sb = new StringBuilder();
        string[] words = text.Split(' ');

        int currentLineLength = 0;

        foreach (string word in words)
        {
            if (currentLineLength + word.Length + 1 > maxLength)
            {
                sb.AppendLine();
                currentLineLength = 0;
            }

            if (currentLineLength > 0)
            {
                sb.Append(' ');
                currentLineLength++;
            }

            sb.Append(word);
            currentLineLength += word.Length;
        }

        messageText.text = sb.ToString();
    }

    private void Update()
    {

    }
}
