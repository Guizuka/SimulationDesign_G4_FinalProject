using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadVictory()
    {
        PlayerPrefs.SetInt("Score is : ", Score.scoreValue);
        SceneManager.LoadScene("WinScene");
    }
    public void LoadLose()
    {
        PlayerPrefs.SetInt("Score is: ", Score.scoreValue);
        SceneManager.LoadScene("LostScene");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("CoffeeShop");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
