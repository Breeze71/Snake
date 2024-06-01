using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;

namespace V
{
public class LaserSpawner : MonoBehaviour
{
    public static LaserSpawner I {private set; get;}

    [SerializeField] private GameObject LaserPrefab;
    [Expandable]
    [SerializeField] private LaserWaveSO[] _laserWaves;
    
    private ObjectPool<LaserController> _laserPool;
    private Coroutine _laserEnableCoroutine;

    #region LC
    private void Awake() 
    {
        if(I != null)
        {
            Debug.LogError("More than one Instance");
            Destroy(this);
        }
        I = this;

        _laserPool = new ObjectPool<LaserController>(CreatePool);    
    }

    private void Start() 
    {
        if(_laserEnableCoroutine != null)
        {
            _laserEnableCoroutine = null;
            _laserEnableCoroutine = StartCoroutine(Coroutine_StartLaserWave());
        }

        _laserEnableCoroutine = StartCoroutine(Coroutine_StartLaserWave());
    
    }

    private void Update() 
    {
#if UNITY_EDITOR
        if(_lasers != null && _laserInfoSOs != null)
        {
            UpdateLaserPosition();
        }
#endif
    }
    #endregion

    private IEnumerator Coroutine_StartLaserWave()
    {
        for(int laserWaveIndex = 0; laserWaveIndex < _laserWaves.Length; laserWaveIndex++)
        {
            yield return new WaitForSeconds(_laserWaves[laserWaveIndex]._spawnCountDown);

            List<LaserController> newLasers = GetLaserFromPool(_laserWaves[laserWaveIndex].LaserInfos.Length, laserWaveIndex);
        }
    }

    #region Object Pool
    private LaserController CreatePool()
    {
        GameObject newLaserGO = Instantiate(LaserPrefab, transform);
        LaserController laser = newLaserGO.GetComponent<LaserController>();

        return laser;
    }
    private LaserController GetLaserFromPool()
    {
        LaserController newLaser = _laserPool.Get();
        newLaser.gameObject.SetActive(true);

        return newLaser;
    }
    private List<LaserController> GetLaserFromPool(int waveLaserAmount, int waveIndex)
    {
        List<LaserController> lasers = new List<LaserController>();
        for(int laserIndex = 0; laserIndex < waveLaserAmount; laserIndex++)
        {
            lasers.Add(_laserPool.Get());

            lasers[laserIndex].gameObject.SetActive(true);
            
            SetUpLaser(lasers[laserIndex], laserIndex, waveIndex);
        }

        return lasers;        
    }
    private LaserController SetUpLaser(LaserController laser, int laserIndex, int waveIndex)
    {
        laser.MoveStartPoint(_laserWaves[waveIndex].LaserInfos[laserIndex].StartPoint);
        laser.MoveEndPoint(_laserWaves[waveIndex].LaserInfos[laserIndex].EndPoint);  

        return laser;
    }
    private void ReleaseLaser(LaserController laser)
    {
        laser.gameObject.SetActive(false);
        _laserPool.Release(laser);
    }
    private void ReleaseLaser(List<LaserController> lasers)
    {
        for(int i = 0; i < lasers.Count; i++)
        {
            lasers[i].gameObject.SetActive(false);
            _laserPool.Release(lasers[i]);
        }
    }    
    #endregion
    
#if UNITY_EDITOR
    private List<LaserController> _lasers = new List<LaserController>();
    private LaserInfoSO[] _laserInfoSOs;
    public void SpawnWave(LaserInfoSO[] laserInfos)
    {
        if(_lasers.Count != 0)
        {
            ReleaseLaser(_lasers);
        }

        List<LaserController> lasers = new List<LaserController>();
        for(int laserIndex = 0; laserIndex < laserInfos.Length; laserIndex++)
        {
            lasers.Add(_laserPool.Get());

            lasers[laserIndex].gameObject.SetActive(true);
            
            lasers[laserIndex].MoveStartPoint(laserInfos[laserIndex].StartPoint);
            lasers[laserIndex].MoveEndPoint(laserInfos[laserIndex].EndPoint);  
        }

        _lasers = lasers;
        _laserInfoSOs = laserInfos;
    }

    private void UpdateLaserPosition()
    {
        for(int laserIndex = 0; laserIndex < _laserInfoSOs.Length; laserIndex++)
        {
            _lasers[laserIndex].MoveStartPoint(_laserInfoSOs[laserIndex].StartPoint);
            _lasers[laserIndex].MoveEndPoint(_laserInfoSOs[laserIndex].EndPoint);  
        }
    }
#endif
}
}