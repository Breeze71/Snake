using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V;

public class HeadPart : SnakePart
{
    [SerializeField] private GameObject _bulletPrefabs;
    [SerializeField] private GameObject _indicator;
    
    private Vector2 _aimDirection;
    private Coroutine _aimHoldCoroutine;
    private int _currentBulletDamage = 0;

    #region LC
    private void Awake() 
    {
        _snake = GetComponentInParent<SnakeManager>();    
    }

    private void OnEnable() 
    {
        InputManager.Instance.AimEvent += InputManager_OnAim;
        InputManager.Instance.AimCanceledEvent += InputManager_OnAimCanceled;
        InputManager.Instance.AimDirectionEvent += InputManager_OnAimDirection;
    }

    private void OnDisable() 
    {
        InputManager.Instance.AimEvent -= InputManager_OnAim;
        InputManager.Instance.AimDirectionEvent -= InputManager_OnAimDirection;
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

    private void InputManager_OnAimDirection(Vector2 aimDir)
    {

    }

    private void InputManager_OnAim()
    {
        if(_aimHoldCoroutine == null)
        {
            _aimHoldCoroutine = StartCoroutine(Coroutine_AimHold());
        }        
        else
        {
            StopCoroutine(_aimHoldCoroutine);
            _aimHoldCoroutine = StartCoroutine(Coroutine_AimHold());            
        }
    }

    private void InputManager_OnAimCanceled()
    {
        StopCoroutine(_aimHoldCoroutine);

        // shot
    }

    private IEnumerator Coroutine_AimHold()
    {
        while(true)
        {
            if(_snake.GetCurrentBodyCount() <= 2)   break;

            yield return new WaitForSeconds(_partSO.BulletChargingTime);
            _snake.DestroyLastBody();
            _currentBulletDamage++;
        }
    }

    #region Take Damage
    public override void HitByLaser(int damageAmount)
    {
        _snake.TakeDamage(damageAmount);
    }

    public override void HitByBullet(int damageAmount)
    {
        
    }
    #endregion

}
