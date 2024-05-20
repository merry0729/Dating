using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonBase : Button, IPointerMoveHandler
{
    public PointerEventData.InputButton clickType = PointerEventData.InputButton.Left;

    // 더블 클릭.
    public bool doubleClick = false;
    private float clickTime = 0;

    // Enter.
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Enter();
    }

    // Exit.
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        Exit();
    }

    // Down.
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Down();
    }

    // Up.
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        Up();
    }

    // Click.
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (doubleClick)
        {
            if (Time.time - clickTime < 0.3f)
                OnClick();
            else
                clickTime = Time.time;
        }
        else
        {
            OnClick();
        }

        void OnClick()
        {
            if (eventData.button == PointerEventData.InputButton.Left
                && clickType == PointerEventData.InputButton.Left)
                Click();
            else if (eventData.button == PointerEventData.InputButton.Right
                    && clickType == PointerEventData.InputButton.Right)
                Click();
            else if (eventData.button == PointerEventData.InputButton.Middle
                    && clickType == PointerEventData.InputButton.Middle)
                Click();
        }
    }

    // Move.
    public void OnPointerMove(PointerEventData eventData)
    {
        Move();
    }

    public virtual void Enter()
    {
        Debug.Log($"Base Enter");
    }

    public virtual void Exit()
    {
        Debug.Log($"Base Exit");
    }

    public virtual void Down()
    {
        Debug.Log($"Base Down");
    }

    public virtual void Up()
    {
        Debug.Log($"Base Up");
    }

    public virtual void Click()
    {
        Debug.Log($"Base Click");
    }

    public virtual void Move()
    {
        Debug.Log($"Base Move");
    }
}
