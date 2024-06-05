using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    // store all history Transform
    [ReadOnly] public int partIndex;
    public List<SnakePartInfo> SnakeInfos = new List<SnakePartInfo>();
    [Expandable][SerializeField] protected SnakePartSO _partSO;
    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] protected SnakeManager _snake;
    public FlashControl Flash;
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

    public virtual void HitByLaser(int damageAmount)
    {
        AudioManager.I.PlayOneShotSound(AudioManager.I._audioSO.HurtClip);
    }
    public virtual void HitByBullet(int damageAmount)
    {
        AudioManager.I.PlayOneShotSound(AudioManager.I._audioSO.HurtClip);
    }

    public virtual void SetAiming()
    {

    }

    public virtual void SetNotAim()
    {

    }
}
