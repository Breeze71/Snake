using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class SnakePart : MonoBehaviour
{
    // store all history Transform
    public List<SnakePartInfo> SnakeParts = new List<SnakePartInfo>();
    [Expandable][SerializeField] protected SnakePartSO _partSO;

    [SerializeField] protected SnakeManager _snake;

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

    public abstract void HitByLaser(int damageAmount);
    public abstract void HitByBullet(int damageAmount);
}
