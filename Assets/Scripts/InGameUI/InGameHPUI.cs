using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHPUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _hpObjects;

    public void SetHp(int hp)
    {
        for (int i = 0; i < hp; i++)
        {
            _hpObjects[i].gameObject.SetActive(true);
        }

        for (int i = hp; i < _hpObjects.Count; i++)
        {
            _hpObjects[i].gameObject.SetActive(false);
        }
    }
}
