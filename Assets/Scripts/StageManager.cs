using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum InGameState
{
    Closed,
    Playing,
}

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

public class StageManager : MonoBehaviour, IStageManager
{
    public static StageManager instance;

    [Header("인어")]
    public Mermaid mermaid;

    [Header("json파일")]
    public TextAsset jsonFile;

    public InGameUI inGameUI;
    public Data.StageJson[] stage;

    private int mermaidCount;
    public int day = 0;    //0일차부터 29일차까지 30일
    private bool IsGuest = false;
    private int hp = 3;
    private float time = 0;
    private int gold = 0;
    private int score = 0;

    private InGameState currentState = InGameState.Closed;
    public InGameState CurrentState
    {
        get => currentState;
    }

    private IngredientUnlockData ingredientUnlockData;
    public IngredientUnlockData IngredientUnlockData => ingredientUnlockData;

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

        var ingredientData = IngredientGameDataHolder.Instance.IngredientGameDatas;
        ingredientUnlockData = new IngredientUnlockData(
                ingredientData.GetAllFreeIces(),
                ingredientData.GetAllFreeSyrups(),
                ingredientData.GetAllFreeToppings()
            );

        ResetBingsu();
        UpdateInGameUI();
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
                // ???? UI(???? ??) ??????
                SubIce(false);
                MermaidExit();
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

    public void OpenStore()
    {
        SetLevel();

        currentState = InGameState.Playing;

        ResetBingsu();
        UpdateInGameUI();

        // UI창에서는 Day + 1로 계산
        StartCoroutine("guestCome");
    }

    void CloseStore()
    {
        //Scene 변경
        //정산
        //각종 초기화 등
        gold += score;

        currentState = InGameState.Closed;

        ResetBingsu();
        UpdateInGameUI();

        day++;
        inGameUI.SetDay(day);

        if (day >= 30)
        {
            //해피 엔딩
        }
    }

    IEnumerator guestCome()
    {

        WaitWhile waitWhile = new WaitWhile(() => IsGuest);
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        int index = 0;
        while (mermaidCount > index)    //Day ???? ????, json ?????? ????
        {
            //손님 이미지 활성화 및 스프라이트 (손님 종류)변경, 빙수 개수, 원하는 빙수 변경 
            mermaid.Setting(day);

            //손님 들어옴, 타이머 시작
            IsGuest = true;

            time = 0;

            //UI(쌓인 빙수 등) 초기화
            //선택한 아이템 초기화

            yield return waitWhile;
            yield return waitForSeconds;   //잠시 손님이 가기 직전에 대기
            index++;
        }
        CloseStore();
    }

    public void SelectIce(Data.ICE iceType)
    {
        if (currentState != InGameState.Playing)
        {
            return;
        }

        // 얼음이 선택되어 있지 않다면, 이 얼음 종류를 선택한다.
        if (selectedIce == Data.ICE.NONE)
        {
            selectedIce = iceType;
            inGameUI.SetResultBingsu(selectedIce, selectedSyrup, selectedTopping);
        }
    }

    public void SelectSyrup(Data.SYRUP syrupType)
    {
        if (currentState != InGameState.Playing)
        {
            return;
        }

        // 얼음이 선택되어 있지 않다면, 선택할 수 없다.
        if (selectedIce == Data.ICE.NONE)
        {
            return;
        }

        // 시럽잇 선택되어 있지 않다면, 이 시럽 종류를 선택한다.
        if (selectedSyrup == Data.SYRUP.NONE)
        {
            selectedSyrup = syrupType;
            inGameUI.SetResultBingsu(selectedIce, selectedSyrup, selectedTopping);
        }
        // 시럽을 더 선택할 수 있다면 (섞을 수 았다면), 섞은 시럽을 선택한다.
        else if (SyrupColorMix.TryGetMixedSyrup(selectedSyrup, syrupType, out var resultSyrup))
        {
            selectedSyrup = resultSyrup;
            inGameUI.SetResultBingsu(selectedIce, selectedSyrup, selectedTopping);
        }
    }

    public void SelectTopping(Data.TOPPING toppingType)
    {
        if (currentState != InGameState.Playing)
        {
            return;
        }

        // 얼음이 선택되어 있지 않다면, 선택할 수 없다.
        if (selectedIce == Data.ICE.NONE)
        {
            return;
        }

        // 토핑이 선택되어 있지 않다면, 이 토핑 종류를 선택한다.
        if (selectedTopping == Data.TOPPING.NONE)
        {
            selectedTopping = toppingType;
            inGameUI.SetResultBingsu(selectedIce, selectedSyrup, selectedTopping);
        }
    }

    public bool BuyIce(Data.ICE iceType)
    {
        if (ingredientUnlockData.IsIceUnlocked(iceType))
        {
            return false;
        }

        var cost = IngredientGameDataHolder.Instance.IngredientGameDatas.GetIceGameData(iceType).IngredientGameData.UnlockCost;
        if (gold < cost)
        {
            return false;
        }

        gold -= cost;
        inGameUI.SetGold(gold);
        return ingredientUnlockData.UnlockIce(iceType);
    }

    public bool BuyTopping(Data.TOPPING toppingType)
    {
        if (ingredientUnlockData.IsToppingUnlocked(toppingType))
        {
            return false;
        }

        var cost = IngredientGameDataHolder.Instance.IngredientGameDatas.GetToppingGameData(toppingType).IngredientGameData.UnlockCost;
        if (gold < cost)
        {
            return false;
        }

        gold -= cost;
        inGameUI.SetGold(gold);
        return ingredientUnlockData.UnlockTopping(toppingType);
    }

    private void UpdateInGameUI()
    {
        inGameUI.SetGold(gold);
        inGameUI.SetDay(day);
        inGameUI.SetHp(hp);
        inGameUI.SetState(currentState);
        inGameUI.UpdateSelectIngredientUI();
        inGameUI.SetResultBingsu(selectedIce, selectedSyrup, selectedTopping);
    }

    //종 누르는 UI 연결하면 됩니다
    void ServeBingsu()
    {
        if (currentState != InGameState.Playing)
        {
            return;
        }

        if (mermaid.CompareBingsu(selectedIce, selectedSyrup, selectedTopping))
        {
            SubIce(false);
            MermaidExit();
            return;
        }
        SubIce(true);
        if (mermaid.bingsuCount == 0)
        {
            MermaidExit();
        }

    }

    // 인어 퇴장 시
    public void MermaidExit()
    {
        //인어 초기화
        mermaid.gameObject.SetActive(false);
        ResetBingsu();
        IsGuest = false;
    }


    void SubIce(bool isSuccess)
    {
        if (isSuccess)
        {
            mermaid.bingsuCount--;
            switch (selectedIce)
            {
                case Data.ICE.SEAWATER:
                    score += 200;
                    break;
                case Data.ICE.WHITE_MILK:
                    score += 350;
                    break;
                case Data.ICE.MINTCHOCO_MILK:
                    score += 750;
                    break;
                case Data.ICE.CHOCO_MILK:
                    score += 1300;
                    break;
                case Data.ICE.STRAWBERRY_MILK:
                    score += 2700;
                    break;
            }
            switch (selectedTopping)
            {
                case Data.TOPPING.REDBEAN:
                    score += 200;
                    break;
                case Data.TOPPING.FRUIT_COCK:
                    score += 350;
                    break;
                case Data.TOPPING.LEMON:
                    score += 750;
                    break;
                case Data.TOPPING.CHOCOLATE:
                    score += 1300;
                    break;
                case Data.TOPPING.STRAWBERRY:
                    score += 2700;
                    break;
            }
            if (time > 10)
            {
                //진주
                // 보석 증가, 표정
                score += 100;
                mermaid.SetExpression(Mermaid.EXPRESSION.ANGRY);
            }
            else if (time > 8)
            {
                //루비
                // 보석 증가, 표정
                score += 250;
                mermaid.SetExpression(Mermaid.EXPRESSION.IDLE);
            }
            else
            {
                //다이아
                // 보석 증가, 표정
                score += 500;
                if (hp >= 3)
                    //점수 증가
                    score += 550;
                else
                    hp++;
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

    private void ResetBingsu()
    {
        selectedIce = Data.ICE.NONE;
        selectedSyrup = Data.SYRUP.NONE;
        selectedTopping = Data.TOPPING.NONE;
        inGameUI.SetResultBingsu(selectedIce, selectedSyrup, selectedTopping);
    }
    void End()
    {
        //엔딩 씬
    }
}
