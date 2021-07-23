using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIControl : MonoBehaviour
{
    private Text _startText;

    private void Start()
    {
        _startText = gameObject.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        StartCoroutine(nameof(Fade));
    }

    public void LoadReadyScene()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator Fade()
    {
        const float fade = 1f;
        float f = 1f;
        bool isFadeIn = true;
        while (true)
        {
            var c = _startText.color;

            if (isFadeIn)
            {
                f -= Time.deltaTime / fade;
                if (f <= 0)
                {
                    isFadeIn = false;
                }
            }
            else
            {
                f += Time.deltaTime / fade;
                if (f > 1)
                {
                    isFadeIn = true;
                }
            }

            c.a = f;
            _startText.color = c;
            yield return null;
        }
    }
}

