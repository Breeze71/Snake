using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V.Event;

namespace V.Tool.SaveLoadSystem
{
    public class CoinInteraction //: InteractableBase, IDataPersistable
    {
    //     [SerializeField] private string id;
    //     [SerializeField] private GameObject visual;

    //     private bool isCollected = false;

    //     /// <summary>
    //     /// 生成一個特定的 id
    //     /// </summary>
    //     [ContextMenu("Generate Guid For Id")]
    //     private void GenerateGuid()
    //     {
    //         id = System.Guid.NewGuid().ToString();
    //     }

    //     #region InteractableBase
    //     public override void EnterTrigger(Collider2D _other)
    //     {
    //         if(isCollected)
    //         {
    //             return;
    //         }

    //         GameEventsManager.Instance.CoinEvent.CoinCollectedEvent();

    //         visual.SetActive(false);
    //         isCollected = true;
    //     }

    //     public override void ExitTrigger(Collider2D _other)
    //     {
            
    //     }

    //     public override void Interact()
    //     {
            
    //     }
    //     #endregion

    //     #region IDataPersistable
    //     public void LoadData(GameData _gameData)
    //     {
    //         _gameData.coinsCollectedDic.TryGetValue(id, out isCollected);
            
    //         if(isCollected)
    //         {
    //             visual.SetActive(false);
    //         }
    //     }

    //     public void SaveData(GameData _gameData)
    //     {
    //         // 確保沒重複
    //         if(_gameData.coinsCollectedDic.ContainsKey(id))
    //         {
    //             _gameData.coinsCollectedDic.Remove(id);
    //         }

    //         // 儲存目前　coin 是否被撿過 
    //         _gameData.coinsCollectedDic.Add(id, isCollected);
    //     }
    //     #endregion
    }
}
