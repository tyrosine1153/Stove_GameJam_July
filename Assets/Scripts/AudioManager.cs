using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] 
    private AudioClip[] _audioClips;
    
    private AudioSource _audioSource;
    private Animator _animator;
    private static readonly int In = Animator.StringToHash("FadeIn");
    private static readonly int Out = Animator.StringToHash("FadeOut");

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    public void FadeIn(int clipNumber)
    {
        _audioSource.clip = _audioClips[clipNumber] ?? _audioClips[0];
        _audioSource.Play();
        _animator.SetTrigger(In);
    }

    public void FadeOut()
    {
        StartCoroutine(nameof(CoFadeOut));
    }

    IEnumerator CoFadeOut()
    {
        _animator.SetTrigger(Out);
        yield return new WaitForSeconds(2f);
        
        _audioSource.Stop();
    }

}
