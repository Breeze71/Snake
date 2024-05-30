using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public List<SnakePartInfo> SnakeParts = new List<SnakePartInfo>();

    public void AddSnakePart()
    {
        SnakeParts.Add(new SnakePartInfo(transform.position, transform.rotation));
    }

    public void ClearPartList()
    {
        SnakeParts.Clear();
        SnakeParts.Add(new SnakePartInfo(transform.position, transform.rotation));
    }
}
