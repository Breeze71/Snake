using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : SnakePart
{
    public override void HitByLaser(int damageAmount)
    {
        _snake.DestroyBodyAndAfter(partIndex);
    }

    public override void HitByBullet(int damageAmount)
    {
        _snake.DestroyLastBody();
    }
}

