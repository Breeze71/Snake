using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


[CreateAssetMenu(fileName = "LaserPos", menuName = "Laser / LaserPosition", order = 1)]
public class LaserInfoSO : ScriptableObject
{
    public Vector2 StartPoint;
    public Vector2 EndPoint;
}
