using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{

    public static GameOverUI Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ActivateUI()
    {
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
