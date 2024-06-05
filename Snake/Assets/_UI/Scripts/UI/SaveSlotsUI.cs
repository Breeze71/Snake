using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using V.Tool;
using V.UI;

namespace V.Tool.SaveLoadSystem
{
    public class SaveSlotsUI : ButtonUIBase
    {
        [SerializeField] private UITriggerEvent back;
        [SerializeField] private UITriggerEvent[] saveSlots;

        private bool isLoadingGame = false;

        private UIBase lastOpenUI;

        protected override void OnEnable() 
        { 
            base.OnEnable();
            
            back.OnClickEvent += Back_OnClickEvent;

            // 每個存檔按鈕
            foreach(UITriggerEvent _saveSlot in saveSlots)
            {
                _saveSlot.OnClickEvent += SaveSlot_OnClickEvent;
            }
        }

        private void SaveSlot_OnClickEvent(UITriggerEvent @event)
        {
            SaveSlotButton _currentSaveSlot = @event.GetComponent<SaveSlotButton>();
            Debug.Log("loading saveId: " + _currentSaveSlot.GetSaveSlotProfileId());
                    
            OnSaveSlotClicked(_currentSaveSlot);
        }

        protected override void OnDisable() 
        {
            base.OnDisable();

            back.OnClickEvent -= Back_OnClickEvent; 

            // 每個存檔按鈕
            foreach(UITriggerEvent _saveSlot in saveSlots)
            {
                _saveSlot.OnClickEvent -= SaveSlot_OnClickEvent;
            }
        }

        private void OnSaveSlotClicked(SaveSlotButton _saveSlotBTN)
        {
            DisableAllButtons();

            // update selected profile
            DataPersistenceManager.Instance.ChangeSelectedProfileId(_saveSlotBTN.GetSaveSlotProfileId());

            if(!isLoadingGame)
            {
                DataPersistenceManager.Instance.StartNewGame();
                // Loader.LoadScene(Scene.ForeWord);
                Debug.Log("fix");
                Debug.LogWarning("Change to fore world");
                return;
            }

            Loader.LoadScene(_saveSlotBTN.GetCurrentScene());
        }

        // Disable all button (避免連點出 bug)
        private void DisableAllButtons()
        {
            foreach(UITriggerEvent _saveSlot in saveSlots)
            {
                _saveSlot.SetUIInteractable(false);
            }

            back.SetUIInteractable(false);
        }

        /// <summary>
        /// 將 UI 和 相應存檔關聯
        /// </summary>
        /// <param name="_isLoadingGame"> 如果是選擇讀存檔，該欄位沒存檔時不可選 </param>
        public void ActiveSaveSlot(bool _isLoadingGame)
        {
            Debug.Log("Active Save slot");
            isLoadingGame = _isLoadingGame;

            Dictionary<string, GameData> _profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

            GameObject _firstSelected = saveSlots[0].gameObject;

            // 將 UI 和 相應存檔關聯
            foreach(UITriggerEvent _saveSlot in saveSlots)
            {
                GameData _profileData = null;
                SaveSlotButton _saveSlotButton = _saveSlot.GetComponent<SaveSlotButton>();

                _profilesGameData.TryGetValue(_saveSlotButton.GetSaveSlotProfileId(), out _profileData);
                _saveSlotButton.SetData(_profileData);

                // // 如果是選擇讀存檔，該欄位沒存檔時不可選
                // if(_profileData == null && isLoadingGame)
                // {
                //     // 
                //     Debug.LogWarning("_saveSlot.SetButtonInteractable(false);");
                //     _saveSlot.SetUIInteractable(false);
                // }

                // // 其他情況(有存檔，無存檔但是新遊戲) 可選
                // else
                // {
                //     //
                //     Debug.LogWarning("_saveSlot.SetButtonInteractable(true);");
                //     _saveSlot.SetUIInteractable(true);

                //     // 確保 firstSelect 在第一個可選擇的按鈕上
                //     if(_firstSelected.Equals(back.gameObject))
                //     {
                //         _firstSelected = _saveSlot.gameObject;
                //     }
                // }                
            }

            StartCoroutine(SetFirstSelectOBJ(_firstSelected));
        }
    
        // Remember Last UI
        public void GetLastOpenUIName(UIBase _uIBase)
        {
            lastOpenUI = _uIBase;
        }

        private void Back_OnClickEvent(UITriggerEvent @event)
        {
            lastOpenUI.Show();

            Hide();            
        }

    }
}
