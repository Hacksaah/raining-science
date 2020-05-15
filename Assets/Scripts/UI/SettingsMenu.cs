using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    static SettingsMenu instance;
    public static SettingsMenu Instance
    {
        get
        {
            if (instance == null)
                new SettingsMenu();
            return instance;
        }
    }

    SettingsMenu() { instance = this; }

    public void ActivateMenu()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void CloseMenu()
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
