using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIControl : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneLoadManager.Instance.MoveScene(SceneType.InGame);
        AudioManager.Instance.FadeOut();
    }
}
