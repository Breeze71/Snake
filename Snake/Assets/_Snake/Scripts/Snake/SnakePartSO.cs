using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Snake", menuName = "Snake / Part")]
public class SnakePartSO : ScriptableObject
{
    public LayerMask ObstacleMask;
}
