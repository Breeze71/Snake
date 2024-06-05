using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("Laser")]
    public Transform _startPoint;
    public Transform _endPoint;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LayerMask _laserHitLayer;

    private Quaternion rotation;

    [Header("VFXEffect")]
    [SerializeField] private GameObject _startVFX;
    [SerializeField] private GameObject _endVFX;

    [Header("Remind Laser")]
    [SerializeField] private LineRenderer _remindRenderer;
    [SerializeField] private GameObject _remindLaser;
    [SerializeField] private Material _remindMat;
    [SerializeField] private Color _remindColor = Color.red;
    private bool isStillRemind = true;
    private Coroutine _flashCoro;
    private Coroutine _LaserREmindCoro;

    private  List<ParticleSystem> particles = new List<ParticleSystem>();

    #region LC
    private void Awake() 
    {
        _remindMat = _remindRenderer.material;
    }
    private void OnEnable() 
    {
        if(_LaserREmindCoro != null)
        {
            StopCoroutine(_LaserREmindCoro);
        }
        StartCoroutine(Coroutine_LaserReminder());            
    }
    private void Start() 
    {
        SetupParticleList();

        _remindMat = _remindRenderer.material;
    }

    private void Update() 
    {
        UpdateLaser();
    }
    #endregion

    public void MoveStartPoint(Vector2 startPos)
    {   
        _startPoint.position = startPos;
    }

    public void MoveEndPoint(Vector2 endPos)
    {
        _endPoint.position = endPos;
    }

    private void UpdateLaser()
    {
        _lineRenderer.SetPosition(0 , _startPoint.position);
        _remindRenderer.SetPosition(0 , _startPoint.position);

        _startVFX.transform.position = _startPoint.position;   
        Vector2 endPoint = _endPoint.transform.position;

        Vector2 direction = endPoint - (Vector2)_startPoint.position;
        RaycastHit2D hitPos = Physics2D.Raycast(_startPoint.position , direction.normalized , direction.magnitude, _laserHitLayer);

        if(hitPos)
        {
            _lineRenderer.SetPosition(1 , hitPos.point);
            _remindRenderer.SetPosition(1 , hitPos.point);

            if(!isStillRemind)
            {
                Damage(hitPos);
            }
        }
        else
        {
            _lineRenderer.SetPosition(1 , endPoint);
            _remindRenderer.SetPosition(1 , endPoint);
        }

        if(isStillRemind)
        {
            _endVFX.transform.position = _lineRenderer.GetPosition(1);     
        }
        else
        {
            _endVFX.transform.position = _remindRenderer.GetPosition(1);    
        }
    }

    private void Damage(RaycastHit2D hit2D)
    {
        SnakePart part; 
        if(hit2D.collider.TryGetComponent(out part))
        {
            part.HitByLaser(1);
        }
    }

    private void SetupParticleList()
    {
        for(int i = 0 ; i < _startVFX.transform.childCount ; i++)
        {
            var ps = _startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();

            if(ps != null)
            {
                particles.Add(ps);
            }
        }

        for(int i = 0 ; i < _endVFX.transform.childCount ; i++)
        {
            var ps = _endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();

            if(ps != null)
            {
                particles.Add(ps);
            }
        }
    }

    private IEnumerator Coroutine_LaserReminder()
    {
        isStillRemind = true;
        _remindLaser.SetActive(true);
        _lineRenderer.enabled = false;

        StartFlash(.4f);
        yield return new WaitForSeconds(.4f);
        StartFlash(.4f);
        yield return new WaitForSeconds(.4f);
        StartFlash(.4f);
        yield return new WaitForSeconds(.4f);
        StartFlash(.4f);
        yield return new WaitForSeconds(.4f);

        _remindLaser.SetActive(false);
        isStillRemind = false;
        _lineRenderer.enabled = true;
    }
    public void StartFlash(float time)
    {
        if(_flashCoro != null)
        {
            StopCoroutine(_flashCoro);
        }
        StartCoroutine(Coroutine_Flash(time));
    }
    private IEnumerator Coroutine_Flash(float time)
    {
        _remindMat.SetColor("_Color", _remindColor * 2f);

        float currentFlashAmount = 0f;
        float elaTime = 0f;
        while(elaTime < time)
        {
            elaTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, .2f, (elaTime / time));
            _remindMat.SetFloat("_Intensity", currentFlashAmount);
            yield return null;
        }
    }    
}
