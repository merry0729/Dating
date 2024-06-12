using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessage : MonoBehaviour
{
    PoolType poolType;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetPos(PoolType type, Vector2 offset, int messageCount)
    {
        poolType = type;

        float x = offset.x + 0; 
        float y = offset.y + (rectTransform.sizeDelta.y * messageCount - 1) + (20 * messageCount - 1);

        if(poolType == PoolType.Message_Mine)
        {
            rectTransform.anchoredPosition = new Vector2(x, y);
        }
        else if (poolType == PoolType.Message_Sender)
        {
            rectTransform.anchoredPosition = new Vector2(x, y);
        }
    }
}
