using UnityEngine;
using UnityEngine.Audio;
using Systems.Collections;
using Systems.Collections.Generic;


public class SettingsMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SetVolume(float volume)
    {
        public AudioMixer addmixer;

    AudioListener.volume = volume;
        Debug.Log("Volume set to: " + volume);
    }
