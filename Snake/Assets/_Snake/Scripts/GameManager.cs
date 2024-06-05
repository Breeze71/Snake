using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using V.Tool;
using V.UI;
namespace V
{
public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;} 

    public int CurrentState;

    [SerializeField] private SnakeManager _snake;
    [SerializeField] private GameObject _blocksParents;
    [ReadOnly] [SerializeField] private List<BossBlock> _bossBlocks;

    [SerializeField] private List<int> _bossStateHealths = new List<int>();
    private int currentBossHealth = 0;

    private LevelEmitterController levelEmitterController;

    #region Lc
    private void Awake() 
    {
        _bossBlocks = _blocksParents.GetComponentsInChildren<BossBlock>().ToList<BossBlock>();
        levelEmitterController = FindObjectOfType<LevelEmitterController>();
    }

    private void Start()
    {
        _snake.SnakeHealth.HealthChangedEvent += Snake_OnHealthChanged;
        foreach(BossBlock bossHealth in _bossBlocks)
        {
            bossHealth.Health.HealthChangedEvent += BossHealth_OnHealthChanged;

            currentBossHealth += bossHealth.Health.GetHealthAmount();
        }

        CheckCurrentState();
    }

    private void OnDestroy()
    {
        _snake.SnakeHealth.HealthChangedEvent -= Snake_OnHealthChanged;
        foreach(BossBlock bossHealth in _bossBlocks)
        {
            bossHealth.Health.HealthChangedEvent -= BossHealth_OnHealthChanged;
        }
    }
    #endregion

    private void Snake_OnHealthChanged(int currentHealth)
    {
        if(currentHealth == 0)
        {
            UIManager.Instance.ShowUI<ReplayUI>("ReplayUI");
        }
    }

    private void BossHealth_OnHealthChanged(int obj)
    {
        currentBossHealth--;

        Debug.Log("current boss health" + currentBossHealth);
        CheckCurrentState();

        if(currentBossHealth == 0)
        {
            Loader.LoadNextScene();
        }
    }

    private void CheckCurrentState()
    {
        for(int i = _bossStateHealths.Count - 1; i > 0; i--)
        {
            if(currentBossHealth <= _bossStateHealths[i])
            {
                CurrentState = i;
                break;
            }
        }  
        if (levelEmitterController != null)
        {
            levelEmitterController.ChangeToStage(CurrentState);
        }
    }
}
}