using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SnakeManager _snake;

    private void Start() 
    {
        _snake.SnakeHealth.HealthChangedEvent += Snake_OnHealthChanged;
    }

    private void Snake_OnHealthChanged(int currentHealth)
    {
        if(currentHealth == 0)
        {
            Debug.Log("game over");
        }
    }
}
