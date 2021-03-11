using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class MainMenu : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneManager.LoadScene(SceneNames.GameScene);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
