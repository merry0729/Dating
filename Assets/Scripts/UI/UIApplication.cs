using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIApplication : MonoBehaviour
{
    public Image appImage;
    public TextMeshProUGUI appText;

    ApplicationType appType;

    UIButton appBtn;

    private void Awake()
    {
        appBtn = GetComponentInChildren<UIButton>();
        appBtn.OnClick += OnClickApp;
    }

    private void OnDestroy()
    {
        if (appBtn != null)
            appBtn.OnClick -= OnClickApp;
    }

    public void SetApplication(int index)
    {
        appType = (ApplicationType)index;
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
        appText.text = appType.ToString();
    }

    void OnClickApp()
    {
        switch (appType)
        {
            case ApplicationType.Messenger:
                PhoneManager.Instance.SetPhoneState(PhoneState.InApp, appType);
                break;
        }
    }

    void Update()
    {
        
    }
}
