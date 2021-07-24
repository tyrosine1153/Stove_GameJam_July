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
    [Header("인어마다 각각의 표정을 idle, impassive, disappointed, angry, happy 순서대로 Sprite를 넣으세요")]
    [SerializeField]
    private List<ExpressionSprites> mermaidSpriteList;
    public Image image;

    public Bingsu[] orderedBingsus = new Bingsu[2];
    public bool[] isOrderSatisfied = new bool[2];

    public bool IsAllOrderSatisfied
    {
        get
        {
            for (int i = 0; i < orderedBingsuCount; i++)
            {
                if (!isOrderSatisfied[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

    // 주문한 빙수 개수
    public int orderedBingsuCount;

    private int mermaidIndex;

    public enum EXPRESSION { IDLE, IMPASSIVE, DISAPPOINTED, ANGRY, HAPPY }

    void Start()
    {
        if (!gameObject.TryGetComponent(out image))
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다");

        orderedBingsus[0] = new Bingsu();
        orderedBingsus[1] = new Bingsu();
    }

    public bool CompareBingsu(Bingsu servedBingsu, out int satisfiedBingsuIndex)
    {
        for (int i = 0; i < orderedBingsuCount; i++)
        {
            if (!isOrderSatisfied[i] && orderedBingsus[i].Equals(servedBingsu))
            {
                satisfiedBingsuIndex = i;
                return true;
            }
        }

        Debug.Log("올바른 빙수 제공에 실패함.");
        satisfiedBingsuIndex = -1;
        return false;
    }

    public List<Bingsu> GetSatisfiedBingsus()
    {
        List<Bingsu> bingsus = new List<Bingsu>();
        for (int i = 0; i < orderedBingsuCount; i++)
        {
            if (isOrderSatisfied[i])
            {
                bingsus.Add(orderedBingsus[i]);
            }
        }
        return bingsus;
    }

    public List<Bingsu> GetNotSatisfiedBingsus()
    {
        List<Bingsu> bingsus = new List<Bingsu>();
        for (int i = 0; i < orderedBingsuCount; i++)
        {
            if (!isOrderSatisfied[i])
            {
                bingsus.Add(orderedBingsus[i]);
            }
        }
        return bingsus;
    }

    private void ResetBingsu()
    {
        for (int i = 0; i < 2; i++)
        {
            orderedBingsus[i].Reset();
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
            orderedBingsuCount = 1;
        else
            orderedBingsuCount = 2;

        mermaidIndex = Random.Range(0, mermaidSpriteList.Count);
        SetExpression(EXPRESSION.IDLE);


        // 해금된 재료에 따라 ice, 시럽, topping 등 선택
        for(int i = 0; i < orderedBingsuCount; i++)
        {
            var ice = StageManager.instance.IngredientUnlockData.GetRandomIce();
            var syrup = StageManager.instance.IngredientUnlockData.GetRandomSyrup();
            var topping = StageManager.instance.IngredientUnlockData.GetRandomTopping();
            orderedBingsus[i] = new Bingsu(ice, syrup, topping);
            isOrderSatisfied[i] = false;

            Debug.Log($"Order [{i}]: {orderedBingsus[i]}");
        }
    }

    public void SetExpression(EXPRESSION expression)
    {
        image.sprite = mermaidSpriteList[mermaidIndex].expression[(int)expression];
    }
}
