using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHover : MonoBehaviour
{

    [Tooltip("The gameobject of the canvas for the UI of this boat")]
    public GameObject boatCanvas;

    [Tooltip("The slider used to display this boat's health")]
    public Slider healthSlider;

    [Tooltip("The image used to color the slider for this boat's health slider")]
    public Image healthSliderImage;

    [Tooltip("The PlayerBoat (script) that this boat piece will correspond to")]
    public PlayerBoat playerBoat;

    Gradient gradient;

    private void Start()
    {
        gradient = new Gradient();
        GradientColorKey[] gck = new GradientColorKey[2];
        GradientAlphaKey[] gak = new GradientAlphaKey[0];
        gck[0].color = Color.red;
        gck[0].time = 0.0F;
        gck[1].color = Color.green;
        gck[1].time = 1.0F;
        /*
        gak[0].alpha = 0.0F;
        gak[0].time = 1.0F;
        gak[1].alpha = 0.0F;
        gak[1].time = -1.0F;
        */
        gradient.SetKeys(gck, gak);
        healthSlider.value = playerBoat.GetCurrentHealth() / playerBoat.maxHealth;
        healthSliderImage.color = gradient.Evaluate(healthSlider.value);
    }

    private void OnMouseOver()
    {
        healthSlider.value = playerBoat.GetCurrentHealth() / playerBoat.maxHealth;
        healthSliderImage.color = gradient.Evaluate(healthSlider.value);
        boatCanvas.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        boatCanvas.gameObject.SetActive(false);
    }

}
