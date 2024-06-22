using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIOptions : MonoBehaviour
{
    RectTransform rect;

    OptionType optionType;
    public UIButton optionBtn;
    public TextMeshProUGUI optionText;

    OptionsData optionsData;
    int count;
    int currentIndex;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        optionBtn = GetComponent<UIButton>();
        optionText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        optionBtn.OnClick += OnClickOption;
    }

    private void OnDestroy()
    {
        optionBtn.OnClick -= OnClickOption;
    }

    public void SetOptionData(OptionsData argOptionsData, int fullCount, int index)
    {
        optionsData = argOptionsData;
        count = fullCount;
        currentIndex = index;

        UpdateOptions();
    }

    void UpdateOptions()
    {
        optionType = (OptionType)optionsData.OptionType[currentIndex];
        optionText.text = optionsData.Options[currentIndex];
        SetPos();
    }

    public void SetPos()
    {
        rect.anchoredPosition = new Vector2(
            0,
            -(Screen.height / (count + 1)) * (currentIndex+1));
    }

    void OnClickOption()
    {
        ConversationManager.Instance.ActiveOption(false);
    }
}
