using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultBingsuUI : MonoBehaviour
{
    [SerializeField]
    private Image _iceImage;

    [SerializeField]
    private Image _syrupImage;

    [SerializeField]
    private Image _toppingImage;

    public void ResetAllIngredients()
    {
        SetResult(Data.ICE.NONE, Data.SYRUP.NONE, Data.TOPPING.NONE);
    }

    public void SetResult(Data.ICE iceType, Data.SYRUP syrupType, Data.TOPPING toppingType)
    {
        SetIce(iceType);
        SetSyrup(syrupType);
        SetTopping(toppingType);
    }

    private void SetIce(Data.ICE iceType)
    {
        if (iceType == Data.ICE.NONE)
        {
            _iceImage.gameObject.SetActive(false);
        }
        else
        {
            _iceImage.gameObject.SetActive(true);
            _iceImage.sprite = IngredientGameDataHolder.Instance.IngredientGameDatas.GetIceGameData(iceType).IngredientGameData.ResultSprite;
        }
    }

    private void SetSyrup(Data.SYRUP syrupType)
    {
        if (syrupType == Data.SYRUP.NONE)
        {
            _syrupImage.gameObject.SetActive(false);
        }
        else
        {
            _syrupImage.gameObject.SetActive(true);
            _syrupImage.sprite = IngredientGameDataHolder.Instance.IngredientGameDatas.GetSyrupGameData(syrupType).IngredientGameData.ResultSprite;
        }
    }

    private void SetTopping(Data.TOPPING toppingType)
    {
        if (toppingType == Data.TOPPING.NONE)
        {
            _toppingImage.gameObject.SetActive(false);
        }
        else
        {
            _toppingImage.gameObject.SetActive(true);
            _toppingImage.sprite = IngredientGameDataHolder.Instance.IngredientGameDatas.GetToppingGameData(toppingType).IngredientGameData.ResultSprite;
        }
    }
}
