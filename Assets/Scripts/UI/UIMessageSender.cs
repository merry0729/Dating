using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.U2D.Animation;

public class UIMessageSender : MonoBehaviour
{
    public CharType currentMessageSender;

    UIButton senderBtn;

    public Image senderIllust;
    public TextMeshProUGUI senderText;

    string directoryPath = "Illustration/Character/";

    CharacterData characterData;

    private void Awake()
    {
        senderBtn = GetComponent<UIButton>();
        senderBtn.OnClick += OnClickSender;
    }

    private void OnDestroy()
    {
        if (senderBtn != null)
            senderBtn.OnClick -= OnClickSender;
    }

    public void SetMessageSender(int index)
    {
        currentMessageSender = (CharType)index;
        characterData = CharacterData.Table.TryGet(index);

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

        Sprite characterSprite = Resources.Load<Sprite>(directoryPath + characterData.ImgFileName);
        senderIllust.sprite = characterSprite;
        senderText.text = ConversationManager.Instance.charTypeDic[currentMessageSender];
    }

    void OnClickSender()
    {
        MessengerManager.Instance.currentCharType = currentMessageSender;
        MessengerManager.Instance.ShowMessage(currentMessageSender);
    }
}
