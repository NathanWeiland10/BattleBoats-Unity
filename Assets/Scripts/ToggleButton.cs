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
    }

    public void UpdateButtonClick()
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

}
