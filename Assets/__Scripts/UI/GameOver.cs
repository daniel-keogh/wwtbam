using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Utilities;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private GameController gc;

    void Start()
    {
        gc = FindObjectOfType<GameController>();

        if (gc != null)
        {
            scoreText.text += $"{string.Format("{0:n0}", gc.GetFinalWinnings())}";
        }
        else
        {
            scoreText.text += "0";
        }
    }

    public void OnPlayClicked()
    {
        gc?.ResetGame();
        SceneManager.LoadScene(SceneNames.GameScene);
    }

    public void OnQuitClicked()
    {
        gc?.ResetGame();
        SceneManager.LoadScene(SceneNames.MainMenu);
    }
}
