using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V.Tool.SaveLoadSystem
{
    public class CoinEvent
    {
        public event Action OnCoinCollected;
        public void CoinCollectedEvent() 
        {
            if (OnCoinCollected != null) 
            {
                OnCoinCollected();
            }
        }
    }
}
