using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V;
using V.UI;

public class TeamUI : ButtonUIBase
{
    [Header("Button")]
    [SerializeField] private UITriggerEvent _returnMenu;

    protected override void OnEnable()
    {
        base.OnEnable();

        _returnMenu.OnClickEvent += ReturnMenu_OnClick;
        InputManager.Instance.ResumeEvent += InputManager_ResumeEvent;

        SetSelectNothing();
    }

    private void InputManager_ResumeEvent()
    {
        gameObject.SetActive(false);
    }

    private void ReturnMenu_OnClick(UITriggerEvent @event)
    {
        gameObject.SetActive(false);
    }

    protected override void OnDisable() 
    {
        base.OnDisable();

        _returnMenu.OnClickEvent -= ReturnMenu_OnClick;
    }

}
