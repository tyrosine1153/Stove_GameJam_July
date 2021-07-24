using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text costTxt;
    public Image lockImg;
    public GameObject costBtn;

    private bool isShaking = false;
    private bool isLock = false;

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
        this.isLock = isLock;
        lockImg.enabled = this.isLock;
        if (cost == 0)
        {
            costBtn.SetActive(false);
        }
        else
        {
            costTxt.text = cost.ToString() + " ¿ø";
        }
    }

    public void buyItem()
    {
        if (!isShaking)
            StartCoroutine(Shake());

    }

    protected IEnumerator Shake()
    {
        isShaking = true;
        int count = 3;
        Vector3 movement = new Vector3(5, 0, 0);

        while (count-- > 0)
        {
            this.transform.position += movement;
            yield return new WaitForSeconds(0.1f);
            this.transform.position -= movement;

            yield return new WaitForSeconds(0.1f);
        }
        isShaking = false;


    }
}
