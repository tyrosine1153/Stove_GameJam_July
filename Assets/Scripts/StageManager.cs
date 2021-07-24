using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageManager))]
public class DuckGenerateButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StageManager generator = (StageManager)target;
        if (GUILayout.Button("Open Store"))
        {
            for(int i = 0; i < 1000; i++)
                generator.OpenStore();
        }
    }
}
public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Header("�ξ�")]
    public Mermaid mermaid;
    [Header("json����")]
    public TextAsset jsonFile;

    public Data.StageJson[] stage;

    private int mermaidCount;
    public int day = 0;    //0�������� 29�������� 30��
    private bool IsGuest = false;
    private int hp = 3;
    private float time = 0;

    public int one = 0;
    public int two = 0;
    public int three = 0;
    public int four = 0;
    public int five = 0;

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

    void SetLevel()
    {
        int total = 0;
        int day_index = 0;
        for (int i = 0; i < stage.Length; i++)
        {
            if (day + 1 < stage[i].Day)
            {
                day_index = i;
                break;
            }
        }

        List<int> list = new List<int>();
        list.Add(stage[day_index].mermaid_one);
        list.Add(stage[day_index].mermaid_two);
        list.Add(stage[day_index].mermaid_three);
        list.Add(stage[day_index].mermaid_four);
        list.Add(stage[day_index].mermaid_five);
        int selectNum = Random.Range(1, 101);

        List<int> index = new List<int>();
        index.Add(0);
        index.Add(1);
        index.Add(2);
        index.Add(3);
        index.Add(4);

        while (index.Count > 0)
        {
            int rnd = Random.Range(0, index.Count);
            total += list[index[rnd]];
            if (total >= selectNum)
            {
                mermaidCount = index[rnd] + 1;
                break;
            }
            index.RemoveAt(rnd);
        }

        //For Debuging
        switch (mermaidCount)
        {
            case 1:
                one++;
                break;
            case 2:
                two++;
                break;
            case 3:
                three++;
                break;
            case 4:
                four++;
                break;
            case 5:
                five++;
                break;

        }
    }

    //Open UI �����ϸ� �˴ϴ�
    public void OpenStore()
    {
        SetLevel();

        //mermaidCount = stage[day].mermaidCount;
        //mermaid.bingsuCount = stage[day].IceCount;
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
            //�մ� �̹��� Ȱ��ȭ �� ��������Ʈ (�մ� ����)����, ���� ����, ���ϴ� ���� ���� 
            mermaid.Setting(day);

            //�մ� ����, Ÿ�̸� ����
            IsGuest = true;
            time = 0;

            //UI(���� ���� ��) �ʱ�ȭ
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
