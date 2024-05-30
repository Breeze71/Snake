using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public List<SnakePartInfo> snakeParts = new List<SnakePartInfo>();

    public void AddSnakePart()
    {
        snakeParts.Add(new SnakePartInfo(transform.position, transform.rotation));
    }
}
