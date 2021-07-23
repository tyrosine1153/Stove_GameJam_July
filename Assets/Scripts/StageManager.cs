using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageJson
{
    public int mermaidCount;
    public int IceCount;
}


public class StageManager : MonoBehaviour
{
    [Header("�ξ�")]
    public Mermaid mermaid;
    [Header("json����")]
    public TextAsset jsonFile;

    private int day = 0;
    private bool IsGuest = false;
    private int hp = 3;
    private float time = 0;

    Data.ICE selectedIce;
    Data.SYRUP selectedSyrup;
    Data.TOPPING selectedTopping;

    void Start()
    {
        
    }

    private void Update()
    {
        if (IsGuest)
        {
            time += Time.deltaTime;
            if (time > 15)
            {
                // ���� UI(���� â) �ʱ�ȭ
                MermaidExit(false);
            }
        }
    }

    //Open UI �����ϸ� �˴ϴ�
    void OpenStore()
    {
        day++;
        StartCoroutine("guestCome");
    }

    IEnumerator guestCome()
    {
        while (true)    //Day ���� ����, json ��ġ�� ����
        {
            IsGuest = true;
            //UI(���� ���� ��) �ʱ�ȭ
            time = 0;
            //�մ� �̹��� Ȱ��ȭ �� ����, ���ϴ� ���� ����
            //�°� ������ �ʱ�ȭ
            yield return new WaitWhile(() => IsGuest);
        }
    }

    // ���� ���� �� UI ����
    void SelectIce()
    {
        //selectedIce = 
    }

    // �÷� ���� �� UI ����
    void SelectSyrup()
    {
        //selectedSyrup = 
    }

    // ���� ���� �� UI ����
    void SelectTopping()
    {
        //selectedTopping = 
    }

    //�� ������ UI �����ϸ� �˴ϴ�
    void ringBell()
    {
        if (selectedIce == mermaid.ice && selectedSyrup == mermaid.syrup && selectedTopping == mermaid.topping)  //�����ϴٸ�
        {
            MermaidExit(true);
        }
        else
        {
            MermaidExit(false);
        }
    }

    // �ξ� ���� ��
    public void MermaidExit(bool isSueccess)
    {
        //�ξ� �ʱ�ȭ
        mermaid.gameObject.SetActive(false);
        if (isSueccess)
        {
            if (time > 10)
            {
                //����
                // ���� ����, ǥ��
            }
            else if (time > 8)
            {
                //���
                // ���� ����, ǥ��
            }
            else
            {
                //���̾�
                // ���� ����, ǥ��
            }
        }
        else
        {
            hp--;
            if (hp == 0)
                End();
        }
        IsGuest = false;
    }

    void End()
    {
        //���� ��
    }
}
