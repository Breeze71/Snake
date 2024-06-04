using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SnakeManager _snake;
    [SerializeField] private GameObject _blocksParents;
    [ReadOnly] [SerializeField] private List<BossBlock> _bossBlocks;

    [ReadOnly] [SerializeField] private int currentBossHealth = 0;

    private void Awake() 
    {
        _bossBlocks = _blocksParents.GetComponentsInChildren<BossBlock>().ToList<BossBlock>();    
    }

    private void Start()
    {
        _snake.SnakeHealth.HealthChangedEvent += Snake_OnHealthChanged;
        foreach(BossBlock bossHealth in _bossBlocks)
        {
            bossHealth.Health.HealthChangedEvent += BossHealth_OnHealthChanged;

            currentBossHealth += bossHealth.Health.GetHealthAmount();
        }
    }

    private void OnDestroy()
    {
        _snake.SnakeHealth.HealthChangedEvent -= Snake_OnHealthChanged;
        foreach(BossBlock bossHealth in _bossBlocks)
        {
            bossHealth.Health.HealthChangedEvent -= BossHealth_OnHealthChanged;
        }
    }

    private void Snake_OnHealthChanged(int currentHealth)
    {
        if(currentHealth == 0)
        {
            Debug.Log("game over");
        }
    }

    private void BossHealth_OnHealthChanged(int obj)
    {
        currentBossHealth--;

        Debug.Log("current boss health" + currentBossHealth);
        // wave
    }
}
