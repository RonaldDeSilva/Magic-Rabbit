using UnityEngine;
using UnityEngine.Audio;


public class SettingsMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public GameObject PauseMenu;
    public GameObject SettingsMenuUI;
    public static bool GameIsPaused = false;

    void Start()
    {
        SettingsMenuUI = GameObject.Find("SettingsMenuUI");
    }

    public void SetVolume(float volume)
    {

        AudioMixer.SetFloat("Master Volume", volume);
    }
    public void setquality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void BackToGame()
    {
        SettingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        PauseMenu.SetActive(true);


    }

}
