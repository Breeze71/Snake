using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V;

public class CorePart : SnakePart
{
    [SerializeField] private SnakeBullet _bulletPrefabs;
    [SerializeField] private Transform _firePoint;
    private SnakeBullet _currentBullet;
    private Vector2 _aimDirection;
    private Coroutine _aimHoldCoroutine;
    private bool _isAiming = false;
    
    protected override void OnEnable() 
    {
        base.OnEnable();

        InputManager.Instance.AimEvent += InputManager_OnAim;
        InputManager.Instance.AimCanceledEvent += InputManager_OnAimCanceled;
        InputManager.Instance.AimDirectionEvent += InputManager_OnAimDirection;
    }

    private void Update() 
    {
        Indicator.transform.rotation = Quaternion.Euler(0f, 0f, GetAngleFromVector(_aimDirection));
        Indicator.transform.position = transform.position;   

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
        Indicator.SetActive(true);
        _snake.SetBodyDisable();
        
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

        _aimDirection = _snake.HeadRB.velocity;

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

        if(Indicator != null)
        {
            Indicator.SetActive(false);
        }
        _snake.SetBodyEnable();

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

    public float GetAngleFromVector(Vector2 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if(n < 0)
        {
            n += 360f;
        }

        return n;
    }

    public override void HitByLaser(int damageAmount)
    {
        // minus health
        if(_snake.IsPause) return;

        base.HitByLaser(damageAmount);
        _snake.TakeDamage(damageAmount);
    }

    public override void HitByBullet(int damageAmount)
    {
        if(_snake.IsPause) return;

        base.HitByBullet(damageAmount);
        _snake.TakeDamage(damageAmount);
    }
}

