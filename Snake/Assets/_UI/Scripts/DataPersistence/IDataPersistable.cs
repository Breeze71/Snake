using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V.Tool.SaveLoadSystem
{
    /// <summary>
    /// Implement to the script need to Save & Load
    /// </summary>
    public interface IDataPersistable
    {
        public void LoadData(GameData _gameData);

        /// <summary>
        /// 傳入的是 ref，需能 Modify the data
        /// </summary>
        public void SaveData(GameData _gameData);
    }
}
