using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBullet : MonoBehaviour
{
    [SerializeField] private float _minStableSpeed;
    [SerializeField] private LayerMask _bossLayer;

    public HealthSystem ChargeDamage;
    private Rigidbody2D _rb;
    private Collider2D _coll;

    private Vector2 moveDiretion = Vector2.zero;

    private void Awake() 
    {
        ChargeDamage = new HealthSystem(1);
    }

    private void OnEnable() 
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
    }

    private void Start() 
    {
        _coll.enabled = false;
        ChargeDamage.HealthChangedEvent += ChargeDamage_OnHealthChanged;
    }

    private void FixedUpdate() 
    {
        if(moveDiretion == Vector2.zero)    return;

        // if current velocity less then minStable Speed
        if(_rb.velocity.magnitude < _minStableSpeed * 0.9f)
        {
            _rb.AddForce(moveDiretion * _minStableSpeed, ForceMode2D.Impulse);
        }

        // if current velocity more than minStable speed
        else if(_rb.velocity.magnitude > _minStableSpeed * 1.1f)
        {
            Vector2 _currentDir = _rb.velocity.normalized;

            _rb.velocity = new Vector2(_currentDir.x, _currentDir.y) * _minStableSpeed;         
        }            
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if((_bossLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            BossBlock currentHitBlcok = other.gameObject.GetComponent<BossBlock>();
            if(currentHitBlcok != null)
            {
                currentHitBlcok.Health.TakeDamage(1);
                ChargeDamage.TakeDamage(1);
            }
        }        
    }

    private void OnDestroy() 
    {
        ChargeDamage.HealthChangedEvent -= ChargeDamage_OnHealthChanged;        
    }

    private void ChargeDamage_OnHealthChanged(int obj)
    {
        if(obj == 0)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector2 dir)
    {
        _coll.enabled = true;
        moveDiretion = dir;
    }
}
