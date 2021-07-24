using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameOrderUI : MonoBehaviour
{
    [SerializeField]
    private List<ResultBingsuUI> _resultBingsuUIs;

    public void SetBingsus(List<Bingsu> bingsus)
    {
        for (int i = 0; i < bingsus.Count; i++)
        {
            _resultBingsuUIs[i].gameObject.SetActive(true);
            _resultBingsuUIs[i].SetResult(bingsus[i]);
        }

        for (int i = bingsus.Count; i < _resultBingsuUIs.Count; i++)
        {
            _resultBingsuUIs[i].gameObject.SetActive(false);
        }
    }
}
