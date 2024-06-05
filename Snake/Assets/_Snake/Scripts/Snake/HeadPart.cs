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
        if(_snake.IsPause)  return;

        base.HitByLaser(damageAmount);
        Flash.StartFlash();
        _snake.TakeDamage(damageAmount);
    }

    public override void HitByBullet(int damageAmount)
    {
        if(IsAimimg)    return;
        if(_snake.IsPause) return;
        
        base.HitByBullet(damageAmount,true);
        //base.HitByBullet(damageAmount);
        Flash.StartFlash();
        _snake.CreateBody();
    }
    #endregion

    public override void SetAiming()
    {
        base.SetAiming();

        if(_sprite != null)
        {
            _sprite.enabled = false;
        }
        IsAimimg = true;    
    }

    public override void SetNotAim()
    {
        base.SetNotAim();

        if(_sprite != null)
        {
            _sprite.enabled = true;
        }
        IsAimimg = false;      
    }
}
