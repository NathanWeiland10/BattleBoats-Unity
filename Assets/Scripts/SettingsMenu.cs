using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Slider volumeSlider;

    SettingsSaver settingsSaver;

    void Awake()
    {
        settingsSaver = FindObjectOfType<SettingsSaver>();
        if (settingsSaver != null)
        {
            volumeSlider.value = settingsSaver.gameVolume;
        }
    }

    public void SetVolume(float v)
    {
        audioMixer.SetFloat("volume", v);
        if (settingsSaver != null)
        {
            settingsSaver.gameVolume = v;
        }
    }

}