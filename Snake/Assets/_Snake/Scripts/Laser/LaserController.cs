using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("Laser")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _endLaser;
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
        DisableLaser();
        EnableLaser();
    }

    private void Update() 
    {
        UpdateLaser();
    }
    #endregion

    public void EnableLaser()
    {
        _lineRenderer.enabled = true;
        
        for(int i = 0 ; i < particles.Count ; i++)
        {
            particles[i].Play();
        }
    }

    public void DisableLaser()
    {
        _lineRenderer.enabled = false;

        for(int i = 0 ; i < particles.Count ; i++)
        {
            particles[i].Stop();
        }
    }    

    private void UpdateLaser()
    {
        _lineRenderer.SetPosition(0 , _firePoint.position);
        _startVFX.transform.position = _firePoint.position;   
        Vector2 endPoint = _endLaser.transform.position;

        Vector2 direction = endPoint - (Vector2)_firePoint.position;
        RaycastHit2D hitPos = Physics2D.Raycast(_firePoint.position , direction.normalized , direction.magnitude, _laserHitLayer);

        if(hitPos)
        {
            _lineRenderer.SetPosition(1 , hitPos.point);
        }
        else
        {
            _lineRenderer.SetPosition(1 , endPoint);
        }

        _endVFX.transform.position = _lineRenderer.GetPosition(1);      

        // RotateLaser(hitPos.point);  
    }

    private void RotateLaser(Vector2 hitPos)
    {
        Vector2 direction = (hitPos - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(direction.y , direction.x) * Mathf.Rad2Deg;
        
        rotation.eulerAngles = new Vector3(0 , 0 , angle);
        transform.rotation = rotation;        
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
