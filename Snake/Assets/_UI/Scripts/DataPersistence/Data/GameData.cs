using System;
using UnityEngine;
using V.Tool;

namespace V.Tool.SaveLoadSystem
{
    /// <summary>
    /// 需存儲的 Game Data
    /// </summary>
    public class GameData
    {
        /// <summary>
        /// 紀錄最近更新時間
        /// </summary>
        public long lastUpdate;

        public Scene CurrentScene;

        /// <summary>
        /// The game start with no data to load (沒存檔時)
        /// </summary>
        public GameData()
        {
            // CurrentScene = Scene.ForeWord;
            Debug.Log("fix");
            // coinsCollectedDic = new SerializableDictionary<string, bool>(); 
        }

        /// <summary>
        /// 根據場景評價完成度
        /// </summary>
        public int GetPercentageComplete()
        {
            int _sceneAmount = Enum.GetValues(typeof(Scene)).Length; // 場景總數
            int _currentSceneCount = (int)CurrentScene;

            // 佔比
            int _percentageCompleted = -1;
            if(_currentSceneCount != 0)
            {
                _percentageCompleted = _currentSceneCount * 100 / _sceneAmount;
            }

            return _percentageCompleted;
        }
        
        /* SerializableDictionary 特定位置金幣收集
        /// <summary>
        /// 特定位置金幣收集
        /// </summary>
        public SerializableDictionary<string, bool> coinsCollectedDic;
        /// <summary>
        /// 當前完成度
        /// </summary>
        public int GetPercentageComplete()
        {
            /*
            int _totalCollectedCoin = 0;
            foreach(bool _collected in coinsCollectedDic.Values)
            {
                if(_collected)
                {
                    _totalCollectedCoin++;
                }
            }

            // 佔比
            int _percentageCompleted = -1;
            if(coinsCollectedDic.Count != 0)
            {
                _percentageCompleted = (_totalCollectedCoin * 100 / coinsCollectedDic.Count);
            }
            return _percentageCompleted;
            
        

        }
        */
    }
}
