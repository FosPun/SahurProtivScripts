using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class MenuFunctions : MonoBehaviour
{
  
    public void NextLevel()
    {
        SceneManager.LoadScene(1);
        YG2.InterstitialAdvShow();
        Time.timeScale = 1f;
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
