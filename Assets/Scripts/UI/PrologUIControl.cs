using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PrologUIControl : MonoBehaviour
{
    private PlayableDirector _playableDirector;
    private void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public void StopProlog()
    {
        _playableDirector.time = 540;
    }
}

