using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V.Tool.SaveLoadSystem
{
    public class TestBackToMenu : MonoBehaviour
    {
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Backspace))
            {
                // Loader.LoadScene(Loader.Scene.TestMenuScene);
                Debug.Log("loader");
            }
        }
    }
}
