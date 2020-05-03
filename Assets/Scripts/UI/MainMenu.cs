﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Insert the name of the scene to go to here.
    [SerializeField]
    private string PlayScene;

    [SerializeField]
    private GameObject SettingsPanel;

    [SerializeField]
    private GameObject HowToPlayPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
