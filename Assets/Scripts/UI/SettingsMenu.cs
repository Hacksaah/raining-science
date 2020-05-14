using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public void ResumeGame()
    {
        gameObject.SetActive(false);
    }
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
