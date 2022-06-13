using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeHitBox : MonoBehaviour
{
    [Tooltip("The PlayerBoat (script) that this hitbox will correspond to")]
    public PlayerBoat playerBoat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BoatCaptureHitBox boat = collision.gameObject.GetComponent<BoatCaptureHitBox>();
        if (boat != null)
        {
            if (boat.GetPlayerBoat().GetBoatFriendlyStatus() != playerBoat.GetBoatFriendlyStatus() && (!playerBoat.IsDead() && !boat.GetPlayerBoat().IsDead()))
            {
                playerBoat.AddEncounteredEnemy(boat.GetPlayerBoat());
            }
        }

        PlayerBase playerBase = collision.gameObject.GetComponent<PlayerBase>();
        if (playerBase != null)
        {
            if (playerBase.GetBaseFriendlyStatus() != playerBoat.GetBoatFriendlyStatus() && (!playerBoat.IsDead() && !playerBase.IsDead()))
            {
                playerBoat.UpdateEncounteredBase(playerBase);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BoatCaptureHitBox boat = collision.gameObject.GetComponent<BoatCaptureHitBox>();
        if (boat != null)
        {
            if (boat.GetPlayerBoat().GetBoatFriendlyStatus() != playerBoat.GetBoatFriendlyStatus() && (!playerBoat.IsDead() && !boat.GetPlayerBoat().IsDead()))
            {
                playerBoat.UpdateCurrentEnemy();
            }
        }

        PlayerBase playerBase = collision.gameObject.GetComponent<PlayerBase>();
        if (playerBase != null)
        {
            if (playerBase.GetBaseFriendlyStatus() != playerBoat.GetBoatFriendlyStatus() && (!playerBoat.IsDead() && !playerBase.IsDead()))
            {
                playerBoat.UpdateEncounteredBase(null);
            }
        }
    }
    
    // FIX LATER:
    // Added due to a bug with a boat not updating its current enemy; maybe replace this later with the OnTriggerEnter2D
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerBoat.currentEnemy == null) {
            BoatCaptureHitBox boat = collision.gameObject.GetComponent<BoatCaptureHitBox>();
            if (boat != null)
            {
                if (boat.GetPlayerBoat().GetBoatFriendlyStatus() != playerBoat.GetBoatFriendlyStatus() && (!playerBoat.IsDead() && !boat.GetPlayerBoat().IsDead()))
                {
                    playerBoat.AddEncounteredEnemy(boat.GetPlayerBoat());
                }
            }

            PlayerBase playerBase = collision.gameObject.GetComponent<PlayerBase>();
            if (playerBase != null)
            {
                if (playerBase.GetBaseFriendlyStatus() != playerBoat.GetBoatFriendlyStatus() && (!playerBoat.IsDead() && !playerBase.IsDead()))
                {
                    playerBoat.UpdateEncounteredBase(playerBase);
                }
            }
        }
    }

}
