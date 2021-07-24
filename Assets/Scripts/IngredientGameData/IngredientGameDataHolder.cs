using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientGameDataHolder : MonoSingleton<IngredientGameDataHolder>
{
    [HideInInspector]
    public IngredientGameDatas IngredientGameDatas;

    private static string dataPath = "GameData/IngredientData/IngredientGameDatas";

    private void Awake()
    {
        if (IngredientGameDatas == null)
        {
            IngredientGameDatas = Resources.Load<IngredientGameDatas>(dataPath);
        }
        IngredientGameDatas.Intialize();
    }
}
