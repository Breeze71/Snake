using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using V;

[CreateAssetMenu(fileName = "LaserWave", menuName = "Laser / LaserWave", order = 1)]
public class LaserWaveSO : ScriptableObject
{
    public float _spawnCountDown;
    [Expandable] public LaserInfoSO[] LaserInfos;

#if UNITY_EDITOR
    [Button]
    public void SpawnWave()
    {
        LaserSpawner.I.SpawnWave(LaserInfos);
    }
#endif
}
