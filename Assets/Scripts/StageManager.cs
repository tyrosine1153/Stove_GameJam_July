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
    [Header("인어")]
    public Mermaid mermaid;
    [Header("json파일")]
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
                // 열린 UI(선택 창) 초기화
                MermaidExit(false);
            }
        }
    }

    //Open UI 연결하면 됩니다
    void OpenStore()
    {
        day++;
        StartCoroutine("guestCome");
    }

    IEnumerator guestCome()
    {
        while (true)    //Day 종료 조건, json 수치에 따라
        {
            IsGuest = true;
            //UI(쌓인 빙수 등) 초기화
            time = 0;
            //손님 이미지 활성화 및 변경, 원하는 빙수 변경
            //온갖 아이템 초기화
            yield return new WaitWhile(() => IsGuest);
        }
    }

    // 얼음 선택 시 UI 연결
    void SelectIce()
    {
        //selectedIce = 
    }

    // 시럽 선택 시 UI 연결
    void SelectSyrup()
    {
        //selectedSyrup = 
    }

    // 토핑 선택 시 UI 연결
    void SelectTopping()
    {
        //selectedTopping = 
    }

    //종 누르는 UI 연결하면 됩니다
    void ringBell()
    {
        if (selectedIce == mermaid.ice && selectedSyrup == mermaid.syrup && selectedTopping == mermaid.topping)  //동일하다면
        {
            MermaidExit(true);
        }
        else
        {
            MermaidExit(false);
        }
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
            }
            else if (time > 8)
            {
                //루비
                // 보석 증가, 표정
            }
            else
            {
                //다이아
                // 보석 증가, 표정
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
        //엔딩 씬
    }
}
