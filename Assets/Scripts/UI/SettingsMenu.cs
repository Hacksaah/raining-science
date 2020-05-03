using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void ResumeGame()
    {
        gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
