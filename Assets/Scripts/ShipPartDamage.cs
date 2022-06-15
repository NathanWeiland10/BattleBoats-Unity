using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartDamage : MonoBehaviour
{
    [Tooltip("The name of this boat piece (make sure to include a number so that sails can correspond to a specific mast)")]
    public string pieceName;
    [Tooltip("The amount of move force this boat piece contributes (once this piece is removed, the move force of the boat will be subtracted by this amount)")]
    public float speedIncrement;

    [Tooltip("Set enabled if this piece is a hull piece or cannon piece (so that it cannot be removed until the boat is fully destroyed)")]
    public bool nonRemovablePiece;
    
    [Tooltip("The maximum health for this boat piece (once health reaches 0, this piece will be removed from the boat)")]
    public float pieceMaxHealth;
    [Tooltip("The PlayerBoat (script) that this boat piece will correspond to")]
    public PlayerBoat playerBoat;

    [Tooltip("The amount of mass that will be added to this piece once it has been removed / destroyed")]
    public float deathWeightAmount = 10f;

    float pieceCurrentHealth;

    void Awake()
    {
        if (nonRemovablePiece)
        {
            pieceCurrentHealth = playerBoat.maxHealth;
        }
        else
        {
            pieceCurrentHealth = pieceMaxHealth;
        }  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CannonBall")
        {
            if (pieceCurrentHealth > 0)
            {
                FindObjectOfType<AudioManager>().PlayAtPoint(collision.gameObject.GetComponent<CannonBall>().GetHitSoundEffect(), collision.gameObject.transform.position);

                float damage = collision.gameObject.GetComponent<CannonBall>().GetCannonBallDamage();
                playerBoat.TakeDamage(damage);
                pieceCurrentHealth -= damage;
            }

            if (pieceCurrentHealth <= 0 && !nonRemovablePiece)
            {
                RemovePiece();
            }
        }
        Destroy(collision.gameObject);
    }

    public void RemovePiece()
    {
        // FIX LATER:
        // Removed due to a possible bug, however, there needs to still be a way for opposing team boats to not collide with a dead boat
        // Potentially remove collisions between opposing boats in the collision matrix?
        // -----
        // PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        // collider.enabled = false;
        // -----
        GetComponent<Rigidbody2D>().mass += deathWeightAmount;
        FixedJoint2D joint = GetComponent<FixedJoint2D>();

        if (playerBoat != null) {
            if (pieceName.ToLower().Contains("mast"))
            {
                GameObject[] boatPieces = playerBoat.boatPieces;
                foreach (GameObject b in boatPieces)
                {
                    string name = b.GetComponent<ShipPartDamage>().pieceName;

                    if (name[name.Length - 1] == pieceName[pieceName.Length - 1] && name.ToLower().Contains("sail"))
                    {
                        b.GetComponent<Rigidbody2D>().mass += b.GetComponent<ShipPartDamage>().deathWeightAmount;
                        FixedJoint2D joint2 = b.GetComponent<FixedJoint2D>();
                        Destroy(joint2);
                    }
                }
            }
        }

        Destroy(joint);

        if (pieceName.ToLower().Contains("mast") || pieceName.ToLower().Contains("sail"))
        {
            playerBoat.UpdateBoatSpeed(-speedIncrement);
        }
    }

    public PlayerBoat GetPlayerBoat()
    {
        return playerBoat;
    }
    public float GetPieceCurrentHealth()
    {
        return pieceCurrentHealth;
    }

}
