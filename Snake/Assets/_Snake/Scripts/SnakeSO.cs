using UnityEngine;

[CreateAssetMenu(fileName = "Snake", menuName = "Snake / Snake Move")]
public class SnakeSO : ScriptableObject
{
    // move
    [Header("Move")]
    public float Speed = 2f;
    public float RotationSpeed = 180f;
    public float Distance = .5f;
    public KeyCode AcclerateKey;
    public float shiftSpeed = 5f;
    public float AimDecreaseSpeed = 1.5f;
    public float disableInputTime = .5f;

    // health
    [Header("Health")]
    public int HealthAmount;
    public float InvincibleTime = .5f;

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
