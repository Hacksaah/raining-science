using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
        SceneManager.LoadScene("GameOverUI_Test");
    }

    public void OpenSettings()
    {
        //Open Settings
    }

    public void OpenHowToPlay()
    {
        //Open How to play
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
