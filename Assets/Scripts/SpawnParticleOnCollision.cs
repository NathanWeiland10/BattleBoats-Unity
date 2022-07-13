using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticleOnCollision : MonoBehaviour
{
    [Tooltip("The particle system that will be instantiated when a collision (OnTriggerEnter2D) occurs with a CannonBall")]
    public ParticleSystem particle;

    [Tooltip("The sound effect that will play once a collision occurs")]
    public string soundEffect;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "CannonBall")
        {

            if (collider.gameObject.transform.position.y >= this.gameObject.transform.position.y)
            {

                if (particle != null)
                {
                    Instantiate(particle, new Vector2(collider.transform.position.x, collider.transform.position.y), Quaternion.identity);
                }
                if (soundEffect != null)
                {
                    FindObjectOfType<AudioManager>().PlayAtPoint(soundEffect, collider.gameObject.transform.position);
                }
            }

        }
    }
}