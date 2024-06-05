using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V.Tool.SaveLoadSystem;
using UnityEngine.SceneManagement;

namespace V
{
    public class SaveSceneIndex : MonoBehaviour, IDataPersistable
    {
        public void LoadData(GameData _gameData)
        {
            
        }

        public void SaveData(GameData _gameData)
        {
            _gameData.CurrentScene = (V.Tool.AsyncLoader.Scene)SceneManager.GetActiveScene().buildIndex;
        }
    }
}
