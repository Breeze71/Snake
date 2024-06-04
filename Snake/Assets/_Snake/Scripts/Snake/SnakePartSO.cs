using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Snake", menuName = "Snake / Part")]
public class SnakePartSO : ScriptableObject
{
    public LayerMask ObstacleMask;
    public int obstacleDamage = 0;

    public float BulletChargingTime = .3f;
    public int BulletDamage = 1;
}
