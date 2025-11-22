using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject SettingsMenu;
    

    void Start()
    {
        PauseMenuUI = GameObject.Find("Pause Menu").transform.GetChild(0).gameObject;
        SettingsMenu = GameObject.Find("Settings Menu").transform.GetChild(0).gameObject;
        PauseMenuUI.SetActive(false);
        SettingsMenu.SetActive(false);
    }

    void Update()
    {
        // Toggle pause using P key
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P key pressed, GameIsPaused = " + GameIsPaused);

            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }  // <-- FIXED: closes the P-key block

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

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SettingsMenu.SetActive(false);
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

    public void OpenSettingsMenu()
    {
        SettingsMenu.SetActive(true);
        PauseMenuUI.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        SettingsMenu.SetActive(false);
        PauseMenuUI.SetActive(true);
    }

}


