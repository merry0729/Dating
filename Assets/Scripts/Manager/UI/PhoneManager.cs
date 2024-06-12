using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneManager : Singleton<PhoneManager>
{
    public PhoneState phoneState = PhoneState.Application;

    public Stack<(PhoneState, GameObject)> openLayerTuStack = new Stack<(PhoneState, GameObject)>();
    public Stack<(PhoneState, GameObject)> closeLayerTuStack = new Stack<(PhoneState, GameObject)>();

    #region [ Application ]

    [Header("[ Application ]")]
    public GameObject applicationPrefab;
    public Transform applicationParent;
    GameObject currentApplicationParent;

    #endregion


    #region [ Messenger ]

    [Header("[ Message ]")]
    public Transform messengerParent;

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
        // Application
        applicationParent = PlayManager.Instance.phoneParent.transform.Find("Application");

        // Messenger
        messengerParent = PlayManager.Instance.phoneParent.transform.Find("Messenger");
        MessengerManager.Instance.SetMessengerUI();

        // Common
        commonParent = PlayManager.Instance.phoneParent.transform.Find("Common");
        backBtn = commonParent.Find("Back_Btn").GetComponent<UIButton>();
        backBtn.OnClick += OnClickBack;

        LoadApplicationUI();
        SetPhoneState(PhoneState.Application);
    }

    public void SetPhoneState(PhoneState state)
    {
        switch(state)
        {
            case PhoneState.Messenger:
                openLayerTuStack.Push((PhoneState.Messenger, messengerParent.gameObject));
                closeLayerTuStack.Push((PhoneState.Application, applicationParent.gameObject));
                ShowCloseBtn(true);
                LayerControl();
                break;
            case PhoneState.Application:
                openLayerTuStack.Push((PhoneState.Application, applicationParent.gameObject));
                closeLayerTuStack.Push((PhoneState.Application, null));
                LayerControl();
                break;
        }
    }

    void LayerControl()
    {
        if (openLayerTuStack.Peek().Item2 != null)
        {
            phoneState = openLayerTuStack.Peek().Item1;
            openLayerTuStack.Peek().Item2.SetActive(true);
        }
        if (closeLayerTuStack.Peek().Item2 != null)
            closeLayerTuStack.Peek().Item2.SetActive(false);
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


    #region [ Common ]


    public void ShowCloseBtn(bool isOn)
    {
        backBtn.gameObject.SetActive(isOn);
    }

    void OnClickBack()
    {
        //CloseApplication(phoneState);
        if (openLayerTuStack.Peek().Item2 != null)
            openLayerTuStack.Pop().Item2.SetActive(false);
        if (closeLayerTuStack.Peek().Item2 != null)
        {
            phoneState = closeLayerTuStack.Peek().Item1;
            closeLayerTuStack.Pop().Item2.SetActive(true);
        }

        if (phoneState == PhoneState.Application)
            ShowCloseBtn(false);
    }

    void CloseApplication(PhoneState state)
    {
        switch(state)
        {
            case PhoneState.Messenger:
                SetPhoneState(PhoneState.Application);
                break;
            case PhoneState.Application:
                return;
        }

        ShowCloseBtn(false);
        applicationParent.gameObject.SetActive(true);

        phoneState = PhoneState.Application;
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            SetPhoneState(PhoneState.Messenger);
    }
}
