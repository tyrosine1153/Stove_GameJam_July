using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EndingUIControl : MonoBehaviour
{
    public static bool IsHappyEnd = false;

    private Animator _imageAnimator;
    private static readonly int EndingOption = Animator.StringToHash("EndingOption");

    void Start()
    {
        _imageAnimator = GetComponentInChildren< Animator>();

        var endingOption = IsHappyEnd ? 1 : 0;
        _imageAnimator.SetInteger(EndingOption, endingOption);
        Debug.Log(_imageAnimator.GetInteger(EndingOption));
    }
}
