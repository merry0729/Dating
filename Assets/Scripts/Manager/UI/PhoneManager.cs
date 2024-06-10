using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneManager : Singleton<PhoneManager>
{
    public Transform messengerParent;

    public ScrollRect messengerScrollRect_Sender;
    public Transform messengerContent_Sender;

    public GameObject messengerSenderPrefab;

    public ScrollRect messengerScrollRect_Message;
    public Transform messengerContent_Message;

    void Start()
    {
        
    }

    void SetPhoneData()
    {

    }

    public void SetPhoneUI()
    {
        messengerParent = PlayManager.Instance.phoneParent.transform.Find("Messenger");
        messengerScrollRect_Sender = messengerParent.Find("Messenger_Scroll").GetComponent<ScrollRect>();
        messengerContent_Sender = messengerScrollRect_Sender.transform.Find("Viewport").Find("Content");
    }

    void ShowMessenger()
    {
        messengerParent.gameObject.SetActive(true);
        LoadMessenger();
    }

    void LoadMessenger()
    {
        for(int index = 0; index < 5; index++)
            Instantiate(messengerSenderPrefab, messengerContent_Sender);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            ShowMessenger();
    }
}
