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

    private  List<ParticleSystem> particles = new List<ParticleSystem>();

    #region LC
    private void Start() 
    {
        SetupParticleList();
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
        _startVFX.transform.position = _startPoint.position;   
        Vector2 endPoint = _endPoint.transform.position;

        Vector2 direction = endPoint - (Vector2)_startPoint.position;
        RaycastHit2D hitPos = Physics2D.Raycast(_startPoint.position , direction.normalized , direction.magnitude, _laserHitLayer);

        if(hitPos)
        {
            _lineRenderer.SetPosition(1 , hitPos.point);
            Damage(hitPos);
        }
        else
        {
            _lineRenderer.SetPosition(1 , endPoint);
        }

        _endVFX.transform.position = _lineRenderer.GetPosition(1);      
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
}
