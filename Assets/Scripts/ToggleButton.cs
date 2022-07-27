using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{

    public GameObject button;

    public Sprite enabledSprite;

    public Sprite disabledSprite;

    public SettingsSaver settingsSaver;

    bool clicked = false;

    Image image;
    
    private void Awake()
    {
        image = button.GetComponent<Image>();
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
