using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Json으로 저장/로 할수 있도록 만들어둠.
public class IngredientUnlockData
{
    public List<Data.ICE> IcesUnlocked;
    public List<Data.SYRUP> SyrupsUnlocked;
    public List<Data.TOPPING> ToppingsUnlocked;

    private static readonly int NONE_INGREDIENT_PROBABILITY = 10;

    public IngredientUnlockData(List<Data.ICE> ices, List<Data.SYRUP> syrups, List<Data.TOPPING> toppings)
    {
        IcesUnlocked = new List<Data.ICE>(ices);
        SyrupsUnlocked = new List<Data.SYRUP>(syrups);
        ToppingsUnlocked = new List<Data.TOPPING>(toppings);
    }

    public bool UnlockIce(Data.ICE iceType)
    {
        if (IsIceUnlocked(iceType))
        {
            return false;
        }

        IcesUnlocked.Add(iceType);
        return true;
    }

    public bool UnlockTopping(Data.TOPPING toppingType)
    {
        if (IsToppingUnlocked(toppingType))
        {
            return false;
        }

        ToppingsUnlocked.Add(toppingType);
        return true;
    }

    public bool IsIceUnlocked(Data.ICE iceType)
    {
        return IcesUnlocked.Contains(iceType);
    }

    public bool IsSyrupUnlocked(Data.SYRUP syrupType)
    {
        return SyrupsUnlocked.Contains(syrupType);
    }

    public bool IsToppingUnlocked(Data.TOPPING toppingType)
    {
        return ToppingsUnlocked.Contains(toppingType);
    }

    public Data.ICE GetRandomIce()
    {
        int index = Random.Range(0, IcesUnlocked.Count);
        return IcesUnlocked[index];
    }

    public Data.SYRUP GetRandomSyrup()
    {
        // 시럽 없는 것은 10%확률로 등장
        if (Random.Range(0, 100) < NONE_INGREDIENT_PROBABILITY)
        {
            return Data.SYRUP.NONE;
        }

        int index = Random.Range(0, SyrupsUnlocked.Count);
        return SyrupsUnlocked[index];
    }

    public Data.TOPPING GetRandomTopping()
    {
        // 토핑 없는 것은 10%확률로 등장
        if (Random.Range(0, 100) < NONE_INGREDIENT_PROBABILITY)
        {
            return Data.TOPPING.NONE;
        }

        int index = Random.Range(0, ToppingsUnlocked.Count);
        return ToppingsUnlocked[index];
    }
}
