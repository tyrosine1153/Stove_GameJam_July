using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bingsu : IEquatable<Bingsu>
{
    public Data.ICE Ice;
    public Data.SYRUP Syrup;
    public Data.TOPPING Topping;

    public Bingsu()
    {
        Ice = Data.ICE.NONE;
        Syrup = Data.SYRUP.NONE;
        Topping = Data.TOPPING.NONE;
    }

    public Bingsu(Data.ICE ice, Data.SYRUP syrup, Data.TOPPING topping)
    {
        Ice = ice;
        Syrup = syrup;
        Topping = topping;
    }

    public bool Equals(Bingsu other)
    {
        return Ice == other.Ice
            && Syrup == other.Syrup
            && Topping == other.Topping;
    }

    public int CalculatePrice()
    {
        var icePrice = IngredientGameDataHolder.Instance.IngredientGameDatas.GetIceGameData(Ice).IngredientGameData.UnlockCost;
        var toppingPrice = IngredientGameDataHolder.Instance.IngredientGameDatas.GetToppingGameData(Topping).IngredientGameData.UnlockCost;
        return icePrice + toppingPrice;
    }

    public void Reset()
    {
        Ice = Data.ICE.NONE;
        Syrup = Data.SYRUP.NONE;
        Topping = Data.TOPPING.NONE;
    }

    public override string ToString()
    {
        return $"Ice: {Ice}, Syrup: {Syrup}, Topping: {Topping}";
    }
}
