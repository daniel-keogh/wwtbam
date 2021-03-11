using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneController : MonoBehaviour
{
    private GameController gc;

    void Start()
    {
        gc = FindObjectOfType<GameController>();
    }

    public void MainMenu()
    {
        gc?.ResetGame();
        SceneManager.LoadScene(SceneNames.MainMenu);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneNames.GameOver);
    }

    public void GameScene()
    {
        gc?.ResetGame();
        SceneManager.LoadScene(SceneNames.GameScene);
    }

    public void Quit()
    {
        if (SceneManager.GetActiveScene().name == SceneNames.MainMenu)
        {
            Application.Quit();
        }
        else
        {
            MainMenu();
        }
    }
}
