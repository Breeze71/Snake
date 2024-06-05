using System.Collections;
using UnityEngine;

namespace V.Tool
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;

        [SerializeField] private float _waitTimer = 3f;
        private Coroutine _waitScreenShiftCoroutine;
        
        private void Update() 
        {
            if(isFirstUpdate)
            {
                if(_waitScreenShiftCoroutine != null)
                {
                    StopCoroutine(_waitScreenShiftCoroutine);
                }
                StartCoroutine(Coroutine_WaitScreenShift());
            }    
        }

        private IEnumerator Coroutine_WaitScreenShift()
        {
            isFirstUpdate = false;
            yield return new WaitForSeconds(_waitTimer);

            Loader.LoaderCallback();
        }
    }
}