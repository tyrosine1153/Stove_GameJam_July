using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderResultFloater : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Image _jewelImage;

    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Sprite _pearlSprite;

    [SerializeField]
    private Sprite _rubySprite;

    [SerializeField]
    private Sprite _diamondSprite;

    public void ShowResult(OrderResult jewel, int score)
    {
        var jewelSprite = GetJewelSprite(jewel);
        if (jewelSprite == null)
        {
            _jewelImage.gameObject.SetActive(false);
        }
        else
        {
            _jewelImage.gameObject.SetActive(true);
            _jewelImage.sprite = jewelSprite;
        }

        _scoreText.text = $"+{score}점";

        _animator.Play("Show");
    }

    private Sprite GetJewelSprite(OrderResult jewel)
    {
        switch(jewel)
        {
            case OrderResult.Pearl:
                return _pearlSprite;
            case OrderResult.Ruby:
                return _rubySprite;
            case OrderResult.Diamond:
                return _diamondSprite;
        }

        return null;
    }
}
