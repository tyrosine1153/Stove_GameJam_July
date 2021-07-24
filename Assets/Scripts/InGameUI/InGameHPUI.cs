using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameHPUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> _hpImages;

    [SerializeField]
    private Sprite _enableSprite;

    [SerializeField]
    private Sprite _disableSprite;

    public void SetHp(int hp)
    {
        for (int i = 0; i < hp; i++)
        {
            _hpImages[i].sprite = _enableSprite;
        }

        for (int i = hp; i < _hpImages.Count; i++)
        {
            _hpImages[i].sprite = _disableSprite;
        }
    }
}
