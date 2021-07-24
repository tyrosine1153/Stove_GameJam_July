using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    private Text _highScore;

    [SerializeField]
    private Text _dayText;

    [SerializeField]
    private Text _goldText;

    [SerializeField]
    private GameObject _goldObject;

    [SerializeField]
    private InGameHPUI _hpUI;

    [SerializeField]
    private SelectIngredientTypeUI _ingredientUI;

    [SerializeField]
    private ResultBingsuUI _resultBingsuUI;

    [SerializeField]
    private InGameOrderUI _orderUI;

    [SerializeField]
    private GameObject _openButton;

    public void SetScore(int score)
    {
        if(int.Parse(_highScore.text) < score)
            _highScore.text = score.ToString();

    }

    public void SetDay(int day)
    {
        _dayText.text = $"{day + 1} 일";
    }

    public void SetHp(int hp)
    {
        _hpUI.SetHp(hp);
    }

    public void SetGold(int gold)
    {
        _goldText.text = $"{gold}G";
    }

    public void SetState(InGameState state)
    {
        SetOrderUIEnable(false);
        _goldObject.gameObject.SetActive(state == InGameState.Closed);
        _openButton.gameObject.SetActive(state == InGameState.Closed);
        _resultBingsuUI.gameObject.SetActive(state == InGameState.Playing);
    }

    public void SetOrderUI(List<Bingsu> bingsus)
    {
        if (bingsus.Count > 0)
        {
            SetOrderUIEnable(true);
            _orderUI.SetBingsus(bingsus);
        }
        else
        {
            SetOrderUIEnable(false);
        }
    }

    public void SetOrderUIEnable(bool flag)
    {
        _orderUI.gameObject.SetActive(flag);
    }

    public void SetResultBingsu(Bingsu bingsu)
    {
        _resultBingsuUI.SetResult(bingsu);
    }

    public void UpdateSelectIngredientUI()
    {
        _ingredientUI.UpdateUI();
    }
}
