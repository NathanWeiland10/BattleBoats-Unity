using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Code referenced from: https://owlcation.com/stem/How-to-fade-out-a-GameObject-in-Unity#:~:text=Unity%20owned%20and%20developed%20by%20Unity%20Technologies.%20You,GameObjects%20you%20know%20should%20be%20fully-%20or%20semi-transparent.

public class ToolTip : MonoBehaviour
{
  // FIX LATER:
  // Make sure to put this script over the thing you want to spawn the tooltip (Ex: Put this on a button and use the tooltip as the var listed here)
  // "Renderer" may need to be changed to SpriteRenderer later as well
  [Tooltip("The GameObject of the tooltip that will fade in upon hovering over this object")]
  public GameObject toolTip;
  
  [Tooltip("The speed at which the tooltip will fade in and out")]
  public float fadeSpeed = 0.8f;
  
  bool fadeIn, fadeOut;
  
  void Update() {
    if (fadeOut) {
      Color objectColor = toolTip.GetComponent<Renderer>().material.color;
      float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
      
      objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
      toolTip.GetComponent<Renderer>().material.color = objectColor;
      
      if (objectColor.a <= 0) {
        fadeOut = false;
      }
   } else if (fadeIn) {
      Color objectColor = toolTip.GetComponent<Renderer>().material.color;
      float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
      
      objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
      toolTip.GetComponent<Renderer>().material.color = objectColor;
      
      if (objectColor.a <= 0) {
        fadeIn = false;
      }
   }
  }
  
  void OnMouseOver() {
    fadeIn = true;
    fadeOut = false;
  }
  
  void OnMouseExit() {
    fadeIn = false;
    fadeOut = true;
  }
  
}
