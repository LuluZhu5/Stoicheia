using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Scene 1");
    }

    public void Setting()
    {
        SceneManager.LoadScene("Setting");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
