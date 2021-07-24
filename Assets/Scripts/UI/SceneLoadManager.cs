using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public Animator crossFade;
    public float transitionTime = 1f;
    
    private static readonly int Start1 = Animator.StringToHash("Start");

    private void Start()
    {
        crossFade = GetComponentInChildren<Animator>();
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        crossFade.SetTrigger(Start1);
        
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
