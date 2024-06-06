using System;
using System.Collections;
using System.Collections.Generic;
using BulletPro;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class SingleEmitter
{
    public BulletEmitter BulletEmitter;
    public EmitterProfile EmitterProfile;

    public void ChangeProfile()
    {
        if (BulletEmitter == null || EmitterProfile == null)
        {
            Debug.LogWarning("BulletEmitter or EmitterProfile is not set!");
            return;
        }
        BulletEmitter.emitterProfile = EmitterProfile;
        BulletEmitter.Play();
    }
}
[Serializable]
public class StageConfiguration
{
    public int WaveNumber;
    public List<SingleEmitter> SingleEmitters=new List<SingleEmitter>();
}
public class LevelEmitterController : MonoBehaviour
{
    private bool _ifFirst = true;
    public List<StageConfiguration> StageConfigurations=new List<StageConfiguration>();
    private int _nowStage = -1;
    private void ClosePreviousEmitters()
    {
        if (_nowStage == 0)
        {
            foreach (var singleEmitter in StageConfigurations[StageConfigurations.Count-1].SingleEmitters)
            {
                singleEmitter.BulletEmitter.Kill();
            }
        }
        else if(_nowStage == -1)
        {
            foreach (var singleEmitter in StageConfigurations[0].SingleEmitters)
            {
                singleEmitter.BulletEmitter.Kill();
            }
        }
        else
        {
            foreach (var singleEmitter in StageConfigurations[_nowStage-1].SingleEmitters)
            {
                singleEmitter.BulletEmitter.Kill();
            }
        }
    }

    private void SwitchNextStage(int num)
    {
        foreach (var singleEmitter in StageConfigurations[num].SingleEmitters)
        {
            singleEmitter.ChangeProfile();
        }
    }
    public void ChangeToStage(int num)
    {
        if (_nowStage != num)
        {
            Debug.Log(num+"   "+StageConfigurations.Count);
            ClosePreviousEmitters();
            _nowStage = num;
            SwitchNextStage(num);
        }
    }
    public void ChangeToStage(int num,bool ifEditor)
    {
        Debug.Log(num+"   "+StageConfigurations.Count);
        ClosePreviousEmitters();
        _nowStage = num;
        SwitchNextStage(num);
    }
    [Button]
    public void NextStage()
    {
        if (_ifFirst)
        {
            _ifFirst = false;
            ChangeToStage(0,true);
        }
        else if (_nowStage + 1 < StageConfigurations.Count)
        {
            _nowStage += 1;
            ChangeToStage(_nowStage,true);
        }
        else
        {
            _nowStage = 0;
            ChangeToStage(0,true);
        }
    }
}
