using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuScreen;
    // [SerializeField] private GameObject _buyButton;
    private int _amountToBuy = 1;

    [SerializeField] private TMP_Text _amountToBuyText;
    [SerializeField] private TMP_Text _playerAmountVillagersText;
    [SerializeField] private TMP_Text _playerAmountCoinsText;
    private int _playerAmountVillagers = 100;
    private int _playerAmountCoins = 100;

    private void Awake()
    {
        UpdateResourceView();
        UpdateAmountView();
    }

    public void BuyItem(string resType)
    {
        if (_playerAmountCoins-_amountToBuy < 0) return;

        if (resType.Equals("villager"))
        {
            _playerAmountCoins -= _amountToBuy;
            _playerAmountVillagers += _amountToBuy;
        }

        Debug.Log("BUY! BUY! BUY! KACHIN!");

        UpdateResourceView();
    }

    public void UpdateResourceView()
    {
        _playerAmountCoinsText.text = _playerAmountCoins.ToString();
        _playerAmountVillagersText.text = _playerAmountVillagers.ToString();
    }

    

    public void ChangeAmount(int newAmount)
    {
        _amountToBuy = newAmount;
        UpdateAmountView();
    }

    public void UpdateAmountView()
    {
        _amountToBuyText.text = $"Buy {_amountToBuy.ToString()}";
    }

    public void BackToMenu()
    {
        StartCoroutine(SceneController.Instance.ChangeScene(this.gameObject, _mainMenuScreen));
    }



}
