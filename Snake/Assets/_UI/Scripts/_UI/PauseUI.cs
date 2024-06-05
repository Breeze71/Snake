using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using V.Tool;
using V.Tool.JuicyFeeling;
using V.Tool.SaveLoadSystem;

namespace V.UI
{
    public class PauseUI : ButtonUIBase
    {   
        [Header("Button")]
        [SerializeField] private UITriggerEvent returnToGame;
        [SerializeField] private UITriggerEvent loadGame;
        [SerializeField] private UITriggerEvent options;
        [SerializeField] private UITriggerEvent quitGame;

        // [Header("Open Sub Menu")]
        // [SerializeField] private GameObject QuitUI;

        protected override void OnEnable()
        {
            base.OnEnable();

            returnToGame.OnClickEvent += ReturnToGame_OnClickEvent;
            loadGame.OnClickEvent += LoadGame_OnClickEvent;
            options.OnClickEvent += Options_OnClickEvent;;
            quitGame.OnClickEvent += QuitGame_OnClickEvent;

            InputManager.Instance.ResumeEvent += InputManager_OnResume;
        }

        private void InputManager_OnResume()
        {
            InputManager.Instance.SetActionMap(InputType.GamePlay);
            gameObject.SetActive(false);
        }

        protected override void OnDisable() 
        {
            base.OnDisable();

            returnToGame.OnClickEvent -= ReturnToGame_OnClickEvent;
            loadGame.OnClickEvent -= LoadGame_OnClickEvent;
            options.OnClickEvent -= Options_OnClickEvent;;
            quitGame.OnClickEvent -= QuitGame_OnClickEvent;   

            InputManager.Instance.ResumeEvent -= InputManager_OnResume;
        }

        private void ReturnToGame_OnClickEvent(UITriggerEvent @event)
        {
            InputManager.Instance.SetActionMap(InputType.GamePlay);
            InputManager.Instance.InvokeResume();
            Debug.Log("d");
            Hide();
        }

        private void LoadGame_OnClickEvent(UITriggerEvent @event)
        {
            SaveSlotsUI _saveSlotsUI;
            _saveSlotsUI = V.UI.UIManager.Instance.ShowUI<SaveSlotsUI>("SaveSlotsUI").GetComponent<SaveSlotsUI>();
            _saveSlotsUI.GetLastOpenUIName(this);
            _saveSlotsUI.ActiveSaveSlot(true);   

            Hide();
        }

        private void QuitGame_OnClickEvent(UITriggerEvent @event)
        {
            Debug.Log("quit");
            Loader.LoadScene(Scene.HOME);
        }


        private void Options_OnClickEvent(UITriggerEvent @event)
        {
            
        }
    }
}
