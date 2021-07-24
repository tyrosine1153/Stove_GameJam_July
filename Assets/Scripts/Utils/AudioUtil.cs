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
}
