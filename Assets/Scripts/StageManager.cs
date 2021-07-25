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
    private static readonly int MAX_DAY = 20;

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

    private int currentGold = 0;
    private int currentScore = 0;
    private int highScore = 0;

    private int dailyScore = 0;
    private int dailyBingsuPrice = 0;
    private int dailyPearlCount = 0;
    private int dailyRubyCount = 0;
    private int dailyDiamondCount = 0;

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
                MoveEndingScene(false);
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

    private static readonly string HIGHSCORE_SAVE_KEY = "HighScore";

    private void Awake()
    {
        instance = this;

        AudioManager.Instance.PlayBgm(1);

        highScore = PlayerPrefs.GetInt(HIGHSCORE_SAVE_KEY, 0);

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
                mermaid.SetExpression(Mermaid.EXPRESSION.IDLE);
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
        AudioManager.Instance.PlaySfx(SfxType.Click);

        dailyBingsuPrice = 0;
        dailyPearlCount = 0;
        dailyRubyCount = 0;
        dailyDiamondCount = 0;
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
        OpenDailyResultUIPopup();

        var dailyGold = CalculateGold(dailyBingsuPrice, dailyPearlCount, dailyRubyCount, dailyDiamondCount);
        currentGold += dailyGold;
        currentScore += dailyScore;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HIGHSCORE_SAVE_KEY, highScore);
            PlayerPrefs.Save();
        }

        currentState = InGameState.Closed;

        ResetBingsu();
        UpdateInGameUI();

        day++;
        inGameUI.SetDay(day);

        if (day >= MAX_DAY)
        {
            MoveEndingScene(true);
        }
    }

    IEnumerator guestCome()
    {

        WaitWhile waitWhile = new WaitWhile(() => IsGuest);
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);
        int guestIndex = 0;
        inGameUI.SetGuest(mermaidCount - guestIndex, mermaidCount);
        while (mermaidCount > guestIndex)    //Day ???? ????, json ?????? ????
        {
            yield return waitForSeconds;    //손님 오기까지 대기 시간
            //손님 이미지 활성화 및 스프라이트 (손님 종류)변경, 빙수 개수, 원하는 빙수 변경
            AudioManager.Instance.PlaySfx(SfxType.Bell);
            mermaid.Setting(day);

            //손님 들어옴, 타이머 시작
            IsGuest = true;

            inGameUI.SetOrderUI(mermaid.GetNotSatisfiedBingsus());
            inGameUI.SetServeButtonAInteractive(true);

            time = 0;

            //UI(쌓인 빙수 등) 초기화
            //선택한 아이템 초기화

            yield return waitWhile;
            inGameUI.SetGuest(mermaidCount - guestIndex - 1, mermaidCount);
            yield return waitForSeconds;   //잠시 손님이 가기 직전에 대기

            mermaid.image.enabled = false;
            guestIndex++;
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
            AudioManager.Instance.PlaySfx(SfxType.Select);
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
            AudioManager.Instance.PlaySfx(SfxType.Select);
            selectedBingsu.Syrup = syrupType;
            inGameUI.SetResultBingsu(selectedBingsu);
        }
        // 시럽을 더 선택할 수 있다면 (섞을 수 았다면), 섞은 시럽을 선택한다.
        else if (SyrupColorMix.TryGetMixedSyrup(selectedBingsu.Syrup, syrupType, out var resultSyrup))
        {
            AudioManager.Instance.PlaySfx(SfxType.Select);
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
            AudioManager.Instance.PlaySfx(SfxType.Select);
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
        if (currentGold < cost)
        {
            return false;
        }

        currentGold -= cost;
        inGameUI.SetGold(currentGold);

        var result = ingredientUnlockData.UnlockIce(iceType);
        inGameUI.UpdateSelectIngredientUI();
        AudioManager.Instance.PlaySfx(SfxType.Unlock);
        return result;
    }

    public bool BuyTopping(Data.TOPPING toppingType)
    {
        if (ingredientUnlockData.IsToppingUnlocked(toppingType))
        {
            return false;
        }

        var cost = IngredientGameDataHolder.Instance.IngredientGameDatas.GetToppingGameData(toppingType).IngredientGameData.UnlockCost;
        if (currentGold < cost)
        {
            return false;
        }

        currentGold -= cost;
        inGameUI.SetGold(currentGold);

        var result = ingredientUnlockData.UnlockTopping(toppingType);
        inGameUI.UpdateSelectIngredientUI();
        AudioManager.Instance.PlaySfx(SfxType.Unlock);
        return result;
    }

    private void UpdateInGameUI()
    {
        inGameUI.SetGold(currentGold);
        inGameUI.SetDay(day);
        inGameUI.SetHp(hp);
        inGameUI.SetScore(currentScore, highScore);
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

        AudioManager.Instance.PlaySfx(SfxType.Success);
        mermaid.isOrderSatisfied[satisfiedBingsuIndex] = true;
        inGameUI.SetOrderUI(mermaid.GetNotSatisfiedBingsus());
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
        int addedScore = totalBingsuPrice / 20;
        switch(result)
        {
            case OrderResult.Pearl:
                dailyPearlCount += 1;
                addedScore += 100;
                break;
            case OrderResult.Ruby:
                dailyRubyCount += 1;
                addedScore += 250;
                break;
            case OrderResult.Diamond:
                dailyDiamondCount += 1;
                addedScore += 500;
                break;
        }

        // 다이아몬드일 경우 HP를 회복하거나, 보너스 점수르 얻는다.
        if (result == OrderResult.Diamond)
        {
            if (Hp == MAX_HP)
            {
                addedScore += 550;
            }
            else
            {
                Hp += 1;
            }
        }

        dailyBingsuPrice += totalBingsuPrice;
        dailyScore += addedScore;

        if (addedScore > 0)
        {
            inGameUI.ShowResultFloater(result, addedScore);
        }

        Debug.Log($"결과: {result}, 획득 점수: {addedScore}");
    }

    private void OrderFailed()
    {
        AudioManager.Instance.PlaySfx(SfxType.Fail);
        ResetBingsu();
        inGameUI.SetOrderUIEnable(false);
        inGameUI.SetServeButtonAInteractive(false);
        DeliverDailyRewards(mermaid.GetSatisfiedBingsus(), OrderResult.Fail);
        mermaid.SetExpression(Mermaid.EXPRESSION.ANGRY);
        --Hp;
        MermaidExit();
    }

    private void OrderSucceessed()
    {
        ResetBingsu();
        inGameUI.SetOrderUIEnable(false);
        inGameUI.SetServeButtonAInteractive(false);
        DeliverDailyRewards(mermaid.GetSatisfiedBingsus(), expectedResult);
        mermaid.SetExpression(Mermaid.EXPRESSION.HAPPY);
        MermaidExit();
    }

    public void OpenDailyResultUIPopup()
    {
        AudioManager.Instance.PlaySfx(SfxType.SuccessHigh);
        var jewelGold = CalculateGold(0, dailyPearlCount, dailyRubyCount, dailyDiamondCount);

        var dailyResult = new DailyResult()
        {
            day = day,

            pearlCount = dailyPearlCount,
            rubyCount = dailyRubyCount,
            diamondCount = dailyDiamondCount,

            prevGold = currentGold,
            dailyBingsuGold = dailyBingsuPrice,
            dailyJewelGold = jewelGold,
            currentGold = currentGold + jewelGold + dailyBingsuPrice,

            prevScore = currentScore,
            dailyScore = dailyScore,
            currentScore = currentScore + dailyScore
        };

        inGameUI.OpenDailyUIPopup(dailyResult);
    }

    private static int CalculateGold(int dailyBingsuPrice, int pearlCount, int rubyCount, int diamondCount)
    {
        return dailyBingsuPrice + 20 * pearlCount + 50 * rubyCount + 100 * diamondCount;
    }

    private void ResetBingsu()
    {
        selectedBingsu.Reset();
        inGameUI.SetResultBingsu(selectedBingsu);
    }

    private void MoveEndingScene(bool isHappyEnd)
    {
        EndingUIControl.IsHappyEnd = isHappyEnd;
        SceneLoadManager.Instance.MoveScene(SceneType.Ending);
    }
}