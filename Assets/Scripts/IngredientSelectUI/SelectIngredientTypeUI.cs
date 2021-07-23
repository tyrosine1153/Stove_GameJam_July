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
        _iceGameDatas = _selectableIceTypes.Select(
                        iceType =>
                        IngredientGameDataHolder.Instance.IngredientGameDatas.GetIceGameData(iceType)).ToList();
        _selectIceUI.SetIngredientEntries(_iceGameDatas.Select(data => data.IngredientGameData).ToList(), this);

        _syrupGameDatas = _selectableSyrupTypes.Select(
                        syrupType =>
                        IngredientGameDataHolder.Instance.IngredientGameDatas.GetSyrupGameData(syrupType)).ToList();
        _selectSyrupUI.SetIngredientEntries(_syrupGameDatas.Select(data => data.IngredientGameData).ToList(), this);

        _toppingGameDatas = _selectableToppingTypes.Select(
                        toppingType =>
                        IngredientGameDataHolder.Instance.IngredientGameDatas.GetToppingGameData(toppingType)).ToList();
        _selectToppingUI.SetIngredientEntries(_toppingGameDatas.Select(data => data.IngredientGameData).ToList(), this);
    }

    public void OnIngradientSelected(IngredientType ingredientType, int index)
    {
        switch (ingredientType)
        {
            case IngredientType.Ice:
                TempStageManager.Instance.SelectIce(_iceGameDatas[index].IceType);
                break;
            case IngredientType.Syrup:
                TempStageManager.Instance.SelectSyrup(_syrupGameDatas[index].SyrupType);
                break;
            case IngredientType.Topping:
                TempStageManager.Instance.SelectTopping(_toppingGameDatas[index].ToppingType);
                break;
        }
    }
}
