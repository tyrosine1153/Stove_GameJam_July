using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    private Text _dayText;

    [SerializeField]
    private Text _goldText;

    [SerializeField]
    private GameObject _goldObject;

    [SerializeField]
    private Text _highScoreText;

    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private GameObject _scoreObject;

    [SerializeField]
    private Text _guestText;

    [SerializeField]
    private GameObject _guestObject;

    [SerializeField]
    private InGameHPUI _hpUI;

    [SerializeField]
    private SelectIngredientTypeUI _ingredientUI;

    [SerializeField]
    private ResultBingsuUI _resultBingsuUI;

    [SerializeField]
    private InGameOrderUI _orderUI;

    [SerializeField]
    private InGameDailyResultUI _dailyResultUI;

    [SerializeField]
    private OrderResultFloater _resultFloater;

    [SerializeField]
    private GameObject _recipeButton;

    [SerializeField]
    private Button _serveButton;

    [SerializeField]
    private GameObject _settingButton;

    [SerializeField]
    private GameObject _openButton;

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

    public void SetScore(int score, int highScore)
    {
        _scoreText.text = $"{score}점";
        _highScoreText.text = $"{highScore}점";
    }

    public void SetGuest(int remainGuest, int maxGuest)
    {
        _guestText.text = $"{remainGuest} / {maxGuest}";
    }

    public void SetState(InGameState state)
    {
        SetOrderUIEnable(false);
        SetServeButtonAInteractive(false);
        _hpUI.gameObject.SetActive(state == InGameState.Playing);
        _settingButton.SetActive(state == InGameState.Closed);
        _serveButton.gameObject.SetActive(state == InGameState.Playing);
        _recipeButton.SetActive(state == InGameState.Closed);
        _guestObject.SetActive(state == InGameState.Playing);
        _goldObject.SetActive(state == InGameState.Closed);
        _openButton.SetActive(state == InGameState.Closed);
        _resultBingsuUI.gameObject.SetActive(state == InGameState.Playing);
        _scoreObject.SetActive(state == InGameState.Closed);
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

    public void SetServeButtonAInteractive(bool flag)
    {
        _serveButton.interactable = flag;
    }

    public void SetResultBingsu(Bingsu bingsu)
    {
        _resultBingsuUI.SetResult(bingsu);
    }

    public void UpdateSelectIngredientUI()
    {
        _ingredientUI.UpdateUI();
    }

    public void OpenDailyUIPopup(DailyResult result)
    {
        _dailyResultUI.gameObject.SetActive(true);
        _dailyResultUI.SetResult(result);
    }

    public void ShowResultFloater(OrderResult jewel, int score)
    {
        _resultFloater.ShowResult(jewel, score);
    }
}
