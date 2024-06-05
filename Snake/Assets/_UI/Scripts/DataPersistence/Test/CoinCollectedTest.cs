using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using V.Event;

namespace V.Tool.SaveLoadSystem
{
    public class CoinCollectedTest : MonoBehaviour//, IDataPersistable
    {
        // [SerializeField] private int totalCoins = 0;

        // private int currentCollectedCoin = 0;

        // [SerializeField] private TextMeshProUGUI coinsCollectedText;
        
        // private void Start() 
        // {
        //     GameEventsManager.Instance.CoinEvent.OnCoinCollected += CoinEvent_OnCoinCollected;
        // }
        // private void OnDestroy() 
        // {
        //     GameEventsManager.Instance.CoinEvent.OnCoinCollected -= CoinEvent_OnCoinCollected;
        // }

        // /// <summary>
        // /// Recieve Coin Event
        // /// </summary>
        // private void CoinEvent_OnCoinCollected()
        // {
        //     currentCollectedCoin++;
        //     coinsCollectedText.text = "Coins: " + currentCollectedCoin + " /  " + totalCoins; // Increase Dead count
        // }

        // #region IDataPersistable
        // public void LoadData(GameData _gameData)
        // {
        //     foreach(KeyValuePair<string, bool> _coinPair in _gameData.coinsCollectedDic)
        //     {
        //         if(_coinPair.Value)
        //         {
        //             currentCollectedCoin++;
        //         }
        //     }
            
        //     coinsCollectedText.text = "Coins: " + currentCollectedCoin + " /  " + totalCoins; // Increase Dead count
        // }

        // public void SaveData(GameData _gameData)
        // {
        //     // no data need to save
        // }
        // #endregion
    }
}
