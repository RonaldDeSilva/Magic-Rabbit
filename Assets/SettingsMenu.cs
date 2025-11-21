using UnityEngine;
using UnityEngine.Audio;


//If player clicks on settings button in pause menu, this script should be activated and allow player to change
//Settings also checks for game to be paused still

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    private GameObject PauseMenu;

    void Start()
    {
        PauseMenu = GameObject.Find("PauseMenu");
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

    public void BackToSettings()
    {
        PauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
