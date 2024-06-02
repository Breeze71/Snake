using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool Parent;

    protected virtual void OnDisable()
    {
        Parent.ReturnObjectToPool(this);
    }
}