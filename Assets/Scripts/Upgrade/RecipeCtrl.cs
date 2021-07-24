using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCtrl : MonoBehaviour
{
    public List<GameObject> recipes;
    public GameObject LBtn;
    public GameObject RBtn;
    public GameObject options;
    int nowIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        recipes[0].SetActive(true);
        for(int idx = 1;idx < recipes.Count;idx++)
        {
            recipes[idx].SetActive(false);
        }
        LBtn.SetActive(false);
        RBtn.SetActive(true);
    }

    public void openRecipe()
    {
        this.gameObject.SetActive(true);
        options?.SetActive(false);
    }

    public void closeRecipe()
    {
        this.gameObject.SetActive(false);
        options?.SetActive(true);
    }

    public void clickLeftBtn()
    {
        recipes[nowIndex].SetActive(false);
        nowIndex--;
        recipes[nowIndex].SetActive(true);

        if (nowIndex == 0)
        {
            LBtn.SetActive(false);
        }
        else
        {
            RBtn.SetActive(true);
        }
    }

    public void clickRightBtn()
    {
        recipes[nowIndex].SetActive(false);
        nowIndex++;
        recipes[nowIndex].SetActive(true);

        if (nowIndex == recipes.Count - 1)
        {
            RBtn.SetActive(false);
        }
        else
        {
            LBtn.SetActive(true);
        }
    }
}
