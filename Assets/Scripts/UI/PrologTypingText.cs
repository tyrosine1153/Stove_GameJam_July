using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrologTypingText : MonoBehaviour
{
    [Multiline]
    public string typingText;
    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
        _text.text = "";
    }

    public void TypeText()
    {
        StartCoroutine(CoTypeText(typingText));
    }

    IEnumerator CoTypeText(string text)
    {
        const float waitTime = 0.1f;
        foreach (var t in text)
        {
            _text.text += t;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
