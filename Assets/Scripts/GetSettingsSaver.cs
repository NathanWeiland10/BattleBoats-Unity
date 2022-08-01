using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSettingsSaver : MonoBehaviour
{
    SettingsSaver settingsSaver;

    void Awake()
    {
        settingsSaver = FindObjectOfType<SettingsSaver>();

        // Create an On-Click event on the button attached to this gameobject:
        this.GetComponent<Button>().onClick.AddListener(delegate{settingsSaver.SetGoToLevelSelectMenu(true);});
    }

}
