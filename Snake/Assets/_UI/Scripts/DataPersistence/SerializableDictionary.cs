using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V.Tool.SaveLoadSystem
{
    /// <summary>
    /// Json 無法直接處理複雜 data，將其分為兩個　List 儲存
    /// </summary>
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keyList = new List<TKey>();
        [SerializeField] private List<TValue> valueList = new List<TValue>();

        /// <summary>
        /// Save the Dictionary to List
        /// </summary>
        public void OnBeforeSerialize()
        {
            // 存檔前先清除
            keyList.Clear();
            valueList.Clear();

            // 將實例化傳入的　Key, Value 傳入　List
            foreach(KeyValuePair<TKey, TValue> _pair in this)
            {
                keyList.Add(_pair.Key);
                valueList.Add(_pair.Value);
            }
        }

        /// <summary>
        /// Load the Dictionary from List
        /// </summary>
        public void OnAfterDeserialize()
        {
            // 讀檔時先清除
            this.Clear();

            if(keyList.Count != valueList.Count)
            {
                Debug.LogError("DeSerizlize a Serlizable Dictionary, but the amount of keys: "
                    + keyList.Count + " /=  values: " + valueList);
            }

            for(int i = 0; i < keyList.Count; i++)
            {
                this.Add(keyList[i], valueList[i]);
            }
        }

    }
}
