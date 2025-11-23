using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSelection : MonoBehaviour
{
    
    public GameObject SettingsMenu;
    public GameObject MainMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SettingsMenu = GameObject.Find("Settings Menu").transform.GetChild(0).gameObject;
        SettingsMenu.SetActive(false);
        MainMenu = GameObject.Find("Main Menu Canvas").transform.GetChild(0).gameObject;
       
    }

    public void LoadCardMenu()
    {
        SceneManager.LoadScene("Card Menu");

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

}
