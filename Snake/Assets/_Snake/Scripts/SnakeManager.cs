using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private SnakeSO _snakeSO;

    [SerializeField] private SnakePart _headPart;
    [SerializeField] List<GameObject> _bodyPrefabs = new List<GameObject>();

    private List<SnakePart> _snakePart = new List<SnakePart>();
    private float _distanceCount = 0f;

    private Rigidbody2D _headRB;
    [ReadOnly] [SerializeField] private float _currentInputAngle;
    private float _currentSpeed;
    private bool _canMoveInput = true;
    private Coroutine _disableInputCoroutine;


    #region LC
    private void Start() 
    {
        _snakePart.Add(_headPart);  

        _headRB = _headPart.GetComponent<Rigidbody2D>();

        _canMoveInput = true;
    }

    private void FixedUpdate()
    {
        MoveSnake();    
        RotateSnake();
        FollowHead();
    }

    private void Update() 
    {
        if(_snakePart.Count > 0 && _snakePart.Count < 5)
        {
            CreateBody();
        }

        HandleAcclerate();
    }
    #endregion

    #region Head
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
        if(Input.GetKey(_snakeSO.AcclerateKey))
        {
            _currentSpeed = _snakeSO.shiftSpeed;
        }
        else
        {
            _currentSpeed = _snakeSO.Speed;
        }
    }
    public void MoveNegative()
    {
        DisableInput();
        
        _currentInputAngle = _currentInputAngle - 180f;
        
        _snakePart[0].transform.eulerAngles = new Vector3(0, 0, _currentInputAngle);
    }
    private void DisableInput()
    {
        if(_disableInputCoroutine != null)
        {
            _disableInputCoroutine = null;
            _disableInputCoroutine = StartCoroutine(Coroutine_DisableMovement());
        }
        _disableInputCoroutine = StartCoroutine(Coroutine_DisableMovement());
    }
    private IEnumerator Coroutine_DisableMovement()
    {
        _canMoveInput = false;
        yield return new WaitForSeconds(_snakeSO.disableInputTime);
        _canMoveInput = true;
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
                _snakePart[i].transform.position = lastPart.SnakeParts[0].Position;
                _snakePart[i].transform.rotation = lastPart.SnakeParts[0].Rotation;

                lastPart.SnakeParts.RemoveAt(0);
            }
        }
    }

    private void CreateBody()
    {
        SnakePart snakePart = _snakePart[_snakePart.Count - 1].GetComponent<SnakePart>();

        if(_distanceCount == 0)
        {
            snakePart.ClearHistoryInfo();
        }

        _distanceCount += Time.deltaTime;

        if(_distanceCount >= _snakeSO.Distance)
        {
            GameObject body = Instantiate(_bodyPrefabs[1], snakePart.SnakeParts[0].Position, snakePart.SnakeParts[0].Rotation, this.transform);  

            _snakePart.Add(body.GetComponent<SnakePart>());
            // _bodyPrefabs.RemoveAt(0);

            body.GetComponent<SnakePart>().ClearHistoryInfo();

            _distanceCount = 0f;
        }
    }
    #endregion
}
