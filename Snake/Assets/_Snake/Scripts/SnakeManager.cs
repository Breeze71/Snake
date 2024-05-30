using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private SnakeSO _snakeSO;

    [SerializeField] List<GameObject> _bodyPrefabs = new List<GameObject>();

    private List<GameObject> _snakePart = new List<GameObject>();
    private float _distanceCount;

    private Rigidbody2D _headRB;
    private float currentInputAngle;

    private void Start() 
    {
        GameObject head = Instantiate(_bodyPrefabs[0], transform.position, transform.rotation, this.transform);  

        if(head.GetComponent<SnakePart>() == null)
        {
            head.AddComponent<SnakePart>();
        }
        if(head.GetComponent<Rigidbody2D>() == null)
        {
            head.AddComponent<Rigidbody2D>();
            head.GetComponent<Rigidbody2D>().gravityScale = 0;
        }

        _snakePart.Add(head);  
        _bodyPrefabs.RemoveAt(0);

        _headRB = _snakePart[0].GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(_snakePart.Count > 0)
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

    private void MoveSnake()
    {
        _headRB.velocity = _snakePart[0].transform.right * _snakeSO.Speed * Time.deltaTime;
    }

    private void FollowHead()
    {
        if(_snakePart.Count > 1)
        {
            for(int i = 1; i < _snakePart.Count; i++)
            {
                SnakePart snakePart = _snakePart[i - 1].GetComponent<SnakePart>();
                _snakePart[i].transform.position = snakePart.SnakeParts[0].Position;
                _snakePart[i].transform.rotation = snakePart.SnakeParts[0].Rotation;

                snakePart.SnakeParts.RemoveAt(0);
            }
        }
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

    private void CreateBody()
    {
        SnakePart snakePart = _snakePart[_snakePart.Count - 1].GetComponent<SnakePart>();

        if(_distanceCount == 0)
        {
            snakePart.ClearPartList();
        }

        _distanceCount += Time.deltaTime;

        if(_distanceCount >= _snakeSO.Distance)
        {
            GameObject body = Instantiate(_bodyPrefabs[0], snakePart.SnakeParts[0].Position, snakePart.SnakeParts[0].Rotation, this.transform);  

            if(body.GetComponent<SnakePart>() == null)
            {
                body.AddComponent<SnakePart>();
            }
            if(body.GetComponent<Rigidbody2D>() == null)
            {
                body.AddComponent<Rigidbody2D>();
                body.GetComponent<Rigidbody2D>().gravityScale = 0;
            }

            _snakePart.Add(body);
            // _bodyPrefabs.RemoveAt(0);

            body.GetComponent<SnakePart>().ClearPartList();

            _distanceCount = 0f;
        }
    }
}
