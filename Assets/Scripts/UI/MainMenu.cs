using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    //Insert the name of the scene to go to here.
    [SerializeField]
    private string PlayScene = "Level1";

    [SerializeField]
    private GameObject SettingsPanel;

    [SerializeField]
    private GameObject HowToPlayPanel;

    public void PlayGame()
    {
        //Play the game.
        SceneManager.LoadScene(2);
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
