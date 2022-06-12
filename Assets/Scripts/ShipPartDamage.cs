using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPartDamage : MonoBehaviour
{
    public string pieceName;
    public float speedIncrement;

    public bool isHullPiece;
    public bool isCannonPiece;

    public float pieceMaxHealth;
    public PlayerBoat playerBoat;

    public float deathWeightAmount = 10f;

    float pieceCurrentHealth;

    void Awake()
    {
        if (isHullPiece || isCannonPiece)
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
            Destroy(collision.gameObject);

            if (pieceCurrentHealth > 0)
            {
                FindObjectOfType<AudioManager>().PlayAtPoint(collision.gameObject.GetComponent<CannonBall>().GetHitSoundEffect(), GetComponentInParent<Transform>().position);

                float damage = collision.gameObject.GetComponent<CannonBall>().GetCannonBallDamage();
                playerBoat.TakeDamage(damage);
                pieceCurrentHealth -= damage;
            }

            if (pieceCurrentHealth <= 0 && !isHullPiece)
            {
                RemovePiece();
            }

        }
    }

    public void RemovePiece()
    {
        // PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        // collider.enabled = false;
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