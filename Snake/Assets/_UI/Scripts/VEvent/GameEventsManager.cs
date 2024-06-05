using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V.Tool.SaveLoadSystem;

namespace V.Event
{
    /// <summary>
    /// 管理遊戲中僅需實例化一個的事件，例如存讀檔，玩家狀態 etc
    /// </summary>    
    public class GameEventsManager : MonoBehaviour
    {
        public static GameEventsManager Instance { get; private set; }

        /// <summary>
        /// 玩家狀態 Event
        /// </summary>
        public PlayerEvent PlayerEvent;
        public CoinEvent CoinEvent;

        private void Awake() 
        {
            if(Instance != null)
            {
                Debug.LogError("More than One GameEvent Manager");
            }    
            Instance = this;

            PlayerEvent = new PlayerEvent();
            CoinEvent = new CoinEvent();
        }
    }
}
