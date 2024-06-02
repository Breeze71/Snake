using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using V;

public class StartLaserWaveExample : MonoBehaviour
{
    public int waveIndex;
    private List<LaserController> _lasers;

    [Button]
    private void StartWave()
    {
        _lasers = LaserSpawner.I.StartLaserWave(waveIndex);
    }

    [Button]
    private void StopWave()
    {
        LaserSpawner.I.StopLaserWave(_lasers);
    }
}
