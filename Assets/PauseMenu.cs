using UnityEngine;
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
        if (Input.GetButtonDown("Exit"))
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

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    void LoadCardMenu()
    {
        SceneManager.LoadScene("CardMenu");
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
