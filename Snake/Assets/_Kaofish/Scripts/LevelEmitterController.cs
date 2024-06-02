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
        BulletEmitter.emitterProfile = EmitterProfile;
        BulletEmitter.Play();
    }
}
[Serializable]
public class StageConfiguration
{
    public int WaveNumber;
    [SerializeField]
    public List<SingleEmitter> SingleEmitters;
}
public class LevelEmitterController : MonoBehaviour
{
    private bool _ifFirst = true;
    public List<StageConfiguration> StageConfigurations;
    private int _nowStage = 0;
    private void ClosePreviousEmitters()
    {
        foreach (var singleEmitter in StageConfigurations[_nowStage].SingleEmitters)
        {
            singleEmitter.BulletEmitter.Kill();
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
            ChangeToStage(0);
        }
        else if (_nowStage + 1 < StageConfigurations.Count)
        {
            ChangeToStage(_nowStage+1);
        }
        else
        {
            ChangeToStage(0);
        }
    }
}
