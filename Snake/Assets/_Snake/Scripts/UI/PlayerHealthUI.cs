using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> _healthBlocks;
    [SerializeField] private List<Sprite> _hurtBlocks;
    [SerializeField] private List<Sprite> _fullhealth;
    [SerializeField] private SnakeManager _snake;

    private void Start() 
    {
        _snake.SnakeHealth.HealthChangedEvent += SnakeHealth_OnHealthChanged;
    }

    private void SnakeHealth_OnHealthChanged(int currentHealth)
    {
        foreach(GameObject healthBlock in _healthBlocks)
        {
            //healthBlock.SetActive(false);
            //healthBlock.GetComponent<Image>().sprite=_hu
        }

        for (int i = 0; i < _healthBlocks.Count; i++)
        {
            _healthBlocks[i].GetComponent<Image>().sprite = _hurtBlocks[i];
        }

        for(int i = 0; i < currentHealth; i++)
        {
            //_healthBlocks[i].SetActive(true);
            _healthBlocks[i].GetComponent<Image>().sprite = _fullhealth[i];
        }
    }
}
