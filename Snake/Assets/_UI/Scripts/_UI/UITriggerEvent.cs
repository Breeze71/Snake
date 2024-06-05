using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using V.Tool.JuicyFeeling;

namespace V.UI
{
    /// <summary>
    /// 為了適應各種選擇方式(滑鼠，鍵盤，手柄)
    /// </summary>
    public class UITriggerEvent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action<GameObject> OnSelectedEvent;
        public event Action<UITriggerEvent> OnClickEvent;

        private SquashAndStretch squashAndStretch;

        public bool isInteractable {get; private set;} = true;
        private Button button;

        private void Awake() 
        {
            squashAndStretch = GetComponent<SquashAndStretch>();  
            button = GetComponent<Button>();  
        }
        private void OnDisable() 
        {
            squashAndStretch.ResetScale();
        }

        #region Pointer
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!isInteractable) return;

            eventData.selectedObject = gameObject;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!isInteractable) return;

            OnClickEvent?.Invoke(this);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if(!isInteractable) return;

            eventData.selectedObject = null;
        }
        #endregion

        #region Select
        public void OnSelect(BaseEventData eventData)
        {
            if(!isInteractable) return;

            OnSelectedEvent?.Invoke(gameObject);

            squashAndStretch.PlaySquashAndStretch();
        }
        public void OnDeselect(BaseEventData eventData)
        {
            if(!isInteractable) return;

            squashAndStretch.ResetScale();
        }
        #endregion

        public void OnSelectedConfirmEvent()
        {
            if(!isInteractable) return;

            squashAndStretch.ResetScale();
            OnClickEvent?.Invoke(this);
        }

        public void SetUIInteractable(bool _isInteractable)
        {
            Debug.Log(gameObject.name + " set " + _isInteractable);

            isInteractable = _isInteractable;
            button.enabled = _isInteractable;
        }

    }
}
