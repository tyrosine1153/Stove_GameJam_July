using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientEntry : MonoBehaviour
{
    [SerializeField]
    private Image _ingredientFillImage;

    [SerializeField]
    private Text _ingredientText;

    private IngredientType _ingredientType;
    private int _index;
    private SelectIngredientTypeUI _ingredientTypeUI;

    public void ApplyData(IngredientGameData ingredientData, IngredientType ingredientType, int index, SelectIngredientTypeUI ingredientUI)
    {
        _ingredientFillImage.sprite = ingredientData.IngredientSprite;
        _ingredientText.text = ingredientData.DisplayName;
        _ingredientType = ingredientType;
        _index = index;
        _ingredientTypeUI = ingredientUI;
    }

    public void OnSelected()
    {
        _ingredientTypeUI.OnIngradientSelected(_ingredientType, _index);
    }
}
