using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectIngredientTypeUI : MonoBehaviour
{
    [SerializeField]
    private List<Data.ICE> _selectableIceTypes = new List<Data.ICE>();
    [SerializeField]
    private List<Data.SYRUP> _selectableSyrupTypes = new List<Data.SYRUP>();
    [SerializeField]
    private List<Data.TOPPING> _selectableToppingTypes = new List<Data.TOPPING>();

    [SerializeField]
    private GameObject _uiRootObject;

    [SerializeField]
    private SelectIngredientUI _selectIceUI;
    [SerializeField]
    private SelectIngredientUI _selectSyrupUI;
    [SerializeField]
    private SelectIngredientUI _selectToppingUI;

    private List<IceGameData> _iceGameDatas;
    private List<SyrupGameData> _syrupGameDatas;
    private List<ToppingGameData> _toppingGameDatas;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        var stageManager = GetStageManager();
        _iceGameDatas = _selectableIceTypes.Select(
                        iceType =>
                        IngredientGameDataHolder.Instance.IngredientGameDatas.GetIceGameData(iceType)).ToList();
        _selectIceUI.SetIngredientEntries(
            _iceGameDatas.Select(data => data.IngredientGameData).ToList(),
            _iceGameDatas.Select(data => stageManager.IngredientUnlockData.IsIceUnlocked(data.IceType)).ToList(),
            stageManager.CurrentState,
            this);

        _syrupGameDatas = _selectableSyrupTypes.Select(
                        syrupType =>
                        IngredientGameDataHolder.Instance.IngredientGameDatas.GetSyrupGameData(syrupType)).ToList();
        _selectSyrupUI.SetIngredientEntries(
            _syrupGameDatas.Select(data => data.IngredientGameData).ToList(),
            _syrupGameDatas.Select(data => stageManager.IngredientUnlockData.IsSyrupUnlocked(data.SyrupType)).ToList(),
            stageManager.CurrentState,
            this);

        _toppingGameDatas = _selectableToppingTypes.Select(
                        toppingType =>
                        IngredientGameDataHolder.Instance.IngredientGameDatas.GetToppingGameData(toppingType)).ToList();
        _selectToppingUI.SetIngredientEntries(
            _toppingGameDatas.Select(data => data.IngredientGameData).ToList(),
            _toppingGameDatas.Select(data => stageManager.IngredientUnlockData.IsToppingUnlocked(data.ToppingType)).ToList(),
            stageManager.CurrentState,
            this);
    }

    public void OnIngradientSelected(IngredientType ingredientType, int index, IngredientEntry entry)
    {
        var stageManager = GetStageManager();
        switch (ingredientType)
        {
            case IngredientType.Ice:
                if (stageManager.CurrentState == InGameState.Playing)
                {
                    stageManager.SelectIce(_iceGameDatas[index].IceType);
                }
                if (stageManager.CurrentState == InGameState.Closed)
                {
                    var buyResult = stageManager.BuyIce(_iceGameDatas[index].IceType);
                    if (!buyResult)
                    {
                        entry.Shake();
                    }
                }
                break;
            case IngredientType.Syrup:
                if (stageManager.CurrentState == InGameState.Playing)
                {
                    stageManager.SelectSyrup(_syrupGameDatas[index].SyrupType);
                }
                break;
            case IngredientType.Topping:
                if (stageManager.CurrentState == InGameState.Playing)
                {
                    stageManager.SelectTopping(_toppingGameDatas[index].ToppingType);
                }
                if (stageManager.CurrentState == InGameState.Closed)
                {
                    var buyResult = stageManager.BuyTopping(_toppingGameDatas[index].ToppingType);
                    if (!buyResult)
                    {
                        entry.Shake();
                    }
                }
                break;
        }
    }

    private static IStageManager GetStageManager()
    {
        IStageManager stageManager = StageManager.instance;
        if (stageManager == null)
        {
            stageManager = TempStageManager.Instance;
        }
        return stageManager;
    }
}
