using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace V.UI
{
    public class UIBase : MonoBehaviour
    {
        private void Start() 
        {
            UIManager.Instance.AddToUIList(this);
        }
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Close()
        {
            UIManager.Instance.CloseUI(gameObject.name);
        }

        /// <summary>
        /// 點擊一次打開，再次點擊關閉
        /// </summary>
        public void ClickOpenClickClose()
        {
            if(gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }           
        }
    }
}
