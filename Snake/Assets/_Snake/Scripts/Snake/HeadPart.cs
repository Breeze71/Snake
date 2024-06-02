using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPart : SnakePart
{
    [SerializeField] private SnakeManager _snake;

    private void Awake() 
    {
        _snake = GetComponentInParent<SnakeManager>();    
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if((_partSO.ObstacleMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log("Hit Obstacle");
            _snake.MoveNegative();
        }
    }
}
