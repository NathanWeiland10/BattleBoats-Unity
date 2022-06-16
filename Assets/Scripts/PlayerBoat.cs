using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoat : MonoBehaviour
{
    [Tooltip("Set enabled for a friendly boat or disabled for an enemy boat (determines the movement direction)")]
    public bool friendlyBoat;

    [Tooltip("The maximum health for this boat")]
    public float maxHealth;

    [Tooltip("The build cost for this boat")]
    public float boatCost;
    
    [Tooltip("The prefab of the projectile for the weapon of this boat")]
    public GameObject cannonBall;

    [Tooltip("The minimum shot delay for this boat (in seconds)")]
    public float minShotDelay;
    [Tooltip("The maximum shot delay for this boat (in seconds)")]
    public float maxShotDelay;
    [Tooltip("The angle variation for each shot of this boat (in degrees)")]
    public float cannonSpread;
    [Tooltip("The amount of recoil this boat receives when shooting")]
    public float shotRecoil;
    
    // FIX LATER:
    // Only have individual piece death weight amounts, not an overall weight to add to each piece (from the ShipPartDamage component):
    public float deathWeightAmount = 10f;
    
    [Tooltip("The movement force (speed) of this boat")]
    public float boatMoveForce;
    [Tooltip("The amount of capture points this boat adds to a capture point every second")]
    public float captureSpeed;

    [Tooltip("The shot sound effects of this boat")]
    public string[] shotSoundEffects;
    [Tooltip("The death sound effects of this boat")]
    public string[] deathSoundEffects;
    
    [Tooltip("Set enabled if the cannon(s) of this boat do not change angle (E.g. are build into the hull) or disabled if the cannon angle changed (E.g. the cannon of a raft)")]
    public bool staticCannon;

    [Tooltip("The collider this boat uses to determine when a boat or base is within reach and will begin attacking")]
    public CircleCollider2D rangeHitBox;
    [Tooltip("The collider of this boat that (either checks for other boats or for capturing: *** CHECK LATER ***)")]
    public BoxCollider2D boatColliderHitBox;

    [Tooltip("The main hull piece of this boat (used for determining the distance from one boat to another)")]
    public GameObject mainHullPiece;
    [Tooltip("The collider this boat uses to determine when a boat or base is within reach and will begin attacking")]
    public Transform cannonPiece;
    [Tooltip("*** ADD TOOLTIP LATER ***")]
    public Transform cannonHullConnection;
    [Tooltip("The transforms that cannonballs will spawn from for this boat")]
    public Transform[] cannonSpawnPoints;

    [Tooltip("All of the pieces for this boat (including the cannon)")]
    public GameObject[] boatPieces;
    
    [Tooltip("The particle effect that is produced once a cannon is shot for this boat")]
    public GameObject cannonSmokeEffect;

    [Tooltip("The GameObject that holds all of the death weights for this boat; once a boat is destroyed, the deathWeights will become enabled)")]
    public GameObject deathWeights;
    
    [Tooltip("The Slider that shows the current health of this boat")]
    public Slider healthSlider;

    List<PlayerBoat> encounteredEnemies = new List<PlayerBoat>();
    PlayerBoat currentEnemy = null;
    PlayerBase encounteredBase = null;
    bool isCapturing;

    GameManager gameManager;

    Vector2 mousePos;
    Vector2 barPos;
    
    float cannonForce;
    
    bool isDelaying;
    bool isDead;
    
    float currentHealth;

    Rigidbody2D boatRigidBody;

    // FIX LATER:
    // Rather than having a boolean for removed sail, have a list that you can add to so 
    // that you can check to see which sails have been removed and then reduce the speed:
    bool sailRemoved;

    void Awake()
    {
        deathWeights.gameObject.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
        boatRigidBody = mainHullPiece.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (!staticCannon) {
            if (friendlyBoat)
            {
                cannonPiece.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                cannonPiece.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            if (currentEnemy == null && !isCapturing && encounteredBase == null)
            {
                if (friendlyBoat)
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight();
                }
            }
            else
            {
                if (!staticCannon)
                {
                    if (friendlyBoat)
                    {
                        cannonPiece.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
                    }
                    else
                    {
                        cannonPiece.rotation = Quaternion.Euler(new Vector3(0, 0, -225));
                    }
                }
                if (!isDelaying && encounteredBase != null)
                {
                    StartCoroutine(AIFireBase(encounteredBase));
                }
                else if (!isDelaying && currentEnemy != null)
                {
                    StartCoroutine(AIFireBoat(currentEnemy));
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        gameManager.UpdateOtherCurrentEnemy(this);
        deathWeights.gameObject.SetActive(true);
        foreach (GameObject boatPiece in boatPieces)
        {
            if (boatPiece.GetComponent<ShipPartDamage>().GetPieceCurrentHealth() > 0) {
            
                FindObjectOfType<AudioManager>().PlayAtPoint(deathSoundEffects[Random.Range(0, deathSoundEffects.Length)], mainHullPiece.transform.position);

                FixedJoint2D joint = boatPiece.GetComponent<FixedJoint2D>();
                Destroy(joint);
                HingeJoint2D joint2 = boatPiece.GetComponent<HingeJoint2D>();
                Destroy(joint2);

                Destroy(boatColliderHitBox);

                boatPiece.GetComponent<Rigidbody2D>().mass += deathWeightAmount;

                isDead = true;

                Destroy(this);
            }
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void MoveLeft()
    {
        mainHullPiece.GetComponent<Rigidbody2D>().AddForce(Vector3.right * boatMoveForce);
    }

    public void MoveRight()
    {
        mainHullPiece.GetComponent<Rigidbody2D>().AddForce(Vector3.left * boatMoveForce);
    }

    public bool GetBoatFriendlyStatus()
    {
        return friendlyBoat;
    }

    public PlayerBoat getCurrentEnemy()
    {
        return currentEnemy;
    }

    public void AddEncounteredEnemy(PlayerBoat enemy)
    {
        if (!encounteredEnemies.Contains(enemy))
        {
            encounteredEnemies.Add(enemy);
        }
        if (currentEnemy == null)
        {
            currentEnemy = enemy;
        }
    }

    public void UpdateEncounteredBase(PlayerBase playerBase)
    {
        encounteredBase = playerBase;
    }

    public void RemoveEnemyFromList(PlayerBoat b)
    {
        encounteredEnemies.Remove(b);
    }

    public void UpdateCurrentEnemy()
    {
        encounteredEnemies.Remove(currentEnemy);
        if (encounteredEnemies.Count != 0)
        {
            currentEnemy = encounteredEnemies[0];
        }
        else
        {
            currentEnemy = null;
        }
    }

    public void UpdateBoatSpeed(float f)
    {
        if (!sailRemoved) {
            boatMoveForce += f;
            sailRemoved = true;
        }
    }

    IEnumerator AIFireBoat(PlayerBoat enemy)
    {
        // Check if the current enemy boat is behind the boat:
        if (CheckIfBehind(enemy))
        {
            UpdateCurrentEnemy();
        }
        else
        {
            isDelaying = true;

            Transform cannonSpawnPoint = cannonSpawnPoints[Random.Range(0, cannonSpawnPoints.Length)];

            float enemyDist = Mathf.Abs(cannonSpawnPoint.position.x - enemy.GetBoatHullXCoord());

            cannonForce = Mathf.Sqrt(enemyDist * 6200)-(cannonSpawnPoint.position.y*8.5f)-(14* Mathf.Abs(enemy.GetBoatHullRB().velocity.x));

            boatRigidBody.AddForce((cannonSpawnPoint.up) * shotRecoil);

            FindObjectOfType<AudioManager>().PlayAtPoint(shotSoundEffects[Random.Range(0, shotSoundEffects.Length)], mainHullPiece.transform.position);

            Instantiate(cannonSmokeEffect, cannonSpawnPoint.position, cannonSpawnPoint.rotation);

            float recoil = Random.Range(-cannonSpread, cannonSpread);
            cannonSpawnPoint.eulerAngles = new Vector3(cannonSpawnPoint.eulerAngles.x, cannonSpawnPoint.eulerAngles.y, cannonSpawnPoint.eulerAngles.z + recoil);
            GameObject bulletToShoot = Instantiate(cannonBall, cannonSpawnPoint.position, cannonSpawnPoint.rotation);
            Rigidbody2D bulletRB = bulletToShoot.GetComponent<Rigidbody2D>();
            bulletRB.AddForce((-cannonSpawnPoint.up) * cannonForce);
            yield return new WaitForSeconds(Random.Range(minShotDelay, maxShotDelay));
            isDelaying = false;
            cannonSpawnPoint.eulerAngles = new Vector3(cannonSpawnPoint.eulerAngles.x, cannonSpawnPoint.eulerAngles.y, cannonSpawnPoint.eulerAngles.z - recoil);
        }
    }

    IEnumerator AIFireBase(PlayerBase playerBase)
    {
        isDelaying = true;

        Transform cannonSpawnPoint = cannonSpawnPoints[Random.Range(0, cannonSpawnPoints.Length)];

        float enemyDist = Mathf.Abs(cannonSpawnPoint.position.x - playerBase.GetXCoord());

        cannonForce = Mathf.Sqrt(enemyDist * 5500);

        boatRigidBody.AddForce((cannonSpawnPoint.up) * shotRecoil);

        FindObjectOfType<AudioManager>().PlayAtPoint(shotSoundEffects[Random.Range(0, shotSoundEffects.Length)], mainHullPiece.transform.position);

        Instantiate(cannonSmokeEffect, cannonSpawnPoint.position, cannonSpawnPoint.rotation);

        float recoil = Random.Range(-cannonSpread, cannonSpread);
        cannonSpawnPoint.eulerAngles = new Vector3(cannonSpawnPoint.eulerAngles.x, cannonSpawnPoint.eulerAngles.y, cannonSpawnPoint.eulerAngles.z + recoil);
        GameObject bulletToShoot = Instantiate(cannonBall, cannonSpawnPoint.position, cannonSpawnPoint.rotation);
        Rigidbody2D bulletRB = bulletToShoot.GetComponent<Rigidbody2D>();
        bulletRB.AddForce((-cannonSpawnPoint.up) * cannonForce);
        yield return new WaitForSeconds(Random.Range(minShotDelay, maxShotDelay));
        isDelaying = false;
        cannonSpawnPoint.eulerAngles = new Vector3(cannonSpawnPoint.eulerAngles.x, cannonSpawnPoint.eulerAngles.y, cannonSpawnPoint.eulerAngles.z - recoil);
    }

    public bool CheckIfBehind(PlayerBoat enemy)
    {
        if (friendlyBoat)
        {
            if (enemy.GetBoatHullXCoord() - GetBoatHullXCoord() < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (GetBoatHullXCoord() - enemy.GetBoatHullXCoord() < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public PlayerBoat GetCurrentEnemy()
    {
        return currentEnemy;
    }

    public List<PlayerBoat> GetEncounteredEnemies()
    {
        return encounteredEnemies;
    }

    public float GetBoatHullXCoord()
    {
        return mainHullPiece.transform.position.x;
    }

    public Rigidbody2D GetBoatHullRB()
    {
        return mainHullPiece.GetComponent<Rigidbody2D>();
    }

    public bool IsDead()
    {
        if (currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetCaptureSpeed()
    {
        return captureSpeed;
    }

    public void SetIsCapturing(bool b)
    {
        isCapturing = b;
    }

}
