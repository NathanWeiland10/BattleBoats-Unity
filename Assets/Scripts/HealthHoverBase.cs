using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHoverBase : MonoBehaviour
{
    [Tooltip("The gameobject of the canvas for the UI of this base")]
    public GameObject baseCanvas;

    [Tooltip("The slider used to display this base's health")]
    public Slider healthSlider;

    [Tooltip("The image used to color the slider for this base's health slider")]
    public Image healthSliderImage;

    [Tooltip("The PlayerBase (script) that this script will correspond to")]
    public PlayerBase playerBase;

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

        gradient.SetKeys(gck, gak);
        healthSlider.value = playerBase.GetBaseHealth() / playerBase.maxBaseHealth;
        healthSliderImage.color = gradient.Evaluate(healthSlider.value);

        baseCanvas.gameObject.SetActive(true);
    }

    public void UpdateHealthBar()
    {
        healthSlider.value = playerBase.GetBaseHealth() / playerBase.maxBaseHealth;
        healthSliderImage.color = gradient.Evaluate(healthSlider.value);
    }

}
