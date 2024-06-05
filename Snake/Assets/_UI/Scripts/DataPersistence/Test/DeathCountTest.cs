using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace V.Tool.SaveLoadSystem
{   
    /// <summary>
    /// 用於測試存讀檔紀錄死亡次數
    /// </summary>
    public class DeathCountTest : MonoBehaviour//, IDataPersistable
    {
        // private int currentDeathCount = 0;

        // [SerializeField] private TextMeshProUGUI deathCountText;

        // #region Unity

        // private void Start() 
        // {
        //     GameEventsManager.Instance.PlayerEvent.OnPlayerDead += PlayerEvent_OnPlayerDead;
        // }
        // private void OnDestroy()
        // {
        //     GameEventsManager.Instance.PlayerEvent.OnPlayerDead -= PlayerEvent_OnPlayerDead;            
        // }
        // #endregion

        // /// <summary>
        // /// Recieve Player Dead Event
        // /// </summary>
        // private void PlayerEvent_OnPlayerDead()
        // {
        //     currentDeathCount++;
        //     deathCountText.text = "DeathCount " + currentDeathCount; // Increase Dead count
        // }

        // #region IDataPersistable
        // public void LoadData(GameData _gameData)
        // {
        //     currentDeathCount = _gameData.deathCount;

        //     deathCountText.text = "DeathCount " + currentDeathCount;
        // }

        // public void SaveData(GameData _gameData)
        // {
        //     _gameData.deathCount = currentDeathCount;
        // }
        // #endregion
    }
}
