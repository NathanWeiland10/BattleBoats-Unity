using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    [Tooltip("The sound effect that will play once a collision occurs")]
    public string soundEffect;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "CannonBall")
        {
            FindObjectOfType<AudioManager>().PlayAtPoint(soundEffect, collider.gameObject.transform.position);
        }
    }
}