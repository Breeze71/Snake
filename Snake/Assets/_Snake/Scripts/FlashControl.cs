using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FlashControl : MonoBehaviour
{
    [SerializeField] private Color _invincibleColor = Color.white;
    [SerializeField] private float _flashTime = .25f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Material _flashMat;

    private Coroutine _flashCorotine;

    private void Awake() 
    {
        _flashMat = _spriteRenderer.material;
    }

    [Button]
    public void StartFlash()
    {
        if(_flashCorotine != null)
        {
            StopCoroutine(_flashCorotine);
        }

        StartCoroutine(Coroutine_Flash(_flashTime));
    }

    public void StartFlash(float time)
    {
        if(_flashCorotine != null)
        {
            StopCoroutine(_flashCorotine);
        }

        StartCoroutine(Coroutine_Flash(time));
    }

    private IEnumerator Coroutine_Flash(float time)
    {
        Debug.Log("StartFlash");
        _flashMat.SetColor("_Color", _invincibleColor * 2f);

        float currentFlashAmount = 0f;
        float elaTime = 0f;
        while(elaTime < time)
        {
            elaTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, 0f, (elaTime / time));
            _flashMat.SetFloat("_Intensity", currentFlashAmount);
            yield return null;
        }
    }
}
