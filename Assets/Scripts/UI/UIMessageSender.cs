using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMessageSender : MonoBehaviour
{
    public CharType currentMessageSender;

    UIButton senderBtn;

    public Image senderIllust;
    public TextMeshProUGUI senderText;

    private void Awake()
    {
        senderBtn = GetComponent<UIButton>();
        senderBtn.OnClick += OnClickSender;
    }

    private void OnDestroy()
    {
        senderBtn.OnClick -= OnClickSender;
    }

    public void SetMessageSender(int index)
    {
        currentMessageSender = (CharType)index;

        SetMessageSenderUI();
        UpdateMessageSenderUI();
    }

    void SetMessageSenderUI()
    {
        senderIllust = transform.Find("Character_Outline").Find("Character_Mask").Find("Characker_Illust").GetComponent<Image>();
        senderText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        Debug.Log($"SetMessageSenderUI");
    }

    void UpdateMessageSenderUI()
    {
        Debug.Log($"UpdateMessageSenderUI");
        //senderIllust.sprite = 
        senderText.text = ConversationManager.Instance.charTypeDic[currentMessageSender];
    }

    void OnClickSender()
    {
        MessengerManager.Instance.ShowMessage(currentMessageSender);
    }
}
