using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text costTxt;
    public Image lockImg;
    public GameObject costBtn;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void setItem(int cost, bool isLock)
    {
        lockImg.enabled = isLock;
        if (cost == 0)
        {
            costBtn.SetActive(false);
        }
        else
        {
            costTxt.text = cost.ToString() + " ¿ø";
        }
    }
}
