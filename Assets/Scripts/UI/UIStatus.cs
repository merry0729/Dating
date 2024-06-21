using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatus : MonoBehaviour
{
    public TextMeshProUGUI statusNameText;
    public TextMeshProUGUI statusValueText;
    public Image statusFillImg;

    public bool isFill;

    public void SetStatus()
    {
        statusNameText = transform.Find($"Status_Frame").Find($"Status_Name").GetComponent<TextMeshProUGUI>();
        statusValueText = transform.Find($"Status_Value").Find($"Text (TMP)").GetComponent<TextMeshProUGUI>();

        if(isFill)
            statusFillImg = transform.Find($"Status_Value").Find($"Image_Back").Find($"Image_Fill").GetComponent<Image>();
    }

    public void UpdateStatus(InfoStatusType type, float value)
    {
        statusNameText.text = type.ToString();
        statusValueText.text = value.ToString();
    }

    public void UpdateStatus(CharStatusType type, float value)
    {
        statusNameText.text = type.ToString();
        statusValueText.text = value.ToString();
        statusFillImg.fillAmount = value / 100;
    }
}
