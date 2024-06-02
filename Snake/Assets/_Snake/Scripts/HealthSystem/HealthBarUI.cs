using System;
using System.Collections;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Transform _barPosition;
    private HealthSystem _healthSystem;

    public void SetupHealthSystemUI(HealthSystem _healthSystem)
    {
        this._healthSystem = _healthSystem;

        _healthSystem.HealthChangedEvent += healthSystem_OnHealthChanged;
    }

    private void healthSystem_OnHealthChanged()
    {
        _barPosition.transform.localScale = new Vector3(_healthSystem.GetHealthPercent(), 1, 1);  
    }
}
