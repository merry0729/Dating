using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConfirm : MonoBehaviour
{
    UIButton confirmBtn;
    UIButton cancelBtn;

    public delegate void OnConfirmCallback();
    OnConfirmCallback confirmCallback;

    private void Awake()
    {
        confirmBtn.OnClick += OnClickConfirm;
        cancelBtn.OnClick += OnClickCancel;
    }

    void OnClickConfirm()
    {

    }

    void OnClickCancel()
    {
        gameObject.SetActive(false);
    }
}
