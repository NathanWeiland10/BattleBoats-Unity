using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSaver : MonoBehaviour
{

    public float gameVolume;

    public bool goToLevelSelectMenu;

    public static SettingsSaver instance;

    void Awake()
    {
        if (instance != null)
        {
            goToLevelSelectMenu = instance.goToLevelSelectMenu;
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetGoToLevelSelectMenu(bool b)
    {
        goToLevelSelectMenu = b;
    }

}
