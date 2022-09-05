using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonReset : MonoBehaviour
{
    public void RestartLevel() //! Debug purposes only
    {
        SceneManager.LoadScene("MainScene");
    }
}
