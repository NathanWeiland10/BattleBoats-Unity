using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Tooltip("The amount of damage this cannonball deals to another boat on collision")]
    [SerializeField] float cannonBallDamage;

    [Tooltip("Set enabled if this projectile follows a trajectory (Ex: An arrow) or disabled otherwise")]
    [SerializeField] bool angledProjectile;

    [Tooltip("Set enabled if this projectile is fired by a friendly ship or disabled otherwise")]
    [SerializeField] bool friendly;

    [Tooltip("The effect that is spawned on collision")]
    [SerializeField] GameObject hitEffect;

    [Tooltip("The sound effect that is produced once this cannonball collides with another boat")]
    [SerializeField] string hitSoundEffect;


    public float GetCannonBallDamage()
    {
        return cannonBallDamage;
    }

    void Update()
    {
        if (angledProjectile)
        {
            float yVec = GetComponent<Rigidbody2D>().velocity.y;
            float angle = 0f;
            if (friendly)
            {
                angle = Mathf.Clamp(yVec * 5, -45f, 45f)+270f;
            }
            else
            {
                angle = -Mathf.Clamp(yVec * 5, -45f, 45f)+90f;
            }
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
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
