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
        SettingsMenuUI = GameObject.Find("Settings Menu").transform.GetChild(0).gameObject;
        PauseMenu = GameObject.Find("Pause Menu").transform.GetChild(0).gameObject;
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
        PauseMenu.SetActive(true);
    }

}
