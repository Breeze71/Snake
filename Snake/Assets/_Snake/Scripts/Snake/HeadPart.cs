using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V;

public class HeadPart : SnakePart
{
    [SerializeField] private Rigidbody2D _rb;

    #region LC
    private void Awake() 
    {
        _snake = GetComponentInParent<SnakeManager>();    
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if((_partSO.ObstacleMask.value & (1 << other.gameObject.layer)) > 0)
        {
            _snake.MoveNegative();
            _snake.TakeDamage(_partSO.obstacleDamage);
        }
    }
    
    #region Take Damage
    public override void HitByLaser(int damageAmount)
    {
        _snake.TakeDamage(damageAmount);
    }

    public override void HitByBullet(int damageAmount)
    {
        if(IsAimimg)    return;

        _snake.CreateBody();
    }
    #endregion

    public override void SetAiming()
    {
        base.SetAiming();

                GetComponentInChildren<SpriteRenderer>().enabled = false;
        IsAimimg = true;    
    }

    public override void SetNotAim()
    {
        base.SetNotAim();

                GetComponentInChildren<SpriteRenderer>().enabled = true;
        IsAimimg = false;      
    }
}
