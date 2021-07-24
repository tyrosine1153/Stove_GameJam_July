using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StageManager 기능이 구현되면, 이 코드를 그쪽에 옮겨간다.
public class TempStageManager : MonoSingleton<TempStageManager>, IStageManager
{
    public InGameState CurrentState => InGameState.Playing;

    [SerializeField]
    private ResultBingsuUI _resultUI;

    private IngredientUnlockData ingredientUnlockData;
    public IngredientUnlockData IngredientUnlockData => ingredientUnlockData;

    private Data.ICE _selectedIce;
    private Data.SYRUP _selectedSyrup;
    private Data.TOPPING _selectedTopping;

    private void Awake()
    {
        var ingredientData = IngredientGameDataHolder.Instance.IngredientGameDatas;
        ingredientUnlockData = new IngredientUnlockData(
                ingredientData.GetAllFreeIces(),
                ingredientData.GetAllFreeSyrups(),
                ingredientData.GetAllFreeToppings()
            );

        ResetIngredientsForTest();
    }

    public void SelectIce(Data.ICE iceType)
    {
        // 얼음이 선택되어 있지 않다면, 이 얼음 종류를 선택한다.
        if (_selectedIce == Data.ICE.NONE)
        {
            _selectedIce = iceType;
            UpdateResultBingsuUI();
        }
    }

    public void SelectSyrup(Data.SYRUP syrupType)
    {
        // 얼음이 선택되어 있지 않다면, 선택할 수 없다.
        if (_selectedIce == Data.ICE.NONE)
        {
            return;
        }

        // 시럽잇 선택되어 있지 않다면, 이 시럽 종류를 선택한다.
        if (_selectedSyrup == Data.SYRUP.NONE)
        {
            _selectedSyrup = syrupType;
            UpdateResultBingsuUI();
        }
        // 시럽을 더 선택할 수 있다면 (섞을 수 았다면), 섞은 시럽을 선택한다.
        else if (SyrupColorMix.TryGetMixedSyrup(_selectedSyrup, syrupType, out var resultSyrup))
        {
            _selectedSyrup = resultSyrup;
            UpdateResultBingsuUI();
        }
    }

    public void SelectTopping(Data.TOPPING toppingType)
    {
        // 얼음이 선택되어 있지 않다면, 선택할 수 없다.
        if (_selectedIce == Data.ICE.NONE)
        {
            return;
        }

        // 토핑이 선택되어 있지 않다면, 이 토핑 종류를 선택한다.
        if (_selectedTopping == Data.TOPPING.NONE)
        {
            _selectedTopping = toppingType;
            UpdateResultBingsuUI();
        }
    }

    // 테스트를 위해 선택한 재료를 리셋한다. 실제 게임에서 사용되지는 않는다.
    public void ResetIngredientsForTest()
    {
        _selectedIce = Data.ICE.NONE;
        _selectedSyrup = Data.SYRUP.NONE;
        _selectedTopping = Data.TOPPING.NONE;

        UpdateResultBingsuUI();
    }

    public void UpdateResultBingsuUI()
    {
        _resultUI.SetResult(new Bingsu(_selectedIce, _selectedSyrup, _selectedTopping));
    }

    public bool BuyIce(Data.ICE iceType)
    {
        return false;
    }

    public bool BuyTopping(Data.TOPPING toppingType)
    {
        return false;
    }
}
