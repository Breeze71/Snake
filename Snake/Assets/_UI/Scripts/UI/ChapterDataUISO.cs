using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V.UI
{
    /// <summary>
    /// 用於存儲 每個場景名稱，示意圖片，
    /// </summary>
    // [CreateAssetMenu(fileName = "ChapterDataUISO", menuName = "UI")]
    public class ChapterDataUISO : ScriptableObject
    {
        /// <summary>
        /// ex: Chapter 3-1 ???
        /// </summary>
        public string[] chapterWholeNames;
        public Sprite[] chapterSprites;
    }
}
