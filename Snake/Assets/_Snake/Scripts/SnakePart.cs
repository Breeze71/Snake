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
    public GameObject Indicator;
    public bool IsAimimg;

    protected virtual void OnEnable()
    {
        _snake = GetComponentInParent<SnakeManager>();
    }

    protected virtual void FixedUpdate() 
    {
        if(_snake.IsPause)  return;

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

    public virtual void SetAiming()
    {

    }

    public virtual void SetNotAim()
    {

    }
}
