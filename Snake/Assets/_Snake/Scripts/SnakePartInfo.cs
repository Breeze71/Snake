using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SnakePartInfo
{
    public Vector3 Position;
    public Quaternion Rotation;

    public SnakePartInfo(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}
