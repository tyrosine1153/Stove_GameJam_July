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
    [Header("인어마다 각각의 표정을 HAPPY(Idle), impassive, disappointed, angry 순서대로 Sprite를 넣으세요")]
    [SerializeField]
    private List<ExpressionSprites> mermaidSpriteList;
    public Image image;

    public Data.ICE[] ice;
    public Data.SYRUP[] syrup;
    public Data.TOPPING[] topping;

    public int bingsuCount; //빙수 개수

    private int mermaidIndex;

    public enum EXPRESSION { HAPPY, IMPASSIVE, DISAPPOINTED, ANGRY }

    void Start()
    {
        if (!gameObject.TryGetComponent(out image))
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다");
        ice = new Data.ICE[2];
        syrup = new Data.SYRUP[2];
        topping = new Data.TOPPING[2];
    }

    public bool CompareBingsu(Data.ICE selectedIce, Data.SYRUP selectedSyrup, Data.TOPPING selectedTopping)
    {
        if (selectedIce == ice[0] && selectedSyrup == syrup[0] && selectedTopping == topping[0])
        {
            //UI update remove wanted bingsu
            return true;
        }
        else if (selectedIce == ice[1] && selectedSyrup == syrup[1] && selectedTopping == topping[1])
        {
            //UI update remove wanted bingsu
            return true;
        }
        else
        {
            Debug.Log("실패");
            return false;
        }
    }
    private void ResetBingsu()
    {
        for (int i = 0; i < 2; i++)
        {
            ice[i] = Data.ICE.NONE;
            syrup[i] = Data.SYRUP.NONE;
            topping[i] = Data.TOPPING.NONE;
        }
    }
    // level에 따라 난이도 증가
    public void Setting(int day)
    {
        ResetBingsu();
        image.enabled = true;
        // 빙수 개수
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
        SetExpression(EXPRESSION.HAPPY);


        // 해금된 재료에 따라 ice, 시럽, topping 등 선택
        for(int i = 0; i < bingsuCount; i++)
        {
            ice[i] = StageManager.instance.IngredientUnlockData.GetRandomIce();
            syrup[i] = StageManager.instance.IngredientUnlockData.GetRandomSyrup();
            topping[i] = StageManager.instance.IngredientUnlockData.GetRandomTopping();
            Debug.Log(ice[i]);
            Debug.Log(syrup[i]);
            Debug.Log(topping[i]);
        }
    }

    public void SetExpression(EXPRESSION expression)
    {
        image.sprite = mermaidSpriteList[mermaidIndex].expression[(int)expression];
    }
}
