using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStatus : MonoBehaviour
{
    public IngredientType ingredientType;
    public GameObject prefab;
    public GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        setItems();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    void setItems()
    {
        int enumCount = getEnumCount();

        for (int idx = enumCount - 1; idx > 0; idx--)
        {
            var newObj = Instantiate(prefab, item.transform);
            Item itemInfo = newObj.GetComponent<Item>();
            if (itemInfo != null)
            {
                int cost = getCost(idx);
                bool isLock = false;
                itemInfo.setItem(cost, isLock);
            }
        }
    }

    int getEnumCount()
    {
        int count = 0;
        switch(ingredientType)
        {
            case IngredientType.Ice:
                count = System.Enum.GetValues(typeof(Data.ICE)).Length;
                break;
            case IngredientType.Syrup:
                count = System.Enum.GetValues(typeof(Data.SYRUP)).Length;
                break;
            case IngredientType.Topping:
                count = System.Enum.GetValues(typeof(Data.TOPPING)).Length;
                break;
        }
        return (count);
    }

    int getCost(int index)
    {
        int cost = 0;
        switch (ingredientType)
        {
            case IngredientType.Ice:
                cost = IngredientGameDataHolder.Instance.IngredientGameDatas.GetIceGameData((Data.ICE)index).IngredientGameData.UnlockCost;
                break;
            case IngredientType.Topping:
                cost = IngredientGameDataHolder.Instance.IngredientGameDatas.GetToppingGameData((Data.TOPPING)index).IngredientGameData.UnlockCost;
                break;
        }
        return (cost);
    }
}
