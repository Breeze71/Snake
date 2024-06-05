using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace V.Tool.SaveLoadSystem
{   
    /// <summary>
    /// 處理 json 文件
    /// </summary>
    public class FileDataHandler
    {
        /// <summary>
        /// 目錄文件夾
        /// </summary>
        private string dataDirectoryPath = "";

        /// <summary>
        /// 文件名稱
        /// </summary>
        private string dataFileName = "";

        /// <summary>
        /// XOR (Exclusive Or) Encryption
        /// </summary>
        private bool isUseEncryption = false;
        private readonly string encryptionCode = "Lucy";

        /// <summary>
        /// Back Up Extension
        /// </summary>
        private readonly string backUpExtension = ".bak";

        
        public FileDataHandler(string _dataDirectoryPath, string _dataFileName, bool _isUseEncryption)
        {
            dataDirectoryPath = _dataDirectoryPath;
            dataFileName = _dataFileName;
            isUseEncryption = _isUseEncryption;
        }

        public GameData LoadJson(string _profileId, bool _isAllowRestoreFromBackup = true)
        {
            // 如果 profileId == null , return(避免取最近遊玩存檔沒取到，即第一次遊玩)
            // string _mostRecentlyProfileId = null;
            // Path.Combine(dataDirectoryPath, _profileId, dataFileName) 避免找不到出 bug
            if(_profileId == null)
            {
                return null;
            }

            // 在任何 OS 中都 Equals to "dataDirectoryPath / _profileId(第幾個存檔) / dataFileName"
            string _fullPath = Path.Combine(dataDirectoryPath, _profileId, dataFileName);
            GameData _loadedData = null;

            // 沒文件直接 return null
            if(!File.Exists(_fullPath))
            {
                Debug.LogWarning("data not exist");
                return _loadedData;
            }

            // Try Load the File 
            try
            {
                string _dataToLoad = "";

                // Read the Serialize data from the file
                using(FileStream _strem = new FileStream(_fullPath, FileMode.Open))
                {
                    using (StreamReader _reader = new StreamReader(_strem))
                    {
                        _dataToLoad = _reader.ReadToEnd();
                    }
                }

                // Encryption
                if(isUseEncryption)
                {
                    _dataToLoad = EncryptDecrypt(_dataToLoad);
                }

                // Deserialize the data from Json to C# object
                _loadedData = JsonUtility.FromJson<GameData>(_dataToLoad);
            }

            catch(Exception _e)
            {
                if(_isAllowRestoreFromBackup)
                {
                    Debug.LogError("Error when Load the file. Attempting to roll back: " + _e);

                    bool _isRollBackSuccess = AttemptRollback(_fullPath);
                    if(_isRollBackSuccess)
                    {
                        _loadedData = LoadJson(_profileId, false); // false 為了避免 roll 不到，重複 roll back
                    }
                }
                else
                {
                    Debug.LogError("Error when Load the file: " + _fullPath + " and back up failed" + "\n" + _e);
                }
            }

            return _loadedData;
        }

        public void SaveAsJson(GameData _gameData, string _profileId)
        {
            // 如果 profileId == null , return(避免最近遊玩存檔沒取到，即第一次遊玩)
            // string _mostRecentlyProfileId = null;
            // Path.Combine(dataDirectoryPath, _profileId, dataFileName) 避免找不到出 bug
            if(_profileId == null)
            {
                return;
            }

            // 在任何 OS 中都 Equals to "dataDirectoryPath / dataFileName"
            string _fullPath = Path.Combine(dataDirectoryPath, _profileId, dataFileName);
            string _backupFilePath = _fullPath + backUpExtension;

            // Try Save the File
            try
            {
                // when not exist, Create the Directory the file will be written at
                Directory.CreateDirectory(Path.GetDirectoryName(_fullPath));

                // Serialize the C# data into Json
                string _dataToStore = JsonUtility.ToJson(_gameData, true);  // format true

                // Encryption
                if(isUseEncryption)
                {
                    _dataToStore = EncryptDecrypt(_dataToStore);
                }

                // Write the Serialize data to the file
                using(FileStream _strem = new FileStream(_fullPath, FileMode.Create))
                {
                    using(StreamWriter _writer = new StreamWriter(_strem))
                    {
                        _writer.Write(_dataToStore);
                    }
                }

                // 確保新存檔沒有損毀(如果沒損毀，複製一份)
                GameData _verifiedGameData = LoadJson(_profileId);
                if(_verifiedGameData != null)
                {
                    File.Copy(_fullPath, _backupFilePath, true);
                }
                else
                {
                    throw new Exception("Save file couldn't be verified and backup could't be create");
                }
            }
            catch(Exception _e)
            {
                Debug.LogError("Error when Save the file: " + _fullPath + "\n" + _e);
            }
        }

        /// <summary>
        /// 讀取所有存檔中最近更新時間，取出最近
        /// </summary>
        public string GetMostRecentlyUpdateProfileId()
        {
            string _mostRecentlyProfileId = null;

            Dictionary<string, GameData> _profileGameData = LoadAllProfiles();
            foreach(KeyValuePair<string, GameData> _pair in _profileGameData)
            {
                string _profileId = _pair.Key;
                GameData _gameData = _pair.Value;

                // skip if 讀到奇怪資料夾
                if(_gameData == null)
                {
                    continue;
                }

                // 先將第一個讀到的存檔設定為最近期存檔
                if(_mostRecentlyProfileId == null)
                {
                    _mostRecentlyProfileId = _profileId;
                }

                // 再去比較時間
                else
                {
                    DateTime _mostRecentlyDateTime = DateTime.FromBinary(_profileGameData[_mostRecentlyProfileId].lastUpdate);
                    DateTime _newDateTime = DateTime.FromBinary(_gameData.lastUpdate);

                    if(_newDateTime > _mostRecentlyDateTime)
                    {
                        _mostRecentlyProfileId = _profileId;
                    }
                }
            }
            
            return _mostRecentlyProfileId;
        }

        public Dictionary<string, GameData> LoadAllProfiles()
        {
            Dictionary<string, GameData> _profileIdDictionary = new Dictionary<string, GameData>();

            // Loop All directory names in data Directory Path 讀所有存檔檔案名
            IEnumerable<DirectoryInfo> _directoryInfoList = new DirectoryInfo(dataDirectoryPath).EnumerateDirectories();
            foreach(DirectoryInfo _directoryInfo in _directoryInfoList)
            {
                string _profileId = _directoryInfo.Name;

                // 避免讀到空或奇怪文件夾
                string _fullPath = Path.Combine(dataDirectoryPath, _profileId, dataFileName);
                if(!File.Exists(_fullPath))
                {
                    Debug.LogWarning("Skipping the directory because it doesn't contain data in "
                        + _profileId);
                    continue;
                }

                // 讀取該存檔，並加入 Dic 
                GameData _profileData = LoadJson(_profileId);
                if(_profileData != null)
                {
                    _profileIdDictionary.Add(_profileId, _profileData);
                }
                else
                {
                    Debug.LogError("Try To Load Profile From Nulti Sava But Somthing Wrong. ProfileId: "
                         + _profileId);
                }
            }

            return _profileIdDictionary;
        }

        /// <summary>
        /// XOR Encryption
        /// </summary>
        private string EncryptDecrypt(string _data)
        {
            string _modifyData = "";

            for(int i = 0; i < _data.Length; i++)
            {
                int _currentIndex = i % encryptionCode.Length;

                _modifyData += (char) (_data[i] ^ encryptionCode[_currentIndex]);
            }

            return _modifyData;
        }

        private bool AttemptRollback(string _fullPath)
        {
            bool _isSuccess = false;
            string _backupFilePath = _fullPath;

            try
            {
                // 如果 backup 存在，用 backup 復原掉損毀的 原存檔
                if(File.Exists(_backupFilePath))
                {
                    File.Copy(_backupFilePath, _fullPath, true);
                    _isSuccess = true;
                    Debug.LogWarning("Had to roll back to backup file at: " + _backupFilePath);
                }
                else
                {
                    throw new Exception("很不幸的，你的存檔跟存檔的備份都炸了");
                }

            }
            catch(Exception e)
            {
                Debug.LogError("Error when trying to roll back to back up file at: "
                     + _backupFilePath + "\n" + e);
            }

            return _isSuccess;
        }

        /// <summary>
        /// 刪除所有存檔
        /// </summary>
        public void DeleteAllSaveData()
        {
            string _fullPath = Path.Combine(dataDirectoryPath);
            Directory.Delete(_fullPath);
        }
    }
}
