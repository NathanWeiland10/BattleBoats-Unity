using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticleOnCollision : MonoBehaviour
{
    [Tooltip("The particle system that will be instantiated when a collision (OnTriggerEnter2D) occurs with a CannonBall")]
    public ParticleSystem particle;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "CannonBall") {
            Instantiate(particle, new Vector2(collider.transform.position.x, collider.transform.position.y), Quaternion.identity);
        }
    }
}
