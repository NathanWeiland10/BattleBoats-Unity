using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffect : MonoBehaviour
{
    [Tooltip("The sound effect that will play once a button is pressed (needs to be set up in the Button component events menu)")]
    public string soundEffect;

    public void PlaySoundEffect()
    {
        FindObjectOfType<AudioManager>().Play(soundEffect);
    }
}
