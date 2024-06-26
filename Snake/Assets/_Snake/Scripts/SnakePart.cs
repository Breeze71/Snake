using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    // store all history Transform
    public List<SnakePartInfo> SnakeParts = new List<SnakePartInfo>();

    private void FixedUpdate() 
    {
        StoreHistoryInfo();
    }

    public void StoreHistoryInfo()
    {
        SnakeParts.Add(new SnakePartInfo(transform.position, transform.rotation));
    }

    public void ClearHistoryInfo()
    {
        SnakeParts.Clear();
        SnakeParts.Add(new SnakePartInfo(transform.position, transform.rotation));
    }
}
