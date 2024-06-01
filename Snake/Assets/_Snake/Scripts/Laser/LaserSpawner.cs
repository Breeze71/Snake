using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;

namespace V
{
public class LaserSpawner : MonoBehaviour
{
    [SerializeField] private bool _isEditMode;
    
    [Expandable] [ReorderableList]
    [SerializeField] private LaserWaveSO[] _laserWaves;
    
    private ObjectPool<LaserController> _laserPool;
    private GameObject currentSpawnPrefab;
    private Coroutine _laserEnableCoroutine;

    #region LC
    private void Awake() 
    {
        _laserPool = new ObjectPool<LaserController>(CreatePool);    
    }

    private void Start() 
    {
        if(_laserEnableCoroutine != null)
        {
            _laserEnableCoroutine = null;
            _laserEnableCoroutine = StartCoroutine(Coroutine_EnableLaser());
        }

        _laserEnableCoroutine = StartCoroutine(Coroutine_EnableLaser());
    
    }

    private void Update() 
    {
#if UNITY_EDITOR
        EditLaserMode();
#endif
    }
    #endregion

    private IEnumerator Coroutine_EnableLaser()
    {
        for(int i = 0; i < _laserWaves.Length; i++)
        {
            yield return new WaitForSeconds(_laserWaves[i]._spawnCountDown);
            
            currentSpawnPrefab = _laserWaves[i].LaserPrefab;

            
            LaserController newLaser = GetLaserFromPool();
            // _laserWaves[i].EnableLaser();
        }
    }

    #region Object Pool
    public LaserController CreatePool()
    {
        GameObject newLaserGO = Instantiate(currentSpawnPrefab, transform);
        LaserController laser = newLaserGO.GetComponent<LaserController>();

        return laser;
    }
    private LaserController GetLaserFromPool()
    {
        LaserController newLaser = _laserPool.Get();
        newLaser.gameObject.SetActive(true);

        return newLaser;
    }
    private void ReleaseTrail(LaserController laser)
    {
        laser.gameObject.SetActive(false);
        _laserPool.Release(laser);
    }
    #endregion

#if UNITY_EDITOR
    private void EditLaserMode()
    {
        if(!_isEditMode) return;

        foreach(LaserWaveSO laser in _laserWaves)
        {
            // laser.EditLaserPosition();
        }          
    }
#endif
}
}