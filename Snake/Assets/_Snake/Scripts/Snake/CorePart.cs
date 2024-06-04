using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorePart : SnakePart
{
    public override void HitByLaser(int damageAmount)
    {
        // minus health
        _snake.TakeDamage(damageAmount);
    }

    public override void HitByBullet(int damageAmount)
    {
        _snake.TakeDamage(damageAmount);
    }
}

