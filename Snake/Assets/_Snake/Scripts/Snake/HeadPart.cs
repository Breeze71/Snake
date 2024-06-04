using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V;

public class HeadPart : SnakePart
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SnakeBullet _bulletPrefabs;
    [SerializeField] private GameObject _indicator;
    [SerializeField] private Transform _firePoint;
    
    private SnakeBullet _currentBullet;
    private Vector2 _aimDirection;
    private Coroutine _aimHoldCoroutine;
    private bool _isAiming = false;

    #region LC
    private void Awake() 
    {
        _snake = GetComponentInParent<SnakeManager>();    
        _indicator.SetActive(false);
    }

    private void OnEnable() 
    {
        InputManager.Instance.AimEvent += InputManager_OnAim;
        InputManager.Instance.AimCanceledEvent += InputManager_OnAimCanceled;
        InputManager.Instance.AimDirectionEvent += InputManager_OnAimDirection;
    }

    private void Update() 
    {
        _indicator.transform.rotation = Quaternion.Euler(0f, 0f, GetAngleFromVector(_aimDirection));
        _indicator.transform.position = transform.position;   

        if(_currentBullet != null)
        {
            _currentBullet.transform.position = _firePoint.position; 
        } 
    }

    private void OnDisable() 
    {
        InputManager.Instance.AimEvent -= InputManager_OnAim;
        InputManager.Instance.AimDirectionEvent -= InputManager_OnAimDirection;
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D other) {
        if((_partSO.ObstacleMask.value & (1 << other.gameObject.layer)) > 0)
        {
            _snake.MoveNegative();
            Debug.Log(other.gameObject.name);
            _snake.TakeDamage(_partSO.obstacleDamage);
        }
    }
    

    private void InputManager_OnAimDirection(Vector2 aimDir)
    {
        _aimDirection = aimDir;
        _aimDirection = Camera.main.ScreenToWorldPoint((Vector3)_aimDirection) - transform.position;
    }

    private void InputManager_OnAim()
    {
        if(_snake.GetCurrentBodyCount() <= 2)   return;
        Debug.Log("BBB");
        _isAiming = true;
        _indicator.SetActive(true);
        
        if(_aimHoldCoroutine == null)
        {
            _aimHoldCoroutine = StartCoroutine(Coroutine_AimHold());
        }        
        else
        {
            StopCoroutine(_aimHoldCoroutine);
            _aimHoldCoroutine = StartCoroutine(Coroutine_AimHold());            
        }
        Debug.Log("AAAA");

        _aimDirection = _rb.velocity;

        _currentBullet = Instantiate(_bulletPrefabs, _firePoint);

        _snake.ChangeCurrentSpeed(_snake._snakeSO.AimDecreaseSpeed);
        _snake._canSpeedChange = false;
    }

    private void InputManager_OnAimCanceled()
    {
        if(_aimHoldCoroutine != null)
        {
            StopCoroutine(_aimHoldCoroutine);
        }

        _indicator.SetActive(false);

        if(!_isAiming)  return;
        _isAiming = false;
        if(_currentBullet == null)  return;

        // shot
        _currentBullet.transform.SetParent(null);
        _currentBullet.Shoot(_aimDirection);
        _currentBullet = null;
        
        _snake.ChangeCurrentSpeed(_snake._snakeSO.Speed);
        _snake._canSpeedChange = true;
    }

    private IEnumerator Coroutine_AimHold()
    {
        _snake.DestroyLastBody();
        while(true)
        {
            if(_snake.GetCurrentBodyCount() <= 2)   break;
            yield return new WaitForSeconds(_partSO.BulletChargingTime);

            _currentBullet.ChargeDamage.HealNoLimit(1);
            _snake.DestroyLastBody();
        }
    }

    #region Take Damage
    public override void HitByLaser(int damageAmount)
    {
        _snake.TakeDamage(damageAmount);
    }

    public override void HitByBullet(int damageAmount)
    {
        _snake.CreateBody();
    }
    #endregion

    public float GetAngleFromVector(Vector2 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if(n < 0)
        {
            n += 360f;
        }

        return n;
    }

}
