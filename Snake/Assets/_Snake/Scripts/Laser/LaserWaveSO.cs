using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserWaveSO", menuName = "Laser")]
public class LaserWaveSO : ScriptableObject
{
    public float _spawnCountDown;

    [ReorderableList]
    public LaserInfo[] _laserInfos;
    

    public void EnableLaser(LaserController laser)
    {
        foreach(LaserInfo laserInfo in _laserInfos)
        {
            laserInfo.LaserPrefab.EnableLaser();
            laserInfo.LaserPrefab.MoveStartPoint(laserInfo.StartPoint);
            laserInfo.LaserPrefab.MoveEndPoint(laserInfo.EndPoint);
        }
    }

    public void EditLaserPosition()
    {
        foreach(LaserInfo laserInfo in _laserInfos)
        {
            laserInfo.LaserPrefab.MoveStartPoint(laserInfo.StartPoint);
            laserInfo.LaserPrefab.MoveEndPoint(laserInfo.EndPoint);
        }       
    }
}

[System.Serializable]
public struct LaserInfo
{
    public GameObject LaserPrefab;
    public Vector2 StartPoint;
    public Vector2 EndPoint;
}
