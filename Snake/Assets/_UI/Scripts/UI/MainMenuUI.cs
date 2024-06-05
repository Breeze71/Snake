using V.UI;
using UnityEngine;
using V.Tool;
using Scene = V.Tool;
using NaughtyAttributes;
using System;

namespace V.Tool.SaveLoadSystem
{
    public class MainMenuUI : ButtonUIBase, IDataPersistable
    {
        [Header("Menu Button")]
        [SerializeField] private UITriggerEvent newGame;
        [SerializeField] private UITriggerEvent continueGame;
        [SerializeField] private UITriggerEvent loadGame;
        [SerializeField] private UITriggerEvent quitGame;

        private Scene lastPlayScene;

        private void Awake() 
        {
            if(DataPersistenceManager.Instance == null) return; 
            DataPersistenceManager.Instance.LoadOneData(this);
        }

        protected override void OnEnable() 
        {
            base.OnEnable();

            newGame.OnClickEvent += NewGame_OnClickEvent;
            continueGame.OnClickEvent += ContinueGame_OnClickEvent;
            loadGame.OnClickEvent += LoadGame_OnClickEvent;
            quitGame.OnClickEvent += QuitGame_OnClickEvent;

            // 沒 game data 時，不能按讀檔和繼續
            if(DataPersistenceManager.Instance == null) return; 
            Debug.Log("Find Game data " + DataPersistenceManager.Instance.HasGameData());

            if(!DataPersistenceManager.Instance.HasGameData())
            {
                continueGame.SetUIInteractable(false);
                loadGame.SetUIInteractable(false);
            }    
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            newGame.OnClickEvent -= NewGame_OnClickEvent;
            continueGame.OnClickEvent -= ContinueGame_OnClickEvent;
            loadGame.OnClickEvent -= LoadGame_OnClickEvent;
            quitGame.OnClickEvent -= QuitGame_OnClickEvent;
        }

        private void NewGame_OnClickEvent(UITriggerEvent @event)
        {
            // SaveSlotsUI _saveSlotsUI;
            // _saveSlotsUI = V.UI.UIManager.Instance.ShowUI<SaveSlotsUI>("SaveSlotsUI").GetComponent<SaveSlotsUI>();
            // _saveSlotsUI.GetLastOpenUIName(this);
            // _saveSlotsUI.ActiveSaveSlot(false);
            // // lastPlayScene = Scene.ForeWord;

            // Hide();   

            Loader.LoadScene(Scene.UI);
            InputManager.Instance.SetActionMap(InputType.GamePlay);
        }
        
        private void ContinueGame_OnClickEvent(UITriggerEvent @event)
        {
            Loader.LoadScene(lastPlayScene);       
        }

        // 顯示存檔 UI
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
            Application.Quit();
        }

        public void LoadData(GameData _gameData)
        {
            lastPlayScene = _gameData.CurrentScene;  
        }

        public void SaveData(GameData _gameData)
        {      
            
        }
    }
}
