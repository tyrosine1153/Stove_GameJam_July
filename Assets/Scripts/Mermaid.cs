using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ExpressionSprites
{
    public Sprite[] expression;
}

public class Mermaid : MonoBehaviour
{
    [Header("???? ??? ??? ??(?), ??, ?? ???? Sprite? ????")]
    [SerializeField]
    private List<ExpressionSprites> mermaidSpriteList;
    private Image image;

    public Data.ICE ice;
    public Data.SYRUP syrup;
    public Data.TOPPING topping;

    public int bingsuCount; //?? ??

    private int mermaidIndex;

    public enum EXPRESSION { IDLE, HAPPY, ANGRY }

    void Start()
    {
        if (!gameObject.TryGetComponent(out image))
            Debug.LogError("Image ?????????? ???? ?? ????????");
    }

    // level? ?? ??? ??
    public void Setting(int day)
    {
        //?? ??
        int day_index = 0;
        for (int i = 0; i < StageManager.instance.stage.Length; i++)
        {
            if (day + 1 < StageManager.instance.stage[i].Day)
            {
                day_index = i;
                break;
            }
        }
        int selectNum = Random.Range(1, 101);
        if (StageManager.instance.stage[day_index].ice_one >= selectNum)
            bingsuCount = 1;
        else
            bingsuCount = 2;

        mermaidIndex = Random.Range(0, mermaidSpriteList.Count);
        SetExpression(EXPRESSION.IDLE);


        //??? ??? ?? ice, ??, topping ? ??
    }

    public void SetExpression(EXPRESSION expression)
    {
        image.sprite = mermaidSpriteList[mermaidIndex].expression[((int)expression)];
    }
}
