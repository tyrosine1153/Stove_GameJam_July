using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider _bgmSlider;

    [SerializeField]
    private Slider _sfxSlider;

    private void Start()
    {
        _bgmSlider.SetValueWithoutNotify(AudioManager.Instance.BgmVolume);
        _sfxSlider.SetValueWithoutNotify(AudioManager.Instance.SfxVolume);
    }
}
