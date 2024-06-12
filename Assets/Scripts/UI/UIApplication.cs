using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIApplication : MonoBehaviour
{
    public Image appImage;
    public TextMeshProUGUI appText;

    PhoneState phoneState;

    UIButton appBtn;

    private void Awake()
    {
        appBtn = GetComponentInChildren<UIButton>();
        appBtn.OnClick += OnClickApp;
    }

    private void OnDestroy()
    {
        appBtn.OnClick -= OnClickApp;
    }

    public void SetApplication(int index)
    {
        phoneState = (PhoneState)index;
        SetApplicationUI();
        UpdateApplicationUI();
    }

    void SetApplicationUI()
    {
        appImage = transform.Find("Application_Image").GetComponent<Image>();
        appText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    void UpdateApplicationUI()
    {
        //appImage.sprite = 
        appText.text = phoneState.ToString();
    }

    void OnClickApp()
    {
        PhoneManager.Instance.ShowCloseBtn();

        switch(phoneState)
        {
            case PhoneState.Messenger:
                PhoneManager.Instance.ShowMessenger();
                break;
            //case ApplicationType.Setting:
            //    UIManager.Instance.ActiveWindowUI(WindowUIType.SettingUI, true);
            //    break;
        }

        PhoneManager.Instance.SetPhoneState(PhoneState.Messenger);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
