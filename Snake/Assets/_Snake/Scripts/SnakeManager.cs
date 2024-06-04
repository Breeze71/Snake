using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

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
    private Rigidbody2D _headRB;
    private float _currentInputAngle;
    private float _currentSpeed;
    private bool _canMoveInput = true;
    private Coroutine _disableInputCoroutine;

    // health
    public HealthSystem SnakeHealth {get; private set;}
    private bool _isInvincible = false;
    private Coroutine _invincibleCoroutine;


    #region LC
    private void Awake() 
    {
        SnakeHealth = new HealthSystem(_snakeSO.HealthAmount);
    }
    private void Start() 
    {
        SpawnSnake();

        _headRB = _headPart.GetComponent<Rigidbody2D>();

        _canMoveInput = true;
        _isInvincible = false;
        _canSpeedChange = true;
        _currentSpeed = _snakeSO.Speed;
    }

    private void FixedUpdate()
    {
        MoveSnake();    
        RotateSnake();
        FollowHead();
    }

    private void Update() 
    {
        if(_spawnAmount > 0)
        {
            CreateSnakePart();
        }

        HandleAcclerate();
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
    private void MoveSnake()
    {
        _headRB.velocity = _snakePart[0].transform.right * _currentSpeed;
    }
    private void RotateSnake()
    {
        // float horizontalInput = Input.GetAxisRaw("Horizontal");
        // if(horizontalInput != 0)
        // {
        //     _snakePart[0].transform.Rotate(new Vector3(0, 0, - _snakeSO.RotationSpeed * Time.deltaTime * horizontalInput));
        // }
        if(!_canMoveInput)    return;
        
        if(_snakeSO.HandleMoveDirection() != Vector2.zero)
        {
            _currentInputAngle = _snakeSO.GetAngleFromVector(_snakeSO.HandleMoveDirection());
        }
        _snakePart[0].transform.eulerAngles = new Vector3(0, 0, _currentInputAngle);
    }
    private void HandleAcclerate()
    {
        if(!_canMoveInput) return;
        if(!_canSpeedChange)    return;

        if(Input.GetKeyDown(_snakeSO.AcclerateKey))
        {
            _currentSpeed = _snakeSO.shiftSpeed;
        }
        else if(Input.GetKeyUp(_snakeSO.AcclerateKey))
        {
            _currentSpeed = _snakeSO.Speed;
        }
    }
    public void MoveNegative()
    {
        if(!_canMoveInput) return;

        StartDisableInput();

        _currentInputAngle = _currentInputAngle - 180f;
        
        _snakePart[0].transform.eulerAngles = new Vector3(0, 0, _currentInputAngle);
        //Debug.Log("aaaa");
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
        for(int i = _snakePart.Count - 1; i >= 2; i--)
        {
            _snakePart[i].gameObject.SetActive(true);
        }
    }

    public void SetBodyDisable()
    {
        for(int i = _snakePart.Count - 1; i >= 2; i--)
        {
            _snakePart[i].gameObject.SetActive(false);
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
    public void TakeDamage(int damage)
    {
        if(_isInvincible)   return;

        StartInvincible();
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
