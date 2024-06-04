using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlock : MonoBehaviour
{
    public HealthSystem Health {get; private set;}
    
    [SerializeField] private int _healthMax = 3;

    private void Awake() 
    {
        Health = new HealthSystem(_healthMax);
    }

    private void Start() 
    {
        Health.HealthChangedEvent += Health_OnHealthChanged;
        
    }

    private void OnDestroy() 
    {
        Health.HealthChangedEvent -= Health_OnHealthChanged;
    }

    private void Health_OnHealthChanged(int obj)
    {
        if(obj == 0)
        {
            Destroy(gameObject);
        }
    }
}
