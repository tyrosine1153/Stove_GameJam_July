using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrologTypingText : MonoBehaviour
{
    [Multiline] [SerializeField] private string typingText;

    [SerializeField]
    private Text _text;

    private void Start()
    {
        _text.text = "";
    }

    public void TypeText(string text)
    {
        StartCoroutine(CoTypeText(text ?? typingText));
    }

    IEnumerator CoTypeText(string text)
    {
        const float waitTime = 0.1f;
        _text.text = "";
        foreach (var t in text)
        {
            _text.text += t;
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void LoadTitleScene()
    {
        SceneLoadManager.Instance.LoadTitleScene();
    }
}
