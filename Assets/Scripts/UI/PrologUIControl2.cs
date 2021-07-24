using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologUIControl2 : MonoBehaviour
{
    public GameObject[] prologPages;
    private int _pageCount;

    private void Start()
    {
        _pageCount = 0;
        prologPages[_pageCount].SetActive(true);
        for (int i = 1; i < prologPages.Length; i++)
        {
            prologPages[i].SetActive(false);
        }
    }

    public void FlipPage()
    {
        if (_pageCount < prologPages.Length - 1)
        {
            prologPages[_pageCount++].SetActive(false);
            prologPages[_pageCount].SetActive(true);
            
            var a = prologPages[_pageCount].GetComponentInChildren<PrologTypingText>();
            if (a != null) a.TypeText();
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }

    public void SkipPages()
    {
        gameObject.SetActive(false);
    }
}