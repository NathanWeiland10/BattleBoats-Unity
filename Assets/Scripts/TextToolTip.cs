using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextToolTip : MonoBehaviour
{
    [Tooltip("The TMP_Text used to display information about a specific boat")]
    public TMP_Text TMPtext;

    public void UpdateText(BoatSelector boat)
    {
        TMPtext.text = "<color=#34ADB1>Name: </color>" + boat.boatName + "<color=#34ADB1>\nDescription: </color>" + boat.boatDescription;
    }

}
