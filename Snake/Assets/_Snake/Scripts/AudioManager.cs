using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I {get; private set;} 

    [field : SerializeField] public AudioSO _audioSO { get; private set;}
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _volume;

    private void Awake()
    {
        if(I != null)
        {
            Destroy(this);
            Debug.LogError("More than one Audio");
            return;
        }

        I = this;
    }

    private void Start() 
    {
        DontDestroyOnLoad(this);    
    }

    public void PlayOneShotSound(AudioClip clip)
    {
        if(clip != null)
        {
            _audioSource.PlayOneShot(clip, _volume);
        }
    }

    public void PlayOneShotSound(AudioClip[] clips)
    {
        if(clips != null)
        {
            _audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)], _volume);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, _volume);
        Debug.Log("play " + clip);
    }

    [Button]
    public void Test()
    {
                AudioSource.PlayClipAtPoint(_audioSO.HurtClip, transform.position, _volume);
    }

}
