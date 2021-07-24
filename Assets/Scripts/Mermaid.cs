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
    [Header("�ξ�� ������ ǥ���� �Ϲ�(��), ����, ȭ�� ������� Sprite�� ��������")]
    [SerializeField]
    private List<ExpressionSprites> mermaidSpriteList;
    private Image image;

    public Data.ICE ice;
    public Data.SYRUP syrup;
    public Data.TOPPING topping;

    public int bingsuCount; //���� ����

    private int mermaidIndex;

    public enum EXPRESSION { IDLE, HAPPY, ANGRY }

    void Start()
    {
        if (!gameObject.TryGetComponent(out image))
            Debug.LogError("Image ������Ʈ�� ã�� �� �����ϴ�");
    }

    // level�� ���� ���̵� ����
    public void Setting(int level)
    {
        //���� 
        mermaidIndex = Random.Range(0, mermaidSpriteList.Count);
        SetExpression(EXPRESSION.IDLE);
        //Level�� ���� ice, topping ����
    }

    public void SetExpression(EXPRESSION expression)
    {
        image.sprite = mermaidSpriteList[mermaidIndex].sprites[((int)expression)];
    }
}
