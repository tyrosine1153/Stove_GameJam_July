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
        int totalPrice = 0;
        if (Ice != Data.ICE.NONE)
        {
            var icePrice = IngredientGameDataHolder.Instance.IngredientGameDatas.GetIceGameData(Ice).IngredientGameData.UnlockCost;
            if (icePrice == 0) icePrice = 10;
            totalPrice += icePrice;
        }

        if (Topping != Data.TOPPING.NONE)
        {
            var toppingPrice = IngredientGameDataHolder.Instance.IngredientGameDatas.GetToppingGameData(Topping).IngredientGameData.UnlockCost;
            if (toppingPrice == 0) toppingPrice = 10;
            totalPrice += toppingPrice;
        }

        return totalPrice;
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
