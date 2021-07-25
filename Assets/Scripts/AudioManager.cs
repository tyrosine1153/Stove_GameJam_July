using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SfxType
{
    Click,
    Select,
    Cancel,
    Success,
    SuccessHigh,
    Fail,
    Unlock,
    Bell,
    Page,
}

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] 
    private AudioClip[] _audioClips;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip[] _sfxClips;

    [SerializeField]
    private AudioSource _sfxSource;

    //[SerializeField]
    //private Animator _animator;

    public float BgmVolume => _audioSource.volume;
    public float SfxVolume => _sfxSource.volume;

    //private static readonly int In = Animator.StringToHash("FadeIn");
    //private static readonly int Out = Animator.StringToHash("FadeOut");

    private static readonly string BGM_VOLUME_KEY = "BgmVolume";
    private static readonly string SFX_VOLUME_KEY = "SfxVolume";

    protected override void Awake()
    {
        base.Awake();

        var bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.5f);
        var sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.5f);

        SetBgmVolume(bgmVolume);
        SetSfxVolume(sfxVolume);
    }

    public void PlaySfx(SfxType sfxType)
    {
        int index = (int)sfxType;
        if (index < _sfxClips.Length)
        {
            _sfxSource.PlayOneShot(_sfxClips[index]);
        }
    }

    public void PlayBgm(int clipNumber)
    {
        _audioSource.clip = _audioClips[clipNumber];
        _audioSource.Play();
    }

    public void SetBgmVolume(float volume)
    {
        _audioSource.volume = volume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float volume)
    {
        _sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    /*
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
    */
}
