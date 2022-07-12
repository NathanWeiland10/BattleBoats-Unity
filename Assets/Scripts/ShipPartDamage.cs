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
        if (nonRemovablePiece && pieceName.ToLower() != "cannon")
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

                if (playerBoat != null) {
                    float damage = collision.gameObject.GetComponent<CannonBall>().GetCannonBallDamage();
                    playerBoat.TakeDamage(damage);
                    pieceCurrentHealth -= damage;
                }
            }

            if (pieceCurrentHealth <= 0 && !nonRemovablePiece)
            {
                RemovePiece();
            }

            if (playerBoat != null && !playerBoat.IsDead())
            {
                FindObjectOfType<AudioManager>().PlayAtPoint(collision.gameObject.GetComponent<CannonBall>().GetHitSoundEffect(), collision.gameObject.transform.position);

                if (collision.gameObject.GetComponent<CannonBall>().GetHitEffect() != null)
                {
                    Instantiate(collision.gameObject.GetComponent<CannonBall>().GetHitEffect(), collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                }
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.tag == "KamikazeAttack")
        {
            PlayerBoat boat = collision.gameObject.GetComponent<KamikazeAttack>().GetPlayerBoat();

            if (pieceCurrentHealth > 0)
            {
                if (boat != null) {
                    boat.Die();
                }

                if (playerBoat != null)
                {
                    float damage = collision.gameObject.GetComponent<KamikazeAttack>().GetAttackDamage();
                    playerBoat.TakeDamage(damage);
                    pieceCurrentHealth -= damage;
                }
            }

            if (pieceCurrentHealth <= 0 && !nonRemovablePiece)
            {
                RemovePiece();
            }

            if (playerBoat != null && !playerBoat.IsDead())
            {
                FindObjectOfType<AudioManager>().PlayAtPoint(collision.gameObject.GetComponent<KamikazeAttack>().GetHitSoundEffect(), collision.gameObject.transform.position);

                if (collision.gameObject.GetComponent<KamikazeAttack>().GetHitEffect() != null)
                {
                    Instantiate(collision.gameObject.GetComponent<KamikazeAttack>().GetHitEffect(), collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                }
                Destroy(collision.gameObject);
            }
        }
    }

    public void RemovePiece()
    {
        GetComponent<Rigidbody2D>().mass += deathWeightAmount;
        FixedJoint2D joint = GetComponent<FixedJoint2D>();
        if (playerBoat != null)
        {
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
            playerBoat.UpdateBoatSpeed(-speedIncrement, pieceName);
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

    public float GetDeathWeight()
    {
        return deathWeightAmount;
    }

}
