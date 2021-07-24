using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Header("�ξ�")]
    public Mermaid mermaid;
    [Header("json����")]
    public TextAsset jsonFile;

    private Data.StageJson[] stage;

    private int mermaidCount;
    private int day = 0;    //0�������� 29�������� 30��
    private bool IsGuest = false;
    private int hp = 3;
    private float time = 0;

    Data.ICE selectedIce;
    Data.SYRUP selectedSyrup;
    Data.TOPPING selectedTopping;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (jsonFile)
            stage = JsonUtility.FromJson<Data.Stage>("{\"stage\":" + jsonFile.text + "}").stage;
        else
            Debug.LogError("Json������ �������� ����");
    }

    private void FixedUpdate()
    {
        if (IsGuest)
        {
            time += Time.fixedDeltaTime;
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
        mermaidCount = stage[day].mermaidCount;
        mermaid.bingsuCount = stage[day].IceCount;
        // UIâ������ Day + 1�� ���
        StartCoroutine("guestCome");
    }
    void CloseStore()
    {
        //Scene ����
        //����
        //���� �ʱ�ȭ ��
        day++;
        if(day >= 30)
        {
            //���� ����
        }
    }

    IEnumerator guestCome()
    {

        WaitWhile waitWhile = new WaitWhile(() => IsGuest);
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        int index = 0;
        while (mermaidCount > index)    //Day ���� ����, json ��ġ�� ����
        {
            IsGuest = true;
            //UI(���� ���� ��) �ʱ�ȭ
            time = 0;
            mermaid.Setting(day);
            //�մ� �̹��� Ȱ��ȭ �� ��������Ʈ (�մ� ����)����, ���ϴ� ���� ���� - mermaid.Setting()���� 
            //������ ������ �ʱ�ȭ
            yield return waitWhile;
            yield return waitForSeconds;    //��� �մ��� ���� ������ ���
            index++;
        }
        CloseStore();
    }

    // ���� ���� �� UI ����
    void SelectIce()
    {
        //selectedIce = ???
    }

    // �÷� ���� �� UI ����
    void SelectSyrup()
    {
        //selectedSyrup = ???
    }

    // ���� ���� �� UI ����
    void SelectTopping()
    {
        //selectedTopping = ???
    }

    //�� ������ UI �����ϸ� �˴ϴ�
    void ringBell()
    {
        for(int i = 0; i < mermaid.bingsuCount; i++)
            if (selectedIce != mermaid.ice || selectedSyrup != mermaid.syrup || selectedTopping != mermaid.topping)
            {
                MermaidExit(false);
                return;
            }
        MermaidExit(true);

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
                mermaid.SetExpression(Mermaid.EXPRESSION.ANGRY);
            }
            else if (time > 8)
            {
                //���
                // ���� ����, ǥ��
                mermaid.SetExpression(Mermaid.EXPRESSION.IDLE);
            }
            else
            {
                //���̾�
                // ���� ����, ǥ��

                if (hp >= 3)
                {
                    //���� ����
                }
                else
                {
                    hp++;
                }
                mermaid.SetExpression(Mermaid.EXPRESSION.HAPPY);
            }
        }
        else
        {
            hp--;
            //HP UI ������Ʈ
            if (hp == 0)
                End();
        }
        IsGuest = false;
    }

    void CalculReward()
    {
        //mermaid.bingsuCount
        //selectedIce ������ ���� ����
    }

    void End()
    {
        //���� ��
    }
}
