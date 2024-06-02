using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SnakeSO")]
public class SnakeSO : ScriptableObject
{
    public float Speed = 2f;
    public float RotationSpeed = 180f;
    public float Distance = .5f;
    public KeyCode AcclerateKey;
    public float shiftSpeed = 5f;

    private float horizontalInput;
    private float verticalInput;

    public Vector2 HandleMoveDirection()
    {
        Vector2 moveDir = Vector3.zero;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(horizontalInput, verticalInput);
        
        return moveDir.normalized;        
    }

    public float GetAngleFromVector(Vector2 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if(n < 0)
        {
            n += 360f;
        }

        return n;
    }

}
