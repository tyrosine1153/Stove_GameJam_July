using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectIngredientUI : MonoBehaviour
{
    [SerializeField]
    private IngredientType _ingredientType;

    [SerializeField]
    private Transform _entryRoot;

    [SerializeField]
    private IngredientEntry _ingredientEntryPrefab;

    private List<IngredientEntry> _ingredientEntries = new List<IngredientEntry>();

    public void SetIngredientEntries(List<IngredientGameData> ingredientDatas, SelectIngredientTypeUI ingredientTypeUI)
    {
        SetIngredientEntryCount(ingredientDatas.Count);
        for (int i = 0; i < ingredientDatas.Count; i++)
        {
            _ingredientEntries[i].ApplyData(ingredientDatas[i], _ingredientType, i, ingredientTypeUI);
        }
    }

    private void SetIngredientEntryCount(int count)
    {
        if (_ingredientEntries.Count < count)
        {
            foreach (var entry in _ingredientEntries)
            {
                entry.gameObject.SetActive(true);
            }

            int makeCount = count - _ingredientEntries.Count;
            while (makeCount > 0)
            {
                var entry = GameObject.Instantiate<IngredientEntry>(_ingredientEntryPrefab, _entryRoot);
                _ingredientEntries.Add(entry);
                makeCount -= 1;
            }
        }
        else if (_ingredientEntries.Count > count)
        {
            for (int i = 0; i < count; i++)
            {
                _ingredientEntries[i].gameObject.SetActive(true);
            }

            for (int i = count; i < _ingredientEntries.Count; i++)
            {
                _ingredientEntries[i].gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var entry in _ingredientEntries)
            {
                entry.gameObject.SetActive(true);
            }
        }
    }
}
