using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneManager : Singleton<PhoneManager>
{
    public PhoneState phoneState = PhoneState.Idle;

    #region [ Application ]

    [Header("[ Application ]")]
    public GameObject applicationPrefab;

    public Transform applicationParent;

    #endregion

    #region [ Messenger ]

    [Header("[ Messenger ]")]
    public Transform messengerParent;

    public ScrollRect messengerScrollRect_Sender;
    public Transform messengerContent_Sender;

    public GameObject messengerSenderPrefab;

    public ScrollRect messengerScrollRect_Message;
    public Transform messengerContent_Message;

    #endregion

    #region [ Common ]

    [Header("[ Common ]")]
    public Transform commonParent;

    public UIButton backBtn;

    #endregion

    void Start()
    {
        
    }

    private void OnDestroy()
    {
        backBtn.OnClick -= OnClickBack;
    }

    void SetPhoneData()
    {

    }

    public void SetPhoneUI()
    {
        applicationParent = PlayManager.Instance.phoneParent.transform.Find("Application");

        messengerParent = PlayManager.Instance.phoneParent.transform.Find("Messenger");
        messengerScrollRect_Sender = messengerParent.Find("Messenger_Scroll").GetComponent<ScrollRect>();
        messengerContent_Sender = messengerScrollRect_Sender.transform.Find("Viewport").Find("Content");

        commonParent = PlayManager.Instance.phoneParent.transform.Find("Common");
        backBtn = commonParent.Find("Back_Btn").GetComponent<UIButton>();
        backBtn.OnClick += OnClickBack;

        LoadApplicationUI();
        LoadMessenger();
    }

    public void SetPhoneState(PhoneState state)
    {
        phoneState = state;
        applicationParent.gameObject.SetActive(false);
    }

    #region [ Application]

    void LoadApplicationUI()
    {
        UIApplication uIApplication = null;

        for (int index = 0; index < Enum.GetValues(typeof(ApplicationType)).Length; index++)
        {
            uIApplication = Instantiate(applicationPrefab, applicationParent).GetComponent<UIApplication>();
            uIApplication.SetApplication(index);
        }
    }

    #endregion

    #region [ Messenger ]

    public void ShowMessenger()
    {
        messengerParent.gameObject.SetActive(true);
    }

    void CloseMessenger()
    {
        messengerParent.gameObject.SetActive(false);
    }

    void LoadMessenger()
    {
        UIMessageSender uiMessageSender = null;

        for (int index = 0; index < ConversationManager.Instance.charTypeDic.Count; index++)
        {
            uiMessageSender = Instantiate(messengerSenderPrefab, messengerContent_Sender).GetComponent<UIMessageSender>();
            uiMessageSender.SetMessageSender(index);
        }
    }

    #endregion

    #region [ Common ]

    public void ShowCloseBtn()
    {
        backBtn.gameObject.SetActive(true);
    }

    void OnClickBack()
    {
        CloseApplication(phoneState);
    }

    void CloseApplication(PhoneState state)
    {
        switch(state)
        {
            case PhoneState.Messenger:
                messengerParent.gameObject.SetActive(false);
                break;
            case PhoneState.Idle:
                return;
        }

        backBtn.gameObject.SetActive(false);
        applicationParent.gameObject.SetActive(true);

        phoneState = PhoneState.Idle;
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            ShowMessenger();
    }
}
