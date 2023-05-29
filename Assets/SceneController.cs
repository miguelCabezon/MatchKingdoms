using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    public static SceneController Instance;
    [SerializeField] private GameObject _loadingScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public IEnumerator ChangeScene(GameObject oldScene, GameObject newScene)
    {
        _loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        oldScene.SetActive(false);
        _loadingScreen.SetActive(false);
        newScene.SetActive(true);
    }
}
