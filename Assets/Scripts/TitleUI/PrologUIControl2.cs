using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologUIControl2 : MonoBehaviour
{
    [SerializeField] private GameObject[] prologPages;
    [SerializeField] private int onEndPrologAudio;
    [SerializeField] private PrologTypingText[] typingTexts;

    private int _pageCount;

    private void Start()
    {
        _pageCount = 0;
        prologPages[_pageCount].SetActive(true);

        for (int i = 1; i < prologPages.Length; i++)
        {
            prologPages[i].SetActive(false);
        }

        if (typingTexts[_pageCount] != null) typingTexts[_pageCount].TypeText(null);
    }

    public void FlipPage()
    {
        if (!typingTexts[_pageCount].FlipedText) return;
        if (_pageCount < prologPages.Length - 1)
        {
            prologPages[_pageCount++].SetActive(false);
            prologPages[_pageCount].SetActive(true);

            if (typingTexts[_pageCount]!= null) typingTexts[_pageCount].TypeText(null);
        }
        else
        {
            gameObject.SetActive(false);
            AudioManager.Instance.FadeIn(onEndPrologAudio);
        }

    }

    public void SkipPages()
    {
        gameObject.SetActive(false);
        AudioManager.Instance.FadeIn(onEndPrologAudio);
    }
}
