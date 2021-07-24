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

    [Header("인어")]
    public Mermaid mermaid;
    [Header("json파일")]
    public TextAsset jsonFile;

    public Data.StageJson[] stage;

    private int mermaidCount;
    public int day = 0;    //0일차부터 29일차까지 30일
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
            Debug.LogError("Json파일이 존재하지 않음");
    }

    private void FixedUpdate()
    {
        if (IsGuest)
        {
            time += Time.fixedDeltaTime;
            if (time > 15)
            {
                // 열린 UI(선택 창) 초기화
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

    //Open UI 연결하면 됩니다
    public void OpenStore()
    {
        SetLevel();

        //mermaidCount = stage[day].mermaidCount;
        //mermaid.bingsuCount = stage[day].IceCount;
        // UI창에서는 Day + 1로 계산
        
        
        StartCoroutine("guestCome");
    }
    void CloseStore()
    {
        //Scene 변경
        //정산
        //각종 초기화 등
        day++;
        if(day >= 30)
        {
            //해피 엔딩
        }
    }

    IEnumerator guestCome()
    {

        WaitWhile waitWhile = new WaitWhile(() => IsGuest);
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        int index = 0;
        while (mermaidCount > index)    //Day 종료 조건, json 수치에 따라
        {
            //손님 이미지 활성화 및 스프라이트 (손님 종류)변경, 빙수 개수, 원하는 빙수 변경 
            mermaid.Setting(day);

            //손님 들어옴, 타이머 시작
            IsGuest = true;
            time = 0;

            //UI(쌓인 빙수 등) 초기화
            //선택한 아이템 초기화
            yield return waitWhile;
            yield return waitForSeconds;    //잠시 손님이 가기 직전에 대기
            index++;
        }
        CloseStore();
    }

    // 얼음 선택 시 UI 연결
    void SelectIce()
    {
        //selectedIce = ???
    }

    // 시럽 선택 시 UI 연결
    void SelectSyrup()
    {
        //selectedSyrup = ???
    }

    // 토핑 선택 시 UI 연결
    void SelectTopping()
    {
        //selectedTopping = ???
    }

    //종 누르는 UI 연결하면 됩니다
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

    // 인어 퇴장 시
    public void MermaidExit(bool isSueccess)
    {
        //인어 초기화
        mermaid.gameObject.SetActive(false);
        if (isSueccess)
        {
            if (time > 10)
            {
                //진주
                // 보석 증가, 표정
                mermaid.SetExpression(Mermaid.EXPRESSION.ANGRY);
            }
            else if (time > 8)
            {
                //루비
                // 보석 증가, 표정
                mermaid.SetExpression(Mermaid.EXPRESSION.IDLE);
            }
            else
            {
                //다이아
                // 보석 증가, 표정

                if (hp >= 3)
                {
                    //점수 증가
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
            //HP UI 업데이트
            if (hp == 0)
                End();
        }
        IsGuest = false;
    }

    void CalculReward()
    {
        //mermaid.bingsuCount
        //selectedIce 종류에 따라 증가
    }

    void End()
    {
        //엔딩 씬
    }
}
