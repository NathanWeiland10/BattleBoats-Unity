using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{

    public GameObject button;

    public Sprite enabledSprite;

    public Sprite disabledSprite;

    bool clicked;

    Image image;

    SettingsSaver settingsSaver;

    private void Awake()
    {
        settingsSaver = FindObjectOfType<SettingsSaver>();
        image = button.GetComponent<Image>();

        clicked = settingsSaver.showFPS;
        if (clicked)
        {
            image.sprite = enabledSprite;
        }
        else
        {
            image.sprite = disabledSprite;
        }
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
    }

    public void SetShowFPS()
    {
        settingsSaver.SetShowFPS(clicked);
    }

}
