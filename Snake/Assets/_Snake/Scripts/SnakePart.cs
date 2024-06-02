using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class SnakePart : MonoBehaviour
{
    // store all history Transform
    [ReadOnly] public int partIndex;
    public List<SnakePartInfo> SnakeInfos = new List<SnakePartInfo>();
    [Expandable][SerializeField] protected SnakePartSO _partSO;

    [SerializeField] protected SnakeManager _snake;

    private void OnEnable()
    {
        _snake = GetComponentInParent<SnakeManager>();
    }

    private void FixedUpdate() 
    {
        StoreHistoryInfo();
    }

    public void StoreHistoryInfo()
    {
        SnakeInfos.Add(new SnakePartInfo(transform.position, transform.rotation));
    }

    public void ClearHistoryInfo()
    {
        SnakeInfos.Clear();
        SnakeInfos.Add(new SnakePartInfo(transform.position, transform.rotation));
    }

    public abstract void HitByLaser(int damageAmount);
    public abstract void HitByBullet(int damageAmount);
}
