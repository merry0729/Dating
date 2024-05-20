using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : UIButtonBase
{
    public override void Click()
    {
        base.Click();
        Debug.Log($"UIButton_LeftClick");
    }
}
