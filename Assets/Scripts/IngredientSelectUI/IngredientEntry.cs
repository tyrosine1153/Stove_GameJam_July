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

    [SerializeField]
    private GameObject _buyUIObject;

    [SerializeField]
    private GameObject _lockUIObject;

    [SerializeField]
    private Text _unlockCostText;

    private IngredientType _ingredientType;
    private int _index;
    private bool _isUnlocked;
    InGameState _inGameState;

    private SelectIngredientTypeUI _ingredientTypeUI;

    private bool isShaking = false;

    public void ApplyData(IngredientGameData ingredientData, bool isUnlocked, InGameState gameState, IngredientType ingredientType, int index, SelectIngredientTypeUI ingredientUI)
    {
        _ingredientFillImage.sprite = ingredientData.IngredientSprite;
        _ingredientText.text = ingredientData.DisplayName;
        _isUnlocked = isUnlocked;
        _inGameState = gameState;
        _ingredientType = ingredientType;
        _index = index;
        _ingredientTypeUI = ingredientUI;

        switch(gameState)
        {
            case InGameState.Playing:
                _buyUIObject.SetActive(false);
                _lockUIObject.SetActive(!isUnlocked);
                break;
            case InGameState.Closed:
                _buyUIObject.SetActive(!isUnlocked);
                _lockUIObject.SetActive(false);
                _unlockCostText.text = $"{ingredientData.UnlockCost}G";
                break;
        }
    }

    public void OnSelected()
    {
        switch(_inGameState)
        {
            case InGameState.Playing:
                if (_isUnlocked)
                {
                    _ingredientTypeUI.OnIngradientSelected(_ingredientType, _index, this);
                }
                break;
            case InGameState.Closed:
                if (!_isUnlocked)
                {
                    _ingredientTypeUI.OnIngradientSelected(_ingredientType, _index, this);
                }
                break;
        }
    }

    public void Shake()
    {
        if (!isShaking)
            StartCoroutine(ShakeAnimation());
    }

    protected IEnumerator ShakeAnimation()
    {
        AudioManager.Instance.PlaySfx(SfxType.Cancel);

        isShaking = true;
        int count = 3;
        Vector3 movement = new Vector3(5, 0, 0);

        while (count-- > 0)
        {
            this.transform.position += movement;
            yield return new WaitForSeconds(0.1f);
            this.transform.position -= movement;

            yield return new WaitForSeconds(0.1f);
        }
        isShaking = false;
    }
}
