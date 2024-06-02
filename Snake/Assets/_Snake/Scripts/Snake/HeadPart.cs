using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPart : SnakePart
{
    private void Awake() 
    {
        _snake = GetComponentInParent<SnakeManager>();    
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if((_partSO.ObstacleMask.value & (1 << other.gameObject.layer)) > 0)
        {
            _snake.MoveNegative();
        }
    }

    public override void HitByLaser(int damageAmount)
    {
        // minus health
    }

    public override void HitByBullet(int damageAmount)
    {
        
    }
}
