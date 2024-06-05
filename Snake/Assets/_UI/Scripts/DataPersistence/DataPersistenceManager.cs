using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

// 只有在 OnSceneLoad and Awake 會 LoadData，所以中途生成的不會讀到數據(需另外處理，或是在生成時 Load)
namespace V.Tool.SaveLoadSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance { get; private set;}

        [Header("File Storage")]
        [SerializeField] private string fileName;  
        [SerializeField] private bool isUseEncryption;

        /// <summary>
        /// 當前存檔
        /// </summary>
        private GameData gameData;
        /// <summary>
        /// All Implment IDataPersistable's Script
        /// </summary>
        private List<IDataPersistable> dataPersistableList;
        /// <summary>
        /// 用於處理 Data -> Json, Json -> Data
        /// </summary>
        private FileDataHandler fileDataHandler;
        /// <summary>
        /// 當前選擇存檔的 Id
        /// </summary>
        private string selectedProfileId = "";

        [Header("Polish")]
        [SerializeField] private float autoSaveTimerMax = 300f;
        [SerializeField] private bool isUseAutoSave = true;
        private Coroutine autoSaveCoroutine;

        [Header("Test")]
        [SerializeField] private bool initGamedataIfNull;


        #region Life Cycle
        private void Awake() 
        {
            // 每個場景都有，但一有重複就刪掉
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }    

            Instance = this;
            DontDestroyOnLoad(gameObject);

            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, isUseEncryption);
            
            // 預設存檔為最近一次
            selectedProfileId = fileDataHandler.GetMostRecentlyUpdateProfileId();
            
            // 避免 SceneManager_OnSceneLoaded 沒觸發(目前第一個場景穩定不觸發)
            dataPersistableList = FindAllDataPersistance();
            LoadGame(); 
        }

        private void OnEnable() 
        {
            SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
        }

        /* 僅次於 OnEnable，場景載入時，讀取存檔 */
        public void SceneManager_OnSceneLoaded(Scene _scene, LoadSceneMode _loadSceneMode)
        {
            dataPersistableList = FindAllDataPersistance();
            
            LoadGame(); 
            SaveGame();

            if(isUseAutoSave)
            {
                if(autoSaveCoroutine != null)
                {
                    StopCoroutine(Coroutine_AutoSave());
                }

                autoSaveCoroutine = StartCoroutine(Coroutine_AutoSave());
            }
        }

        private void OnDisable() 
        {
            SaveGame();

            SceneManager.sceneLoaded -= SceneManager_OnSceneLoaded;
        }
        
        private void OnApplicationQuit() 
        {
            SaveGame();
        }

        /// <summary>
        /// 找出場景所有有 IDataPersistable 的 Mono
        /// </summary>
        private List<IDataPersistable> FindAllDataPersistance()
        {
            IEnumerable<IDataPersistable> _dataPersistableList = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<IDataPersistable>();

            return new List<IDataPersistable>(_dataPersistableList);
        }
        #endregion
        
        #region Start & Save & Load
        public void StartNewGame()
        {
            gameData = new GameData();

            // 新存檔 json 
            fileDataHandler.SaveAsJson(gameData, selectedProfileId); 
        }

        public void LoadGame()
        {
            // Load save data from json
            gameData = fileDataHandler.LoadJson(selectedProfileId);

            // For Test
            if(gameData == null && initGamedataIfNull)
            {
                StartNewGame();
            }

            if(gameData == null)
            {
                Debug.LogWarning("There is no save game data found");
                return;
            }
            if(dataPersistableList == null)
            {
                Debug.Log("No data need to load");
                return;
            }

            // 讀取傳入的 GameData
            foreach(IDataPersistable _dataPersistable in dataPersistableList)
            {
                _dataPersistable.LoadData(gameData);
            }
        }
        
        /// <summary>
        /// 只有在 OnSceneLoad and Awake 會 LoadData，所以中途生成的不會讀到數據(需另外處理，或是在生成時 Load)
        /// </summary>
        /// <param name="_dataPersistable"></param>
        public void LoadOneData(IDataPersistable _dataPersistable)
        {
            // Load save data from json
            gameData = fileDataHandler.LoadJson(selectedProfileId);

            // For Test
            if(gameData == null && initGamedataIfNull)
            {
                StartNewGame();
            }

            if(gameData == null)
            {
                Debug.LogWarning("There is no save game data found");
                return;
            }
            if(dataPersistableList == null)
            {
                Debug.Log("No data need to load");
                return;
            }
            
            dataPersistableList.Add(_dataPersistable);

            _dataPersistable.LoadData(gameData);
        }

        public void SaveGame()
        {
            if(gameData == null)
            {
                Debug.LogWarning("No data was found, a new game need to be started before data can be save");
                return;
            }
            if(dataPersistableList == null)
            {
                return;
            }
            // Debug.Log("Save Game");

            // 修改並存除傳入的 GameData
            foreach(IDataPersistable _dataPersistable in dataPersistableList)
            {
                _dataPersistable.SaveData(gameData);
            }

            // 存儲更新時間
            gameData.lastUpdate = System.DateTime.Now.ToBinary();
            
            // Save the data to json
            fileDataHandler.SaveAsJson(gameData, selectedProfileId); 
        }
        #endregion

        #region Interface
        /// <summary>
        /// 獲取所有 data
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, GameData> GetAllProfilesGameData()
        {
            return fileDataHandler.LoadAllProfiles();
        }

        /// <summary>
        /// 更換成該存檔
        /// </summary>
        /// <param name="_newProfileId"></param>
        public void ChangeSelectedProfileId(string _newProfileId)
        {
            // 切換存檔 Id　並讀取
            selectedProfileId = _newProfileId;

            LoadGame();
        }
        
        // 用於 ui 
        public bool HasGameData()
        {
            return gameData != null;
        }

        public void UpdateDataPersistableList()
        {
            dataPersistableList = FindAllDataPersistance();
        }
        #endregion
    
        #region Polish (Delete, AutoSave)
        private IEnumerator Coroutine_AutoSave()
        {
            while(true)
            {
                yield return new WaitForSeconds(autoSaveTimerMax);
                SaveGame();

                Debug.Log("Auto Save");
            }
        }
        #endregion
    }
}
