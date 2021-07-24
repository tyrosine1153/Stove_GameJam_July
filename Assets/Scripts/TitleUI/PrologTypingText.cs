using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrologTypingText : MonoBehaviour
{
    [Multiline] [SerializeField] private string typingText;
    [SerializeField] private Text textUI;

    private Coroutine _coTypeText;
    public bool FlipedText { get; private set; }
    private void Start()
    {
        textUI.text = "";
        FlipedText = false;
    }

    public void TypeText(string text)
    {
        _coTypeText = StartCoroutine(CoTypeText(text ?? typingText));
    }

    IEnumerator CoTypeText(string text)
    {
        const float waitTime = 0.1f;
        textUI.text = "";
        foreach (var t in text)
        {
            textUI.text += t;
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void FlipText()
    {
        StopCoroutine(_coTypeText);
        textUI.text = typingText;
        FlipedText = true;
    }

    public void MoveSceneByEnding()
    {
        var isHappyEnd = EndingUIControl.IsHappyEnd;
        if (isHappyEnd)
        {
            SceneLoadManager.Instance.MoveScene(SceneType.Title);
        }
        else
        {
            SceneLoadManager.Instance.MoveScene(SceneType.InGame);
        }
    }
}
