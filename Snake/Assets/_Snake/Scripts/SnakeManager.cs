using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private SnakeSO _snakeSO;

    [SerializeField] List<GameObject> _bodyPrefabs = new List<GameObject>();

    private List<SnakePart> _snakePart = new List<SnakePart>();
    private float _distanceCount;

    private Rigidbody2D _headRB;
    private float currentInputAngle;

    #region LC
    private void Start() 
    {
        GameObject head = Instantiate(_bodyPrefabs[0], transform.position, transform.rotation, this.transform);

        _snakePart.Add(head.GetComponent<SnakePart>());  
        _bodyPrefabs.RemoveAt(0);

        _headRB = _snakePart[0].GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(_snakePart.Count > 0 && _snakePart.Count < 5)
        {
            CreateBody();
        }

        MoveSnake();    
        RotateSnake();
    }

    private void Update() 
    {
        FollowHead();
    }
    #endregion

    #region Head
    private void MoveSnake()
    {
        _headRB.velocity = _snakePart[0].transform.right * _snakeSO.Speed * Time.deltaTime;
    }
    private void RotateSnake()
    {
        // float horizontalInput = Input.GetAxisRaw("Horizontal");
        // if(horizontalInput != 0)
        // {
        //     _snakePart[0].transform.Rotate(new Vector3(0, 0, - _snakeSO.RotationSpeed * Time.deltaTime * horizontalInput));
        // }
        
        if(_snakeSO.HandleMoveDirection() != Vector2.zero)
        {
            currentInputAngle = _snakeSO.GetAngleFromVector(_snakeSO.HandleMoveDirection());
        }
        _snakePart[0].transform.eulerAngles = new Vector3(0, 0, currentInputAngle);
    }
    #endregion

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

        _snakePart[_snakePart.Count - 1].ClearHistoryInfo(); // remove final part history info
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
            GameObject body = Instantiate(_bodyPrefabs[0], snakePart.SnakeParts[0].Position, snakePart.SnakeParts[0].Rotation, this.transform);  

            _snakePart.Add(body.GetComponent<SnakePart>());
            // _bodyPrefabs.RemoveAt(0);

            body.GetComponent<SnakePart>().ClearHistoryInfo();

            _distanceCount = 0f;
        }
    }
}
