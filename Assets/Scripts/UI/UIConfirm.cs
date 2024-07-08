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
        gameObject.SetActive(false);

        confirmBtn.OnClick += OnClickConfirm;
        cancelBtn.OnClick += OnClickCancel;
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
