using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : SnakePart
{
    public override void HitByLaser(int damageAmount)
    {
        if(IsAimimg)   return;
        if(_snake.IsPause) return;

        base.HitByLaser(damageAmount);
        _snake.DestroyBodyAndAfter(partIndex);
    }

    public override void HitByBullet(int damageAmount)
    {
        if(IsAimimg)   return;
        if(_snake.IsPause) return;
        
        base.HitByBullet(damageAmount);
        _snake.DestroyLastBody();
    }

    public override void SetAiming()
    {
        base.SetAiming();
        
                _sprite.enabled = false;
        IsAimimg = true;    
    }

    public override void SetNotAim()
    {
        base.SetNotAim();

                _sprite.enabled = true;
        IsAimimg = false;      
    }
}

