using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : SnakePart
{
    public override void HitByLaser(int damageAmount)
    {
        if(IsAimimg)   return;
        if(_snake.IsPause) return;

        _snake.DestroyBodyAndAfter(partIndex);
    }

    public override void HitByBullet(int damageAmount)
    {
        if(IsAimimg)   return;
        if(_snake.IsPause) return;
        
        _snake.DestroyLastBody();
    }

    public override void SetAiming()
    {
        base.SetAiming();
        
                GetComponentInChildren<SpriteRenderer>().enabled = false;
        IsAimimg = true;    
    }

    public override void SetNotAim()
    {
        base.SetNotAim();

                GetComponentInChildren<SpriteRenderer>().enabled = true;
        IsAimimg = false;      
    }
}

