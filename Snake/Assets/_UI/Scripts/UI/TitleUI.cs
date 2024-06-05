using System;
using V.Tool.SaveLoadSystem;

namespace V.UI
{
    /// <summary>
    /// 按下按鈕後開始
    /// </summary>
    public class TitleUI : UIBase
    {
        private void OnEnable() 
        {
            InputManager.Instance.OnSubmitEvent += VUIInputManager_OnSubmitEvent;
            InputManager.Instance.OnConfirmEvent += VUIInputManager_OnConfirmEvent;
        }

        private void OnDisable() 
        {
            InputManager.Instance.OnSubmitEvent -= VUIInputManager_OnSubmitEvent;
            InputManager.Instance.OnConfirmEvent -= VUIInputManager_OnConfirmEvent;
        }

        private void VUIInputManager_OnConfirmEvent()
        {
            UIManager.Instance.ShowUI<V.Tool.SaveLoadSystem.MainMenuUI>("MainMenuUI");

            UIManager.Instance.CloseUI(name);
        }

        private void VUIInputManager_OnSubmitEvent()
        {
            UIManager.Instance.ShowUI<V.Tool.SaveLoadSystem.MainMenuUI>("MainMenuUI");

            UIManager.Instance.CloseUI(name);
        }

    }
}
