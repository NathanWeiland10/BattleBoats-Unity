using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEffect : MonoBehaviour
{
    public string soundEffect;

    public void PlaySoundEffect()
    {
        // Play sound:
        FindObjectOfType<AudioManager>().Play(soundEffect);
    }
}