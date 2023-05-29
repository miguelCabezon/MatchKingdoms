using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC.Model;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private GameObject _shopScreen;
    // private GameState _gameState = GameState.MAINMENU;

    public void PlayGame() 
    {
        Debug.Log("play!");
        StartCoroutine(SceneController.Instance.ChangeScene(this.gameObject, _gameScreen));
    }

    public void Shop() 
    {
        StartCoroutine(SceneController.Instance.ChangeScene(this.gameObject, _shopScreen));
    }
}
