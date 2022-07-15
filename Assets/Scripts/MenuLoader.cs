using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoader : MonoBehaviour
{
    public GameObject titleScreen;

    public GameObject levelSelectScreen;

    public GameObject optionsScreen;

    SettingsSaver settingsSaver;

    void Awake()
    {
        settingsSaver = FindObjectOfType<SettingsSaver>();
        if (settingsSaver != null)
        {
            if (settingsSaver.goToLevelSelectMenu)
            {
                Debug.Log("Here");
                LoadlevelSelectScreenn();
            }
        }
    }

    void LoadlevelSelectScreenn()
    {
        titleScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
        optionsScreen.SetActive(false);
        settingsSaver.goToLevelSelectMenu = false;
    }
}
