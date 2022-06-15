using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Tooltip("Set enabled for a friendly base or disabled for an enemy base (determines the movement direction)")]
    public bool friendlyBase;
    [Tooltip("The amount of money per second this base currently makes (initial / starting value can be set here)")]
    public float moneyPerSecond;
    [Tooltip("The maximum health for this base")]
    public float maxBaseHealth;
    [Tooltip("The sound effect that will play once this base has been destroyed")]
    public string deathSoundEffect;

    GameManager gameManager;
    
    float currentBaseHealth;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentBaseHealth = maxBaseHealth;    
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CannonBall")
        {
            Destroy(collision.gameObject);

            if (currentBaseHealth > 0)
            {
                FindObjectOfType<AudioManager>().PlayAtPoint(collision.gameObject.GetComponent<CannonBall>().GetHitSoundEffect(), this.transform.position);
                float damage = collision.gameObject.GetComponent<CannonBall>().GetCannonBallDamage();
                ChangeBaseHealth(-damage);
            }
            else
            {
                Die();
            }
        }
    }

    public void ChangeBaseHealth(float f)
    {
        currentBaseHealth += f;

        if (currentBaseHealth > maxBaseHealth)
        {
            currentBaseHealth = maxBaseHealth;
        }
        else if (currentBaseHealth < 0)
        {
            currentBaseHealth = 0;
        }
    }

    public float GetBaseHealth()
    {
        return currentBaseHealth;
    }

    public bool GetBaseFriendlyStatus()
    {
        return friendlyBase;
    }

    public bool IsDead()
    {
        if (currentBaseHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetXCoord()
    {
        return transform.position.x;
    }

    public void Die()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Destroy(collider);

        if (friendlyBase)
        {
            gameManager.EnemyWin();
        }
        else
        {
            gameManager.FriendlyWin();
        }

        FindObjectOfType<AudioManager>().Play(deathSoundEffect);
        // FIX LATER:
        // Currently will only destroy the script (might be useful if decided to change sprite to 'death sprite' as to not delete the whole game object)
        Destroy(this); 
    }

    public float GetBaseMoneyPerSecond()
    {
        return moneyPerSecond;
    }

    public void AddBaseMoneyPerSecond(float f)
    {
        moneyPerSecond += f;
    }

}
