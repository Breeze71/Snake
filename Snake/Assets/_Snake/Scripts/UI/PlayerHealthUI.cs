using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> _healthBlocks;
    [SerializeField] private SnakeManager _snake;

    private void Start() 
    {
        _snake.SnakeHealth.HealthChangedEvent += SnakeHealth_OnHealthChanged;
    }

    private void SnakeHealth_OnHealthChanged(int currentHealth)
    {
        foreach(GameObject healthBlock in _healthBlocks)
        {
            healthBlock.SetActive(false);
        }

        for(int i = 0; i < currentHealth; i++)
        {
            _healthBlocks[i].SetActive(true);
        }
    }
}
