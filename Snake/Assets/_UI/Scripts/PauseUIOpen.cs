using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V;
using V.UI;

public class PauseUIOpen : MonoBehaviour
{
    void Start()
    {
        InputManager.Instance.PauseEvent += InputManager_PauseEvent;
    }

    private void InputManager_PauseEvent()
    {
        UIManager.Instance.ShowUI<PauseUI>("PauseUI");
        InputManager.Instance.SetActionMap(InputType.UI);
    }

    private void OnDestroy() 
    {
        InputManager.Instance.PauseEvent -= InputManager_PauseEvent;
    }
}
