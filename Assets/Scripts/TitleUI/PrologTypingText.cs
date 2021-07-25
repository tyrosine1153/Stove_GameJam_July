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
    public bool FlipedText { get; private set; }

    private Coroutine _coTypeText;

    private void Start()
    {
        textUI.text = "";
        FlipedText = false;
    }

    public void TypeText(string text)
    {
        if (string.IsNullOrEmpty(text)) text = typingText;
        
        _coTypeText = StartCoroutine(CoTypeText(text));
    }

    IEnumerator CoTypeText(string text)
    {
        const float waitTime = 0.1f;
        textUI.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            textUI.text += text[i];
            yield return new WaitForSeconds(waitTime);
        }
        FlipedText = true;
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
