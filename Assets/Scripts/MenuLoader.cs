using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoader : MonoBehaviour
{
    public GameObject titleScreen;

    public GameObject playScreen;

    public GameObject optionsScreen;

    SettingsSaver settingsSaver;

    void Awake()
    {
        settingsSaver = FindObjectOfType<SettingsSaver>();
        if (settingsSaver != null)
        {
            if (settingsSaver.goToPlayMenu)
            {
                LoadPlayScreen();
            }
        }
    }

    void LoadPlayScreen()
    {
        titleScreen.SetActive(false);
        playScreen.SetActive(true);
        optionsScreen.SetActive(false);
        settingsSaver.goToPlayMenu = false;
    }
}
