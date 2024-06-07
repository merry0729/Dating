using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIOptions : MonoBehaviour
{
    OptionType optionType;
    public TextMeshProUGUI optionText;

    private void Awake()
    {
        optionText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void ShowOptions(OptionsData optionsData, int index)
    {
        optionType = (OptionType)optionsData.OptionType[index];
        optionText.text = optionsData.Options[index];
    }
}
