using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    // TEST
    [SerializeField] float cannonBallDamage;
    [SerializeField] string hitSoundEffect;

    public float GetCannonBallDamage()
    {
        return cannonBallDamage;
    }

    public string GetHitSoundEffect()
    {
        return hitSoundEffect;
    }

}