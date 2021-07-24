using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyResult
{
    public int day;
    
    public int pearlCount;
    public int rubyCount;
    public int diamondCount;

    public int prevGold;
    public int dailyBingsuGold;
    public int dailyJewelGold;
    public int currentGold;

    public int prevScore;
    public int dailyScore;
    public int currentScore;
}

public class InGameDailyResultUI : MonoBehaviour
{
    [SerializeField]
    private Text _titleText;

    [SerializeField]
    private Text _pearlCountText;

    [SerializeField]
    private Text _rubyCountText;

    [SerializeField]
    private Text _diamondCountText;

    [SerializeField]
    private Text _prevGoldText;

    [SerializeField]
    private Text _dailyBingsuGoldText;

    [SerializeField]
    private Text _dailyJewelGoldText;

    [SerializeField]
    private Text _currentGoldText;

    [SerializeField]
    private Text _prevScoreText;

    [SerializeField]
    private Text _dailyScoreText;

    [SerializeField]
    private Text _currentScoreText;

    public void SetResult(DailyResult dailyResult)
    {
        _titleText.text = $"{dailyResult.day}일차 결과";

        _pearlCountText.text = $"x {dailyResult.pearlCount}";
        _rubyCountText.text = $"x {dailyResult.rubyCount}";
        _diamondCountText.text = $"x {dailyResult.diamondCount}";

        _prevGoldText.text = $"{dailyResult.prevGold}G";
        _dailyBingsuGoldText.text = $"+ {dailyResult.dailyBingsuGold}G";
        _dailyJewelGoldText.text = $"+ {dailyResult.dailyJewelGold}G";
        _currentGoldText.text = $"{dailyResult.currentGold}G";

        _prevScoreText.text = $"{dailyResult.prevScore}점";
        _dailyScoreText.text = $"+ {dailyResult.dailyScore}점";
        _currentScoreText.text = $"{dailyResult.currentScore}점";
    }
}
