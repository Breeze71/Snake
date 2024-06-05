using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V.Tool;
using V.UI;

public class EndGameUI : ButtonUIBase
{
    [Header("Button")]
    [SerializeField] private UITriggerEvent _returnToMenu;
    [SerializeField] private UITriggerEvent _showteam;

    protected override void OnEnable()
    {
        base.OnEnable();

        _returnToMenu.OnClickEvent += ReturnMenu_OnClick;
        _showteam.OnClickEvent += Showteam_OnClick;

        SetSelectNothing();
    }

    private void Showteam_OnClick(UITriggerEvent @event)
    {
        AudioManager.I.PlayOneShotSound(AudioManager.I._audioSO.UIClickClip);
        UIManager.Instance.ShowUI<TeamUI>("TeamUI");
    }

    private void ReturnMenu_OnClick(UITriggerEvent @event)
    {
        AudioManager.I.PlayOneShotSound(AudioManager.I._audioSO.UIClickClip);
        Loader.LoadScene(Scene.HOME);
    }

    protected override void OnDisable() 
    {
        base.OnDisable();

        _returnToMenu.OnClickEvent -= ReturnMenu_OnClick;
        _showteam.OnClickEvent -= Showteam_OnClick;
    }
}
