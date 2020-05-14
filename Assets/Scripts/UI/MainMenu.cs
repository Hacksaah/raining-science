using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    //Insert the name of the scene to go to here.
    [SerializeField]
    private int PlayScene = 1;

    [SerializeField]
    private GameObject SettingsPanel = null;

    [SerializeField]
    private GameObject HowToPlayPanel = null;

    [SerializeField]
    private GameObject CreditsPanel = null;

    public void PlayGame()
    {
        //Play the game.
        SceneManager.LoadScene(PlayScene);
    }

    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenHowToPlay()
    {
        HowToPlayPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OpenCredits()
    {
        CreditsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

    public void BackToMenu(GameObject panelToClose)
    {
        panelToClose.SetActive(false);
        gameObject.SetActive(true);
    }
}
