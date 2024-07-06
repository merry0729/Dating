using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UIConfirm;
using static UIManager;

public class UIConfirm : MonoBehaviour
{
    public UIButton confirmBtn;
    public UIButton cancelBtn;

    private void Awake()
    {
        confirmBtn.OnClick += OnClickConfirm;
        cancelBtn.OnClick += OnClickCancel;
    }

    public void ShowConfirmDialog(OnConfirmCallback callback)
    {
        UIManager.Instance.confirmCallback = callback;
    }

    void OnClickConfirm()
    {
        gameObject.SetActive(false);
        UIManager.Instance.confirmCallback?.Invoke();
    }

    void OnClickCancel()
    {
        gameObject.SetActive(false);
    }
}
