using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace V.UI
{
    public enum ButtonsLayoutState
    {
        None,
        Vertical,
        Horizontal,
        Both,
    }  

    public class ButtonUIBase : UIBase
    {
        [Header("First Select")]
        [SerializeField] protected GameObject[] buttons;
        [SerializeField] protected ButtonsLayoutState buttonsLayoutState;

        protected GameObject LastSelect;
        protected int LastSelectIndex;


        #region Life Cycle
        protected virtual void OnEnable() 
        {
            StartCoroutine(SetFirstSelectOBJ(buttons[0]));

            InputManager.Instance.OnConfirmEvent += VUIInputManager_OnConfirmEvent;

            foreach(GameObject _go in buttons)
            {
                _go.GetComponent<UITriggerEvent>().OnSelectedEvent += BTN_OnSelectedEvent;
            }
        }
        
        protected virtual void OnDisable() 
        {
            InputManager.Instance.OnConfirmEvent -= VUIInputManager_OnConfirmEvent;

            foreach(GameObject _go in buttons)
            {
                _go.GetComponent<UITriggerEvent>().OnSelectedEvent -= BTN_OnSelectedEvent;
            }                
        }

        // 為了適應各種選擇方式(滑鼠，鍵盤，手柄)
        protected virtual void Update()
        {
            switch(buttonsLayoutState)
            {
            //     case ButtonsLayoutState.None:
            //         return;

                case ButtonsLayoutState.Vertical:
                    if(InputManager.Instance.NavigationInput.y > 0)
                    {
                        HandleNextSelection(-1);
                    }
                    if(InputManager.Instance.NavigationInput.y < 0)
                    {
                        HandleNextSelection(1);
                    }       
                    break;             
                
                case ButtonsLayoutState.Horizontal:
                    if(InputManager.Instance.NavigationInput.x > 0)
                    {
                        HandleNextSelection(1);
                    }
                    if(InputManager.Instance.NavigationInput.x < 0)
                    {
                        HandleNextSelection(-1);
                    }
                    break; 
            }
        }

        /// <summary>
        /// 預設選擇
        /// </summary>
        protected IEnumerator SetFirstSelectOBJ(GameObject _go)
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(_go);
        }

        /// <summary>
        /// 鍵盤輸入的記住選擇
        /// </summary>
        private void HandleNextSelection(int _addition)
        {
            if(EventSystem.current.currentSelectedGameObject == null && LastSelect != null)
            {
                int _newIndex = LastSelectIndex + _addition;

                // 頭尾相連
                if(_newIndex > buttons.Length - 1)
                {
                    _newIndex = 0;
                }
                else if(_newIndex < 0)
                {
                    _newIndex = buttons.Length - 1;
                }

                Mathf.Clamp(_newIndex, 0, buttons.Length - 1);

                EventSystem.current.SetSelectedGameObject(buttons[_newIndex]);
            }
        }
        #endregion
        
        /// <summary>
        /// 記住 Last Select
        /// </summary>
        protected virtual void BTN_OnSelectedEvent(GameObject @object)
        {
            LastSelect = @object;

            // Find the Index
            for(int i = 0; i < buttons.Length; i++)
            {
                if(buttons[i] == @object)
                {
                    LastSelectIndex = i;
                    return;
                }
            }
        }

        /// <summary>
        /// Select 時，按下 Submit 按鈕 會觸發 OnClick
        /// </summary>
        private void VUIInputManager_OnConfirmEvent()
        {
            // 呼叫被 Select 的
            if(LastSelect == null) return;
            
            LastSelect.GetComponent<UITriggerEvent>().OnSelectedConfirmEvent();
        }
    }
}
