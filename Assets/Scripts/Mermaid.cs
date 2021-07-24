using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ExpressionSprites
{
    public Sprite[] sprites;
}

public class Mermaid : MonoBehaviour
{
    [Header("인어마다 각각의 표정을 일반(무), 웃는, 화난 순서대로 Sprite를 넣으세요")]
    [SerializeField]
    private List<ExpressionSprites> mermaidSpriteList;
    private Image image;

    public Data.ICE ice;
    public Data.SYRUP syrup;
    public Data.TOPPING topping;

    public int bingsuCount; //빙수 개수

    private int mermaidIndex;

    public enum EXPRESSION { IDLE, HAPPY, ANGRY }

    void Start()
    {
        if (!gameObject.TryGetComponent(out image))
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다");
    }

    // level에 따라 난이도 증가
    public void Setting(int level)
    {
        //빙수 
        mermaidIndex = Random.Range(0, mermaidSpriteList.Count);
        SetExpression(EXPRESSION.IDLE);
        //Level에 따른 ice, topping 선택
    }

    public void SetExpression(EXPRESSION expression)
    {
        image.sprite = mermaidSpriteList[mermaidIndex].sprites[((int)expression)];
    }
}
