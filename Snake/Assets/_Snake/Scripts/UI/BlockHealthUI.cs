using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockHealthUI : MonoBehaviour
{
    [SerializeField] private BossBlock _bossBlock;
    [SerializeField] private TextMeshPro _healthText;

    private void Start() 
    {
        _bossBlock.Health.HealthChangedEvent += BossBlock_OnHealthChanged;
        BossBlock_OnHealthChanged(_bossBlock.Health.GetHealthAmount());
    }
    private void OnDestroy() 
    {
        _bossBlock.Health.HealthChangedEvent -= BossBlock_OnHealthChanged;
    }

    private void BossBlock_OnHealthChanged(int obj)
    {
        _healthText.text = obj.ToString();
    }
}
