using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletChargeUI : MonoBehaviour
{
    [SerializeField] private SnakeBullet _snakeBullet;
    [SerializeField] private TextMeshPro _chargeText;
    
    private void Start() 
    {
        _snakeBullet.ChargeDamage.HealthChangedEvent += SnakeBullet_OnHealthChanged;
        SnakeBullet_OnHealthChanged(_snakeBullet.ChargeDamage.GetHealthAmount());
    }
    private void OnDestroy() 
    {
        _snakeBullet.ChargeDamage.HealthChangedEvent -= SnakeBullet_OnHealthChanged;
    }

    private void SnakeBullet_OnHealthChanged(int currentHealth)
    {
        _chargeText.text = currentHealth.ToString();
    }

}
