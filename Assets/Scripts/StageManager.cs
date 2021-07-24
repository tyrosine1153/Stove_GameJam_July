using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum InGameState
{
    Closed,
    Playing,
}

public enum OrderResult
{
    Fail,
    Pearl,
    Ruby,
    Diamond,
}

public class StageManager : MonoBehaviour, IStageManager
{
    public static StageManager instance;

    private static readonly int MAX_HP = 3;

    [Header("인어")]
    public Mermaid mermaid;

    [Header("json파일")]
    public TextAsset jsonFile;

    public InGameUI inGameUI;
    public Data.StageJson[] stage;

    private int mermaidCount;
    public int day = 0;    //0일차부터 29일차까지 30일
    private bool IsGuest = false;
    private float time = 0;
    private int gold = 0;
    private int dailyScore = 0;
    private int dailyGold = 0;

    private int hp = 3;
    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp > MAX_HP)
            {
                hp = MAX_HP;
            }
            else if (hp < 0)
            {
                hp = 0;
            }

            inGameUI.SetHp(hp);
            if (hp == 0)
            {
                End();
            }
        }
    }

    // 지금 주문을 완료했을때 받게될 결과. (다이아몬드, 루비, 진주)
    private OrderResult expectedResult;

    private InGameState currentState = InGameState.Closed;
    public InGameState CurrentState
    {
        get => currentState;
    }

    private IngredientUnlockData ingredientUnlockData;
    public IngredientUnlockData IngredientUnlockData => ingredientUnlockData;

    private Bingsu selectedBingsu = new Bingsu();

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
            if (time < 8)
            {
                expectedResult = OrderResult.Diamond;
                mermaid.SetExpression(Mermaid.EXPRESSION.HAPPY);
            }
            else if (time < 10)
            {
                expectedResult = OrderResult.Ruby;
                mermaid.SetExpression(Mermaid.EXPRESSION.IMPASSIVE);
            }
            else if (time < 15)
            {
                expectedResult = OrderResult.Pearl;
                mermaid.SetExpression(Mermaid.EXPRESSION.DISAPPOINTED);
            }
            else
            {
                expectedResult = OrderResult.Fail;
                OrderFailed();
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
    }

    public void OpenStore()
    {
        dailyGold = 0;
        dailyScore = 0;
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
        gold += dailyGold;

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
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);
        int index = 0;
        while (mermaidCount > index)    //Day ???? ????, json ?????? ????
        {
            yield return waitForSeconds;    //손님 오기까지 대기 시간
            //손님 이미지 활성화 및 스프라이트 (손님 종류)변경, 빙수 개수, 원하는 빙수 변경
            mermaid.Setting(day);

            //손님 들어옴, 타이머 시작
            IsGuest = true;

            time = 0;

            //UI(쌓인 빙수 등) 초기화
            //선택한 아이템 초기화

            yield return waitWhile;
            yield return waitForSeconds;   //잠시 손님이 가기 직전에 대기
            mermaid.image.enabled = false;
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
        if (selectedBingsu.Ice == Data.ICE.NONE)
        {
            selectedBingsu.Ice = iceType;
            inGameUI.SetResultBingsu(selectedBingsu);
        }
    }

    public void SelectSyrup(Data.SYRUP syrupType)
    {
        if (currentState != InGameState.Playing)
        {
            return;
        }

        // 얼음이 선택되어 있지 않다면, 선택할 수 없다.
        if (selectedBingsu.Ice == Data.ICE.NONE)
        {
            return;
        }

        // 시럽잇 선택되어 있지 않다면, 이 시럽 종류를 선택한다.
        if (selectedBingsu.Syrup == Data.SYRUP.NONE)
        {
            selectedBingsu.Syrup = syrupType;
            inGameUI.SetResultBingsu(selectedBingsu);
        }
        // 시럽을 더 선택할 수 있다면 (섞을 수 았다면), 섞은 시럽을 선택한다.
        else if (SyrupColorMix.TryGetMixedSyrup(selectedBingsu.Syrup, syrupType, out var resultSyrup))
        {
            selectedBingsu.Syrup = resultSyrup;
            inGameUI.SetResultBingsu(selectedBingsu);
        }
    }

    public void SelectTopping(Data.TOPPING toppingType)
    {
        if (currentState != InGameState.Playing)
        {
            return;
        }

        // 얼음이 선택되어 있지 않다면, 선택할 수 없다.
        if (selectedBingsu.Ice == Data.ICE.NONE)
        {
            return;
        }

        // 토핑이 선택되어 있지 않다면, 이 토핑 종류를 선택한다.
        if (selectedBingsu.Topping == Data.TOPPING.NONE)
        {
            selectedBingsu.Topping = toppingType;
            inGameUI.SetResultBingsu(selectedBingsu);
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

        var result = ingredientUnlockData.UnlockIce(iceType);
        inGameUI.UpdateSelectIngredientUI();
        return result;
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

        var result = ingredientUnlockData.UnlockTopping(toppingType);
        inGameUI.UpdateSelectIngredientUI();
        return result;
    }

    private void UpdateInGameUI()
    {
        //inGameUI.SetScore(dailyScore);
        inGameUI.SetGold(gold);
        inGameUI.SetDay(day);
        inGameUI.SetHp(hp);
        inGameUI.SetState(currentState);
        inGameUI.UpdateSelectIngredientUI();
        inGameUI.SetResultBingsu(selectedBingsu);
    }

    public void ServeBingsu()
    {
        if (currentState != InGameState.Playing)
        {
            return;
        }


        // 일치하는 빙수가 없다면, 실패한다.
        if (!mermaid.CompareBingsu(selectedBingsu, out var satisfiedBingsuIndex))
        {
            OrderFailed();
            return;
        }

        mermaid.isOrderSatisfied[satisfiedBingsuIndex] = true;
        ResetBingsu();

        if (mermaid.IsAllOrderSatisfied)
        {
            OrderSucceessed();
        }
    }

    // 인어 퇴장 시
    public void MermaidExit()
    {
        //인어 초기화
        ResetBingsu();
        IsGuest = false;
    }


    private void DeliverDailyRewards(List<Bingsu> bingsus, OrderResult result)
    {
        // 빙수 가격을 계산한다.
        int totalBingsuPrice = 0;
        foreach (var bingsu in bingsus)
        {
            totalBingsuPrice += bingsu.CalculatePrice();
        }

        // 추가 보상을 계산한다.
        int jewelPrice = 0;
        int score = 0;
        switch(result)
        {
            case OrderResult.Pearl:
                jewelPrice = 20;
                score = 100;
                break;
            case OrderResult.Ruby:
                jewelPrice = 50;
                score = 250;
                break;
            case OrderResult.Diamond:
                jewelPrice = 100;
                score = 500;
                break;
        }

        // 다이아몬드일 경우 HP를 회복하거나, 보너스 점수르 얻는다.
        if (result == OrderResult.Diamond)
        {
            if (Hp == MAX_HP)
            {
                score += 550;
            }
            else
            {
                Hp += 1;
            }
        }

        AddDailyGold(totalBingsuPrice + jewelPrice);
        AddDailyScore(score);

        Debug.Log($"결과: {result}, 획득 골드: {totalBingsuPrice + jewelPrice}, 획득 점수: {score}");
    }

    private void OrderFailed()
    {
        ResetBingsu();
        DeliverDailyRewards(mermaid.GetSatisfiedBingsus(), OrderResult.Fail);
        mermaid.SetExpression(Mermaid.EXPRESSION.ANGRY);
        --Hp;
        MermaidExit();
    }

    private void OrderSucceessed()
    {
        ResetBingsu();
        DeliverDailyRewards(mermaid.GetSatisfiedBingsus(), expectedResult);
        MermaidExit();
    }

    private void AddDailyGold(int money)
    {
        dailyGold += money;
    }

    private void AddDailyScore(int score)
    {
        dailyScore += score;
        //UI Update
    }

    private void ResetBingsu()
    {
        selectedBingsu.Reset();
        inGameUI.SetResultBingsu(selectedBingsu);
    }

    void End()
    {
        //엔딩 씬
    }
}
