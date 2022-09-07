using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{

    public GameObject button;

    public Sprite enabledSprite;

    public Sprite disabledSprite;

    public bool clicked;

    public Image image;

    SettingsSaver settingsSaver;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (clicked)
        {
            image.sprite = enabledSprite;
        }
        else
        {
            image.sprite = disabledSprite;
        }
    }

    public void ShowFPSUpdateButtonClick()
    {
        clicked = !clicked;
        if (clicked)
        {
            image.sprite = enabledSprite;
        }
        else
        {
            image.sprite = disabledSprite;
        }

        if (gameManager != null)
        {
            if (clicked)
            {
                gameManager.FPSText.SetActive(true);
            }
            else
            {
                gameManager.FPSText.SetActive(false);
            }
        }
    }

    public void ShowLevelEffectsUpdateButtonClick()
    {
        clicked = !clicked;
        if (clicked)
        {
            image.sprite = enabledSprite;
        }
        else
        {
            image.sprite = disabledSprite;
        }

        if (gameManager != null)
        {
            if (gameManager.levelParticleEffects != null)
            {
                if (clicked)
                {
                    gameManager.levelParticleEffects.SetActive(true);
                }
                else
                {
                    gameManager.levelParticleEffects.SetActive(false);
                }
            }
        }
    }

    public void ShowBoatEffectsUpdateButtonClick()
    {
        clicked = !clicked;
        if (clicked)
        {
            image.sprite = enabledSprite;
        }
        else
        {
            image.sprite = disabledSprite;
        }

        if (gameManager != null)
        {
            if (clicked)
            {
                gameManager.showBoatEffects = true;
            }
            else
            {
                gameManager.showBoatEffects = false;
            }
        }
    }

}
