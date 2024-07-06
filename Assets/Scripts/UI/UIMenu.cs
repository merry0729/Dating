using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIMenu : MonoBehaviour
{
    MenuType menuType;

    UIButton menuBtn;
    Image menuImage;
    TextMeshProUGUI menuText;

    string directoryPath = "UI/Menu/";

    MenuTable menuTable;
    MenuData menuData;

    private void Awake()
    {
        menuTable = MenuData.Table;
    }

    void Start()
    {
        menuBtn = GetComponentInChildren<UIButton>();
        menuBtn.OnClick += () => OnClickMenu(menuType);
    }

    private void OnDestroy()
    {
        if (menuBtn != null)
            menuBtn.OnClick -= () => OnClickMenu(menuType);
    }

    public void SetMenu(MenuType menu)
    {
        menuImage = GetComponentInChildren<Image>();
        menuText = GetComponentInChildren<TextMeshProUGUI>();

        menuType = menu;
        menuData = menuTable.TryGet((int)menu);

        Sprite menuSpr = Resources.Load<Sprite>(directoryPath + menuData.MenuFileName);
        menuImage.sprite = menuSpr;

        menuText.text = menuType.ToString();
    }

    void OnClickMenu(MenuType menu)
    {
        switch(menuType)
        {
            case MenuType.Status:
                Debug.Log($"Status Menu");
                PlayManager.Instance.ReverseActivePlayUI(PlayUIType.Status);
                break;
            case MenuType.Achievement:
                Debug.Log($"Achievement Menu");
                break;
            case MenuType.Phone:
                Debug.Log($"Phone Menu");
                PlayManager.Instance.ReverseActivePlayUI(PlayUIType.Phone);
                break;
            case MenuType.UIClear:
                Debug.Log($"UI Clear Menu");
                PlayManager.Instance.ActiveAllUI(false);
                break;
            case MenuType.SaveLoad:
                Debug.Log($"Save Load Menu");
                break;
            case MenuType.Setting:
                Debug.Log($"Setting Menu");
                UIManager.Instance.ActiveWindowUI(WindowUIType.SettingUI, true);
                break;
            case MenuType.Exit:
                Debug.Log($"Exit Menu");
                UIManager.Instance.ActiveWindowUI(WindowUIType.ConfirmUI, true, ConfirmExit);
                break;
        }
    }

    void ConfirmExit()
    {
        Debug.Log($"ConfirmExit");
    }
}
