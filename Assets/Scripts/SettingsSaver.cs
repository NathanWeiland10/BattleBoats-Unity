using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSaver : MonoBehaviour
{

    public float gameVolume;

    public bool goToLevelSelectMenu;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetGoToLevelSelectMenu(bool b)
    {
        goToLevelSelectMenu = b;
    }

}
