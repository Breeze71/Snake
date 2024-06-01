using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserWaveSO", menuName = "Laser")]
public class LaserWaveSO : ScriptableObject
{
    public GameObject LaserPrefab;
    public float _spawnCountDown;

    [ReorderableList] public LaserInfo[] _laserInfos;
    
    public void EnableLaser(LaserController laser)
    {
        foreach(LaserInfo laserInfo in _laserInfos)
        {
            laser.EnableLaser();
            laser.MoveStartPoint(laserInfo.StartPoint);
            laser.MoveEndPoint(laserInfo.EndPoint);
        }
    }

    public void EditLaserPosition(LaserController laser)
    {
        foreach(LaserInfo laserInfo in _laserInfos)
        {
            laser.MoveStartPoint(laserInfo.StartPoint);
            laser.MoveEndPoint(laserInfo.EndPoint);
        }       
    }
}

[System.Serializable]
public struct LaserInfo
{
    public Vector2 StartPoint;
    public Vector2 EndPoint;
}
