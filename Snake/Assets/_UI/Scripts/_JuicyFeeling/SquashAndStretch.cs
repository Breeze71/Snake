using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace V.Tool.JuicyFeeling
{
    public class SquashAndStretch : MonoBehaviour
    {
        private static event Action onSquashAndStretchAll;

        [SerializeField] private Transform affectTransfrom;
        public SquashStretchSO so; 

        private Coroutine squashAndStretchCoroutine;
        private Vector3 originScaleVector;
        private Vector3 originScale;

        private bool isReverse;

        // Flag 是否有選擇
        private bool affectX => (so.AxisToAffect & SquashStretchAxis.X) != 0;
        private bool affectY => (so.AxisToAffect & SquashStretchAxis.Y) != 0;
        private bool affectZ => (so.AxisToAffect & SquashStretchAxis.Z) != 0;
        
        public static void SquashAndStretchAll()
        {
            onSquashAndStretchAll?.Invoke();
        }

        #region Unity Fuc
        private void Awake() 
        {
            if(affectTransfrom == null)
            {
                affectTransfrom = transform;
            }

            originScaleVector = affectTransfrom.localScale;
        }

        private void Start() 
        {
            if(so.PlayOnStart)
            {
                StartSquashAndStretch();
            }    
        }

        private void OnEnable() 
        {
            onSquashAndStretchAll += PlaySquashAndStretch;
        }
        private void OnDisable() 
        {
            if(squashAndStretchCoroutine != null)
            {
                StopCoroutine(squashAndStretchCoroutine);
            }

            onSquashAndStretchAll -= PlaySquashAndStretch;
        }
        #endregion

        #region Squash And Stretch
        [Button]
        public void PlaySquashAndStretch()
        {
            if (so.CanLoop && !so.CanOverwritten) 
            {
                return;
            }

            StartSquashAndStretch();
        }
        public void PlaySquashAndStretch(SquashStretchSO _SquashStretchSO)
        {
            if (so.CanLoop && !so.CanOverwritten) 
            {
                return;
            }

            StartSquashAndStretch();            
        }


        private void StartSquashAndStretch()
        {
            if(so.AxisToAffect == SquashStretchAxis.None)
            {
                Debug.Log("No Affect Vector");
                return;
            }

            if(squashAndStretchCoroutine != null)
            {
                StopCoroutine(squashAndStretchCoroutine);

                if(so.CanPlayEveryTime && so.ResetScaleOrNot)
                {
                    ResetScale();
                }
            }

            squashAndStretchCoroutine = StartCoroutine(Coroutine_SquashStretch());
        }

        private IEnumerator Coroutine_SquashStretch()
        {
            WaitForSeconds _loopingDelay = new WaitForSeconds(so.LoopingDelay);

            do
            {   
                // 依照機率播放
                if(!so.CanPlayEveryTime)
                {
                    if(UnityEngine.Random.Range(0f, 100f) > so.PlayPercentage)
                    {
                        yield return null;
                        continue;
                    }
                }

                if(so.CanReverseAfterPlaying)
                {
                    isReverse = !isReverse;
                }

                float _elapsedTimer = 0;
                originScale = originScaleVector;
                Vector3 _modifiedScale = originScale;

                while (_elapsedTimer < so.Duration)
                {
                    _elapsedTimer += Time.deltaTime;
                    
                    // Curve Postion
                    float _curvePosition;

                    // 判斷是否要依照 Curve 擠壓或膨脹
                    if(isReverse)
                    {
                        _curvePosition = 1 - (_elapsedTimer / so.Duration);
                    }
                    else
                    {
                        _curvePosition = _elapsedTimer / so.Duration;
                    }

                    // Curve Value
                    float _curveValue = so.SquashStretchCurve.Evaluate(_curvePosition);
                    float _remapValue = so.OriginScale + (_curveValue * (so.MaxScale - so.OriginScale));    // 確保大小介於 Origin Scale and MaxScale

                    float _miniumThreshold = .0001f;
                    if(Mathf.Abs(_remapValue) < _miniumThreshold)
                    {
                        _remapValue = _miniumThreshold;
                        Debug.Log("Minimun");
                    }

                    _modifiedScale = CheckModifiedVector(_modifiedScale, originScale, _remapValue);
                    affectTransfrom.localScale = _modifiedScale;

                    yield return null;
                }

                if(so.ResetScaleOrNot)
                {
                    affectTransfrom.localScale = originScale;
                }

                if(so.CanLoop)
                {
                    yield return _loopingDelay;
                }
            }while(so.CanLoop);
        }
    
        private Vector3 CheckModifiedVector(Vector3 _modifiedScale, Vector3 _originScale, float _remapValue)
        {
            if (affectX)
            {
                _modifiedScale.x = _originScale.x * _remapValue;
            }
            else
            {
                _modifiedScale.x = _originScale.x / _remapValue;
            }

            if (affectY)
            {
                _modifiedScale.y = _originScale.y * _remapValue;
            }
            else
            {
                _modifiedScale.y = _originScale.y / _remapValue;
            }

            if (affectZ)
            {
                _modifiedScale.z = _originScale.z * _remapValue;
            }
            else
            {
                _modifiedScale.z = _originScale.z / _remapValue;
            }

            return _modifiedScale;
        }
        #endregion

        [Button]
        public void ResetScale()
        {
            if(squashAndStretchCoroutine != null)
            {
                StopCoroutine(squashAndStretchCoroutine);
            }
            
            affectTransfrom.localScale = originScaleVector;
        }    
    }
}
