using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
                break;
            case MenuType.Achievement:
                Debug.Log($"Achievement Menu");
                break;
            case MenuType.Infomation:
                Debug.Log($"Infomation Menu");
                break;
            case MenuType.Setting:
                Debug.Log($"Setting Menu");
                break;
            case MenuType.Exit:
                Debug.Log($"Exit Menu");
                break;
        }
    }
}
