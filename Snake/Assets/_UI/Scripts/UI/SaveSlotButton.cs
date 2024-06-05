using TMPro;
using UnityEngine;
using UnityEngine.UI;
using V.Tool;
using V.UI;

namespace V.Tool.SaveLoadSystem
{
    public class SaveSlotButton : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private string profileId = "";
        
        [Header("Content")]
        [SerializeField] private GameObject noDataContent;
        [SerializeField] private GameObject dataContent;

        [Header("UI Data")]
        [SerializeField] private TextMeshProUGUI percentageCompleteText;
        [SerializeField] private TextMeshProUGUI chapterIndexText;
        [SerializeField] private TextMeshProUGUI chapterNameText;
        [SerializeField] private TextMeshProUGUI dateTEXT;
        [SerializeField] private Image chapterImg;
        [SerializeField] private ChapterDataUISO chapterDataSO;

        private Scene currentScene;
        private Button saveSlotButton;
        private UITriggerEvent uITriggerEvent;

        private void Awake() 
        {
            saveSlotButton = GetComponent<Button>(); 
            uITriggerEvent = GetComponent<UITriggerEvent>();   
        }

        public void SetData(GameData _gameData)
        {
            if(_gameData == null)
            {
                noDataContent.SetActive(true);
                dataContent.SetActive(false);
            }
            else
            {
                noDataContent.SetActive(false);
                dataContent.SetActive(true);

                currentScene = _gameData.CurrentScene;

                percentageCompleteText.text = _gameData.GetPercentageComplete() + "% COMPLETE";
                
                dateTEXT.text = System.DateTime.FromBinary(_gameData.lastUpdate).ToString();

                SetUIFromScene(_gameData);
            }
        }

        /// <summary>
        /// 獲取場景 Index
        /// </summary>
        private void SetUIFromScene(GameData _gameData)
        {
            int _currentSceneIndex = (int)_gameData.CurrentScene;

            Debug.Log("ProfileId" + profileId + "Save Scene: " + _gameData.CurrentScene);

            // 確保是不是 MainMenu, LoadingScene...
            if(_currentSceneIndex > 3)
            {
                // 用空格分開　(Chapter 3-1 ???)
                string[] _chapterWholeNames = chapterDataSO.chapterWholeNames[_currentSceneIndex].Split(' ');
                chapterIndexText.text = _chapterWholeNames[1];
                chapterNameText.text = _chapterWholeNames[2];
                
                chapterImg.sprite = chapterDataSO.chapterSprites[_currentSceneIndex];
            }
            else if(_currentSceneIndex == 2)
            {
                chapterIndexText.text = "";
                chapterNameText.text = "SelectGameScene";
            }
            else if(_currentSceneIndex == 1)
            {
                chapterIndexText.text = "";
                chapterNameText.text = "ForeWord";                
            }
        }

        /// <summary>
        /// 用於關閉按鈕，避免連擊
        /// </summary>
        public void SetInteractable(bool _isInteractable)
        {
            saveSlotButton.interactable = _isInteractable;
            uITriggerEvent.SetUIInteractable(_isInteractable);
        }

        public string GetSaveSlotProfileId()
        {
            return profileId;
        }

        public Scene GetCurrentScene()
        {
            return currentScene;
        }
    }
}
