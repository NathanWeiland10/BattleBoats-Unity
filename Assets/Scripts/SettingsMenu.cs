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

    public ToggleButton showFPSButton;

    SettingsSaver settingsSaver;

    void Awake()
    {
        settingsSaver = FindObjectOfType<SettingsSaver>();
        if (settingsSaver != null)
        {
            volumeSlider.value = settingsSaver.gameVolume;
            if (settingsSaver.showFPS)
            {
                showFPSButton.clicked = true;
                showFPSButton.image.sprite = showFPSButton.enabledSprite;
            }
            else
            {
                showFPSButton.clicked = false;
                showFPSButton.image.sprite = showFPSButton.disabledSprite;
            }
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

    public void SetShowFPS()
    {
        if (settingsSaver != null)
        {
            settingsSaver.SetShowFPS(showFPSButton.clicked);
        }
    }

}