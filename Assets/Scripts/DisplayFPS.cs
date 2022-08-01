using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayFPS : MonoBehaviour
{

    float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    public TMP_Text displayText;

    void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0)
        {
            avgFramerate = (int)(1f / timelapse);
        }
        displayText.text = string.Format(display, avgFramerate.ToString());
    }

}