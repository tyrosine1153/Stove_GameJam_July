using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStageManager
{
    InGameState CurrentState { get; }
    IngredientUnlockData IngredientUnlockData { get; }
    void SelectIce(Data.ICE iceType);
    void SelectSyrup(Data.SYRUP syrupType);
    void SelectTopping(Data.TOPPING toppingType);
    bool BuyIce(Data.ICE iceType);
    bool BuyTopping(Data.TOPPING toppingType);
}
