using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuScreen;
    // [SerializeField] private GameObject _buyButton;

    public void BuyItem()
    {
        Debug.Log("BUY! BUY! BUY! KACHIN!");
    }

    public void BackToMenu()
    {
        StartCoroutine(SceneController.Instance.ChangeScene(this.gameObject, _mainMenuScreen));
    }


}
