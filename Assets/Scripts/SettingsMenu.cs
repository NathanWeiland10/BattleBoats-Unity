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

    public ToggleButton showLevelEffectsButton;

    public ToggleButton showBoatEffectsButton;

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

            if (settingsSaver.showLevelParticleEffects)
            {
                showLevelEffectsButton.clicked = true;
                showLevelEffectsButton.image.sprite = showLevelEffectsButton.enabledSprite;
            }
            else
            {
                showLevelEffectsButton.clicked = false;
                showLevelEffectsButton.image.sprite = showLevelEffectsButton.disabledSprite;
            }

            if (settingsSaver.showBoatParticleEffects)
            {
                showBoatEffectsButton.clicked = true;
                showBoatEffectsButton.image.sprite = showBoatEffectsButton.enabledSprite;
            }
            else
            {
                showBoatEffectsButton.clicked = false;
                showBoatEffectsButton.image.sprite = showBoatEffectsButton.disabledSprite;
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

    public void SetShowLevelParticleEffects()
    {
        if (settingsSaver != null)
        {
            settingsSaver.SetShowLevelParticleEffects(showLevelEffectsButton.clicked);
        }
    }

    public void SetShowBoatParticleEffects()
    {
        if (settingsSaver != null)
        {
            settingsSaver.SetShowBoatParticleEffects(showBoatEffectsButton.clicked);
        }
    }

}