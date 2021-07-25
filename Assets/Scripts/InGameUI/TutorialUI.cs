using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private List<Sprite> _sprites = new List<Sprite>();

    private int _step = 0;

    private static readonly string TUTORIAL_KEY = "Tutorial";

    private void Start()
    {
        if (PlayerPrefs.HasKey(TUTORIAL_KEY))
        {
            return;
        }
        _image.gameObject.SetActive(true);
        UpdateSprite();
    }

    public void ProgressTutorial()
    {
        _step += 1;

        if (_step >= _sprites.Count)
        {
            _image.gameObject.SetActive(false);
            PlayerPrefs.SetInt(TUTORIAL_KEY, 0);
            return;
        }

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (_step >= 0 && _step < _sprites.Count)
        {
            _image.sprite = _sprites[_step];
        }
    }
}
