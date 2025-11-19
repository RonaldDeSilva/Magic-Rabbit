using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;

    void Update()
    {
        // Toggle pause using P key
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }

        // Controller: open Card Menu with StartButton
        if (Input.GetButtonDown("StartButton"))
        {
            LoadCardMenu();
        }

        // Quit with Exit button
        if (Input.GetButtonDown("Cancel"))
        {
            QuitGame();
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadCardMenu()
    {
        SceneManager.LoadScene("Card Menu 2");
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();

    }
}

/*
    public void OpenSettingsMenu()
    {
       PauseMenuUI.SetActive(false);     // Hide pause menu
       SettingsMenu.SetActive(true);   // Show settings menu
    }

    public void CloseSettingsMenu()
    {
        SettingsMenu.SetActive(false);  // Hide settings menu
        PauseMenuUI.SetActive(true);      // Show pause menu again
    }
}
 */

