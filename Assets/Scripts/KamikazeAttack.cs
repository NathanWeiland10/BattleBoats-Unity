using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeAttack : MonoBehaviour
{
    [Tooltip("The amount of damage this attack deals to another boat on collision")]
    [SerializeField] float attackDamage;

    [Tooltip("The sound effect that is produced once this attack collides with another boat")]
    [SerializeField] string hitSoundEffect;

    [Tooltip("The effect that is spawned on collision")]
    [SerializeField] GameObject hitEffect;

    [Tooltip("The boat that is associated with this attack")]
    [SerializeField] PlayerBoat playerBoat;

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public string GetHitSoundEffect()
    {
        return hitSoundEffect;
    }

    public GameObject GetHitEffect()
    {
        return hitEffect;
    }

    public PlayerBoat GetPlayerBoat()
    {
        return playerBoat;
    }

    public void DisableHitBox()
    {
        gameObject.SetActive(false);
    }

}