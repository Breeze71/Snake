using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private SnakeSO _snakeSO;

    [ShowAssetPreview]
    [SerializeField] List<GameObject> bodyPrefabs = new List<GameObject>();

    private List<GameObject> snakePart = new List<GameObject>();

    private void Start() 
    {
        GameObject head = Instantiate(bodyPrefabs[0], transform.position, transform.rotation, this.transform);  

        if(head.GetComponent<SnakePart>() == null)
        {
            head.AddComponent<SnakePart>();
        }
        if(head.GetComponent<Rigidbody2D>() == null)
        {
            head.AddComponent<Rigidbody2D>();
            head.GetComponent<Rigidbody2D>().gravityScale = 0;
        }

        snakePart.Add(head);  
    }

    private void FixedUpdate()
    {
        MoveSnake();    
        RotateSnake();
    }

    private void MoveSnake()
    {
        Rigidbody2D headRB = snakePart[0].GetComponent<Rigidbody2D>();

        headRB.velocity = snakePart[0].transform.right * _snakeSO.Speed * Time.deltaTime;
    }

    private void RotateSnake()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if(horizontalInput != 0)
        {
            snakePart[0].transform.Rotate(new Vector3(0, 0, - _snakeSO.RotationSpeed * Time.deltaTime * horizontalInput));
        }
    }
}
