using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtil : MonoBehaviour
{
    public void PlayClickSfx()
    {
        AudioManager.Instance.PlaySfx(SfxType.Click);
    }

    public void PlayPageSfx()
    {
        AudioManager.Instance.PlaySfx(SfxType.Page);
    }

    public void SetBgmVolume(float volume)
    {
        AudioManager.Instance.SetBgmVolume(volume);
    }

    public void SetSfxVolume(float volume)
    {
        AudioManager.Instance.SetSfxVolume(volume);
    }
}
