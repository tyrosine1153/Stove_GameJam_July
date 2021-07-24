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

    [Header("????")]
    public Mermaid mermaid;
    [Header("json????")]
    public TextAsset jsonFile;

    public InGameUI inGameUI;
    public Data.StageJson[] stage;

    private int mermaidCount;
    public int day = 0;    //0�������� 29�������� 30��
    private bool IsGuest = false;
    private int hp = 3;
    private float time = 0;
    private int gold = 0;

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
        UpdateIngredientUI();
        UpdateResultBingsuUI();
    }

    void Start()
    {
        if (jsonFile)
            stage = JsonUtility.FromJson<Data.Stage>("{\"stage\":" + jsonFile.text + "}").stage;
        else
            Debug.LogError("Json?????? ???????? ????");

    }

    private void FixedUpdate()
    {
        if (IsGuest)
        {
            time += Time.fixedDeltaTime;
            if (time > 15)
            {
                // ???? UI(???? ??) ??????
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

        currentState = InGameState.Playing;
        UpdateIngredientUI();
        UpdateResultBingsuUI();

        // UI???????? Day + 1?? ????
        StartCoroutine("guestCome");
    }

    void CloseStore()
    {
        currentState = InGameState.Closed;
        UpdateIngredientUI();
        UpdateResultBingsuUI();

        //Scene ????
        //????
        //???? ?????? ??
        day++;
        if(day >= 30)
        {
            //???? ????
        }
    }

    IEnumerator guestCome()
    {

        WaitWhile waitWhile = new WaitWhile(() => IsGuest);
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        int index = 0;
        while (mermaidCount > index)    //Day ???? ????, json ?????? ????
        {
            //�մ� �̹��� Ȱ��ȭ �� ��������Ʈ (�մ� ����)����, ���� ����, ���ϴ� ���� ����
            mermaid.Setting(day);

            //�մ� ����, Ÿ�̸� ����
            IsGuest = true;

            time = 0;

            //UI(���� ���� ��) �ʱ�ȭ
            //������ ������ �ʱ�ȭ

            yield return waitWhile;
            yield return waitForSeconds;    //???? ?????? ???? ?????? ????
            index++;
        }
        CloseStore();
    }

    public void SelectIce(Data.ICE iceType)
    {
        // ??? ???? ?? ???, ? ?? ??? ????.
        if (selectedIce == Data.ICE.NONE)
        {
            selectedIce = iceType;
            UpdateResultBingsuUI();
        }
    }

    public void SelectSyrup(Data.SYRUP syrupType)
    {
        // ??? ???? ?? ???, ??? ? ??.
        if (selectedIce == Data.ICE.NONE)
        {
            return;
        }

        // ??? ???? ?? ???, ? ?? ??? ????.
        if (selectedSyrup == Data.SYRUP.NONE)
        {
            selectedSyrup = syrupType;
            UpdateResultBingsuUI();
        }
        // ??? ? ??? ? ??? (?? ? ???), ?? ??? ????.
        else if (SyrupColorMix.TryGetMixedSyrup(selectedSyrup, syrupType, out var resultSyrup))
        {
            selectedSyrup = resultSyrup;
            UpdateResultBingsuUI();
        }
    }

    public void SelectTopping(Data.TOPPING toppingType)
    {
        // ??? ???? ?? ???, ??? ? ??.
        if (selectedIce == Data.ICE.NONE)
        {
            return;
        }

        // ??? ???? ?? ???, ? ?? ??? ????.
        if (selectedTopping == Data.TOPPING.NONE)
        {
            selectedTopping = toppingType;
            UpdateResultBingsuUI();
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
        return ingredientUnlockData.UnlockTopping(toppingType);
    }

    private void UpdateIngredientUI()
    {
        inGameUI.UpdateSelectIngredientUI();
    }

    public void UpdateResultBingsuUI()
    {
        inGameUI.SetResultBingsu(selectedIce, selectedSyrup, selectedTopping);
    }

    //?? ?????? UI ???????? ??????
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

    // ???? ???? ??
    public void MermaidExit(bool isSueccess)
    {
        //???? ??????
        mermaid.gameObject.SetActive(false);
        if (isSueccess)
        {
            if (time > 10)
            {
                //????
                // ???? ????, ????
                mermaid.SetExpression(Mermaid.EXPRESSION.ANGRY);
            }
            else if (time > 8)
            {
                //????
                // ???? ????, ????
                mermaid.SetExpression(Mermaid.EXPRESSION.IDLE);
            }
            else
            {
                //??????
                // ???? ????, ????

                if (hp >= 3)
                {
                    //???? ????
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
            //HP UI ????????
            if (hp == 0)
                End();
        }
        IsGuest = false;
    }

    void CalculReward()
    {
        //mermaid.bingsuCount
        //selectedIce ?????? ???? ????
    }

    void End()
    {
        //???? ??
    }
}
