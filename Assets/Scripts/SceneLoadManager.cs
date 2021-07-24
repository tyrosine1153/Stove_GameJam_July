using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : PersistentSingleton<SceneLoadManager>
{
    [SerializeField]
    private Animator crossFade;
    [SerializeField]
    private float transitionTime = 1f;
    
    private static readonly int StartTrigger = Animator.StringToHash("Start");
    private static readonly int EndTrigger = Animator.StringToHash("End");

    private void Start()
    {
        crossFade = GetComponentInChildren<Animator>();
    }

    public void LoadTitleScene()
    {
        StartCoroutine(LoadScene(0));

    }
    
    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        crossFade.SetTrigger(StartTrigger);
        
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
        
        crossFade.SetTrigger(EndTrigger);
    }
}
