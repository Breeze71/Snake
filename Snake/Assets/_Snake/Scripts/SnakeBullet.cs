using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    public int ChargeDamage = 0;
    public Rigidbody2D Rb;

    public void Shoot(Vector2 dir)
    {
        Rb.AddForce(dir * bulletSpeed, ForceMode2D.Force);
    }
}
