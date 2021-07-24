using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    InGame,
    Ending,
}

public class SceneLoadManager : PersistentSingleton<SceneLoadManager>
{
    [SerializeField]
    private Animator crossFade;
    [SerializeField]
    private float transitionTime = 1f;
    
    private static readonly int StartTrigger = Animator.StringToHash("Start");
    private static readonly int EndTrigger = Animator.StringToHash("End");

    private static readonly string TITLE_SCENE_NAME = "PrologAndTitle";
    private static readonly string INGAME_SCENE_NAME = "InGame";
    private static readonly string ENDING_SCENE_NAME = "Ending";

    public void MoveScene(SceneType sceneType)
    {
        var sceneName = GetSceneName(sceneType);
        StartCoroutine(LoadScene(sceneName));
    }

    private static string GetSceneName(SceneType sceneType)
    {
        switch(sceneType)
        {
            case SceneType.Title:
                return TITLE_SCENE_NAME;
            case SceneType.InGame:
                return INGAME_SCENE_NAME;
            case SceneType.Ending:
                return ENDING_SCENE_NAME;
        }
        return string.Empty;
    }

    IEnumerator LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        crossFade.SetTrigger(StartTrigger);
        
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
        
        crossFade.SetTrigger(EndTrigger);
    }
}
