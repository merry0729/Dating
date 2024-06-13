using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessage : MonoBehaviour
{
    PoolType poolType;
    RectTransform rectTransform;

    HorizontalLayoutGroup hor;

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

    private void Update()
    {

    }
}
