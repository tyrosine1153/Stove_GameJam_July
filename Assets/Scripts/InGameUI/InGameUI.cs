using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    private Text _dayText;

    [SerializeField]
    private InGameHPUI _hpUI;

    [SerializeField]
    private SelectIngredientTypeUI _ingredientUI;

    [SerializeField]
    private ResultBingsuUI _resultBingsuUI;

    private InGameState _currentState;

    public void SetDay(int day)
    {
        _dayText.text = $"{day} 일";
    }

    public void SetHp(int hp)
    {
        _hpUI.SetHp(hp);
    }

    public void SetResultBingsu(Data.ICE selectedIce, Data.SYRUP selectedSyrup, Data.TOPPING selectedTopping)
    {
        _resultBingsuUI.SetResult(selectedIce, selectedSyrup, selectedTopping);
    }

    public void UpdateSelectIngredientUI()
    {
        _ingredientUI.UpdateUI();
    }
}
