using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Text scoreText;
   
    void Start()
    {
        int score = PlayerPrefs.GetInt("score");
        scoreText.text = score.ToString();
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadVictory()
    {
        SceneManager.LoadScene("WinScene");
    }
    public void LoadLose()
    {
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
