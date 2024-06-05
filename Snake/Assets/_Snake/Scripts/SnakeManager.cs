using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using V;

public class SnakeManager : MonoBehaviour
{
    [Expandable]public SnakeSO _snakeSO;

    // body
    [SerializeField] private List<GameObject> _bodyPrefabs = new List<GameObject>();
    [ReadOnly] [SerializeField] private List<SnakePart> _snakePart = new List<SnakePart>();
    public int _spawnAmount = 1;
    [SerializeField] private SnakePart _headPart;
    private float _distanceCount = 0f;
    private bool isSpawnCore = true;

    // move
    [ReadOnly] public bool _canSpeedChange = true;
    public Rigidbody2D HeadRB;
    private Vector2 _inputVector;
    private float _currentMoveAngle;
    private float _currentSpeed;
    private bool _canMoveInput = true;
    private Coroutine _disableInputCoroutine;

    // health
    public HealthSystem SnakeHealth {get; private set;}
    private bool _isInvincible = false;
    private Coroutine _invincibleCoroutine;

    // 
    public GameObject Indicator;
    public bool IsPause;


    #region LC
    private void Awake() 
    {
        SnakeHealth = new HealthSystem(_snakeSO.HealthAmount);
    }
    private void Start() 
    {
        SpawnSnake();

        HeadRB = _headPart.GetComponent<Rigidbody2D>();

        _canMoveInput = true;
        _isInvincible = false;
        _canSpeedChange = true;
        _currentSpeed = _snakeSO.Speed;

        InputManager.Instance.MoveEvent += InputManager_OnMove;
        InputManager.Instance.AcclerateEvent += InputManager_OnAcclerate;
        InputManager.Instance.AcclerateCanceledEvent += InputManager_OnAcclerateCancel;
        InputManager.Instance.PauseEvent += Pasue;
        InputManager.Instance.ResumeEvent += Resume;
    }

    private void FixedUpdate()
    {
        MoveSnake();    
        RotateSnake();
        FollowHead();

        if(_spawnAmount > 0)
        {
            CreateSnakePart();
        }
    }

    private void OnDestroy() 
    {
        InputManager.Instance.MoveEvent -= InputManager_OnMove;
        InputManager.Instance.AcclerateEvent -= InputManager_OnAcclerate;
        InputManager.Instance.AcclerateCanceledEvent -= InputManager_OnAcclerateCancel;    
    }
    #endregion

    #region Init
    private void SpawnSnake()
    {
        _snakePart.Add(_headPart);
        _headPart.partIndex = 0;
    }
    #endregion

    #region Head Movement
    private void InputManager_OnAcclerateCancel()
    {
        if(!_canMoveInput) return;
        if(!_canSpeedChange)    return;
        _currentSpeed = _snakeSO.Speed;
    }

    private void InputManager_OnAcclerate()
    {
        if(!_canMoveInput) return;
        if(!_canSpeedChange)    return;
        _currentSpeed = _snakeSO.shiftSpeed;
    }

    private void InputManager_OnMove(Vector2 vector)
    {
        _inputVector = vector;
    }
    private void MoveSnake()
    {
        if(IsPause)
        {
            HeadRB.velocity = Vector2.zero;

            return;
        }

        HeadRB.velocity = _snakePart[0].transform.right * _currentSpeed;
    }
    private void RotateSnake()
    {
        // float horizontalInput = Input.GetAxisRaw("Horizontal");
        // if(horizontalInput != 0)
        // {
        //     _snakePart[0].transform.Rotate(new Vector3(0, 0, - _snakeSO.RotationSpeed * Time.deltaTime * horizontalInput));
        // }

        if(_canMoveInput && _inputVector != Vector2.zero)
        {
            _currentMoveAngle = _snakeSO.GetAngleFromVector(_inputVector);
        }
        
        _snakePart[0].transform.eulerAngles = new Vector3(0, 0, _currentMoveAngle);
    }
    public void MoveNegative()
    {
        if(!_canMoveInput) return;

        StartDisableInput();

        _currentMoveAngle = _currentMoveAngle - 180f;
        
        _snakePart[0].transform.eulerAngles = new Vector3(0, 0, _currentMoveAngle);
    }
    private void StartDisableInput()
    {
        if(_disableInputCoroutine != null)
        {
            StopCoroutine(_disableInputCoroutine);
            _disableInputCoroutine = StartCoroutine(Coroutine_DisableMovement());
            return;
        }
        _disableInputCoroutine = StartCoroutine(Coroutine_DisableMovement());
    }
    private IEnumerator Coroutine_DisableMovement()
    {
        _canMoveInput = false;
        yield return new WaitForSeconds(_snakeSO.disableInputTime);
        _canMoveInput = true;
    }
    public void ChangeCurrentSpeed(float speed)
    {
        _currentSpeed = speed;
    }
    #endregion

    #region Body
    private void FollowHead()
    {
        if(IsPause) return;

        if(_snakePart.Count > 1)
        {
            // act as last part history Transform then remove
            for(int i = 1; i < _snakePart.Count; i++)
            {
                SnakePart lastPart = _snakePart[i - 1];
                _snakePart[i].transform.position = lastPart.SnakeInfos[0].Position;
                _snakePart[i].transform.rotation = lastPart.SnakeInfos[0].Rotation;

                lastPart.SnakeInfos.RemoveAt(0);
            }
        }
    }

    private void CreateSnakePart()
    {
        if(IsPause) return;

        SnakePart snakePart = _snakePart[_snakePart.Count - 1];

        if(_distanceCount == 0)
        {
            snakePart.ClearHistoryInfo();
        }

        _distanceCount += Time.deltaTime;

        if(_distanceCount >= _snakeSO.Distance)
        {
            GameObject body = Instantiate(_bodyPrefabs[1], snakePart.SnakeInfos[0].Position, snakePart.SnakeInfos[0].Rotation, this.transform);  
            SnakePart bodyPart = body.GetComponent<SnakePart>();

            _snakePart.Add(bodyPart);
            bodyPart.partIndex = _snakePart.Count - 1; // currentIndex

            bodyPart.ClearHistoryInfo();

            _distanceCount = 0f;
            _spawnAmount--;

            // 只生成一次 core
            if(isSpawnCore)
            {
                bodyPart.Indicator = Indicator;
                _bodyPrefabs.RemoveAt(1);
                isSpawnCore = false;
            }
        }   

    }

    [Button]
    public void CreateBody()
    {
        _spawnAmount++;
    }
    public void DestroyBodyAndAfter(int partIndex)
    {
        if(_isInvincible)   return;

        for(int i = _snakePart.Count - 1; i >= partIndex ; i--)
        {
            Destroy(_snakePart[i].gameObject);
            _snakePart.RemoveAt(i);
        }
    }
    public void DestroyLastBody()
    {
        if(_snakePart.Count > 2)
        {
            int lastPartIndex = _snakePart.Count - 1;

            Destroy(_snakePart[lastPartIndex].gameObject);
            _snakePart.RemoveAt(lastPartIndex);
        }
    }

    public int GetCurrentBodyCount()
    {
        return _snakePart.Count;
    }

    public void SetBodyEnable()
    {
        _snakePart[0].SetNotAim();
        for(int i = _snakePart.Count - 1; i >= 2; i--)
        {
            _snakePart[i].SetNotAim();
        }
    }

    public void SetBodyDisable()
    {
        _snakePart[0].SetAiming();
        for(int i = _snakePart.Count - 1; i >= 2; i--)
        {
            _snakePart[i].SetAiming();
        }
    }

    [Button]
    private void TestDestroy()
    {
        for(int i = _snakePart.Count - 1; i >= 2 ; i--)
        {
            Destroy(_snakePart[i].gameObject);
            _snakePart.RemoveAt(i);
        }
    }
    #endregion

    #region Take Damage
    private void Resume()
    {
        IsPause = false;
    }

    private void Pasue()
    {
        IsPause = true;
    }

    public void TakeDamage(int damage)
    {
        if(_isInvincible)   return;
        if(IsPause)    return;

        if(damage > 0)
        {
            StartInvincible();
        }
        SnakeHealth.TakeDamage(damage);
    }

    private void StartInvincible()
    {
        if(_invincibleCoroutine != null)
        {
            StopCoroutine(_invincibleCoroutine);
            _invincibleCoroutine = StartCoroutine(Coroutine_Invincible());
            return;
        }
        _invincibleCoroutine = StartCoroutine(Coroutine_Invincible());
    }

    private IEnumerator Coroutine_Invincible()
    {
        //MoveNegative();
        _isInvincible = true;
        yield return new WaitForSeconds(_snakeSO.InvincibleTime);
        _isInvincible = false;
    }

    [Button]
    private void TestMinus2Health()
    {
        TakeDamage(2);
    }
    [Button]
    private void TestHeal2()
    {
        SnakeHealth.Heal(2);
    }
    #endregion

}
