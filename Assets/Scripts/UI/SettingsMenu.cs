using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public VarBool CanShoot;

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

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ActivateMenu()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            CanShoot.value = false;
        }
        else
        {
            gameObject.SetActive(false);
            CanShoot.value = true;
        }            
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
