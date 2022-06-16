using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Tooltip("The amount of damage this cannonball deals to another boat on collision")]
    [SerializeField] float cannonBallDamage;
    
    [Tooltip("The sound effect that is produced once this cannonball collides with another boat")]
    [SerializeField] string hitSoundEffect;

    [Tooltip("The effect that is spawned on collision")]
    [SerializeField] GameObject hitEffect;

    public float GetCannonBallDamage()
    {
        return cannonBallDamage;
    }

    public string GetHitSoundEffect()
    {
        return hitSoundEffect;
    }

    public GameObject GetHitEffect()
    {
        return hitEffect;
    }

}
