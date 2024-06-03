using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletChargeUI : MonoBehaviour
{
    [SerializeField] private SnakeBullet snakeBullet;
    [SerializeField] private TextMeshPro chargeText;

    private void OnEnable() 
    {
        // chargeText = GetComponent<TextMeshPro>();
    }
    private void Update() 
    {
        chargeText.text = snakeBullet.ChargeDamage.ToString();
    }

}
