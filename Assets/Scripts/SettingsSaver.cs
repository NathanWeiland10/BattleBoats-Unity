using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSaver : MonoBehaviour
{

    public float gameVolume;

    public bool goToPlayMenu;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetGoToPlayMenu(bool b)
    {
        goToPlayMenu = b;
    }

}
