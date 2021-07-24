using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        var mainCamera = GetComponent<Camera>();
        var rect = mainCamera.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // (가로 / 세로)
        float scaleWidth = 1f / scaleHeight;
        if (scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }
        mainCamera.rect = rect;
    }
    
    
    void OnPreCull() => GL.Clear(true, true, Color.black);
}
