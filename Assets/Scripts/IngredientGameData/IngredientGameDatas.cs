using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class IceGameData
{
    public Data.ICE IceType;
    public IngredientGameData IngredientGameData;
}

[System.Serializable]
public class SyrupGameData
{
    public Data.SYRUP SyrupType;
    public IngredientGameData IngredientGameData;
}

[System.Serializable]
public class ToppingGameData
{
    public Data.TOPPING ToppingType;
    public IngredientGameData IngredientGameData;
}

[System.Serializable]
public class IngredientGameData
{
    public string DisplayName;
    public Sprite ResultSprite;
    public Sprite IngredientSprite;
    public int UnlockCost;
}

[CreateAssetMenu]
public class IngredientGameDatas : ScriptableObject
{
    public List<IceGameData> IceDatas = new List<IceGameData>();
    public List<SyrupGameData> SyrupDatas = new List<SyrupGameData>();
    public List<ToppingGameData> ToppingDatas = new List<ToppingGameData>();

    private Dictionary<Data.ICE, IceGameData> _iceDatas = new Dictionary<Data.ICE, IceGameData>();
    private Dictionary<Data.SYRUP, SyrupGameData>  _syrupDatas = new Dictionary<Data.SYRUP, SyrupGameData>();
    private Dictionary<Data.TOPPING, ToppingGameData> _toppingDatas = new Dictionary<Data.TOPPING, ToppingGameData>();

    public void Intialize()
    {
        _iceDatas.Clear();
        _syrupDatas.Clear();
        _toppingDatas.Clear();

        foreach(var data in IceDatas)
        {
            if (_iceDatas.ContainsKey(data.IceType))
            {
                Debug.LogError($"ICE 데이터를 초기화하는 중 중복되는 데이터가 감지되었습니다. 해당 데이터를 무시합니다. ICE[{data.IceType}]");
                continue;
            }
            _iceDatas.Add(data.IceType, data);
        }

        foreach (var data in SyrupDatas)
        {
            if (_syrupDatas.ContainsKey(data.SyrupType))
            {
                Debug.LogError($"SYRUP 데이터를 초기화하는 중 중복되는 데이터가 감지되었습니다. 해당 데이터를 무시합니다. SYRUP[{data.SyrupType}]");
                continue;
            }
            _syrupDatas.Add(data.SyrupType, data);
        }

        foreach (var data in ToppingDatas)
        {
            if (_toppingDatas.ContainsKey(data.ToppingType))
            {
                Debug.LogError($"TOPPING 데이터를 초기화하는 중 중복되는 데이터가 감지되었습니다. 해당 데이터를 무시합니다. TOPPING[{data.ToppingType}]");
                continue;
            }
            _toppingDatas.Add(data.ToppingType, data);
        }
    }

    public IceGameData GetIceGameData(Data.ICE iceType)
    {
        if (!_iceDatas.TryGetValue(iceType, out var gameData))
        {
            Debug.LogError($"ICE 데이터를 찾을수 없습니다. ICE[{iceType}]");
            return null;
        }

        return gameData;
    }

    public SyrupGameData GetSyrupGameData(Data.SYRUP syrupType)
    {
        if (!_syrupDatas.TryGetValue(syrupType, out var gameData))
        {
            Debug.LogError($"SYRUP 데이터를 찾을수 없습니다. SYRUP[{syrupType}]");
            return null;
        }

        return gameData;
    }

    public ToppingGameData GetToppingGameData(Data.TOPPING toppingType)
    {
        if (!_toppingDatas.TryGetValue(toppingType, out var gameData))
        {
            Debug.LogError($"TOPPING 데이터를 찾을수 없습니다. TOPPING[{toppingType}]");
            return null;
        }

        return gameData;
    }

    public List<IngredientGameData> GetAllIceIngredientDatas()
    {
        return _iceDatas.Select(data => data.Value.IngredientGameData).ToList();
    }

    public List<IngredientGameData> GetAllSyrupIngredientDatas()
    {
        return _syrupDatas.Select(data => data.Value.IngredientGameData).ToList();
    }

    public List<IngredientGameData> GetAllToppingIngredientDatas()
    {
        return _toppingDatas.Select(data => data.Value.IngredientGameData).ToList();
    }

    public List<Data.ICE> GetAllFreeIces()
    {
        return _iceDatas
            .Where(data => data.Value.IngredientGameData.UnlockCost == 0)
            .Select(data => data.Key)
            .ToList();
    }

    public List<Data.SYRUP> GetAllFreeSyrups()
    {
        return _syrupDatas
            .Where(data => data.Value.IngredientGameData.UnlockCost == 0)
            .Select(data => data.Key)
            .ToList();
    }

    public List<Data.TOPPING> GetAllFreeToppings()
    {
        return _toppingDatas
            .Where(data => data.Value.IngredientGameData.UnlockCost == 0)
            .Select(data => data.Key)
            .ToList();
    }
}
