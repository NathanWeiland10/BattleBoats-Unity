using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoat : MonoBehaviour
{

    GameManager gameManager;

    Vector2 mousePos;
    Vector2 barPos;

    public float boatCost;

    public string[] shotSoundEffects;
    public string[] deathSoundEffects;

    public float boatMoveForce;

    public bool friendlyBoat;

    public float captureSpeed;

    public float deathWeightAmount = 10f;

    public Slider healthSlider;

    public CircleCollider2D rangeHitBox;
    public BoxCollider2D boatColliderHitBox;

    public GameObject[] boatPieces;
    public GameObject hullPiece;

    public bool staticCannon;

    public GameObject smokeEffect;

    public GameObject deathWeights;

    public Transform cannonJoint;
    public Transform cannonHullConnection;

    public Transform[] cannonSpawnPoints;

    [Tooltip("The prefab of the projectile for the weapon of this boat")]
    public GameObject cannonBall;

    public float cannonSpread;

    public float minShotDelay;
    public float maxShotDelay;

    public float shotRecoil;
    public float maxHealth;

    float currentHealth;

    bool isDelaying = false;

    bool isDead = false;

    public bool isCapturing;

    float cannonForce;

    Rigidbody2D boatRigidBody;

    public List<PlayerBoat> encounteredEnemies = new List<PlayerBoat>();
    public PlayerBoat currentEnemy = null;

    public PlayerBase encounteredBase = null;

    bool sailRemoved = false;

    void Awake()
    {
        deathWeights.gameObject.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
        boatRigidBody = hullPiece.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        if (!staticCannon) {
            if (friendlyBoat)
            {
                cannonJoint.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                cannonJoint.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Remove later: Bug occured: Enemy boat died but was not removed from list / as current enemy:
        /*
        if (currentEnemy != null && currentEnemy.IsDead())
        {
            UpdateCurrentEnemy();
        }
        */

        if (!isDead)
        {
            if (currentEnemy == null && !isCapturing && encounteredBase == null)
            {
                if (friendlyBoat)
                {
                    // cannonJoint.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    MoveLeft();
                }
                else
                {
                    // cannonJoint.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
                    MoveRight();
                }
            }
            else
            {
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

                FindObjectOfType<AudioManager>().PlayAtPoint(deathSoundEffects[Random.Range(0, deathSoundEffects.Length)], this.transform.position);

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
        hullPiece.GetComponent<Rigidbody2D>().AddForce(Vector3.right * boatMoveForce);
    }

    public void MoveRight()
    {
        hullPiece.GetComponent<Rigidbody2D>().AddForce(Vector3.left * boatMoveForce);
    }

    public bool GetBoatFriendlyStatus()
    {
        return friendlyBoat;
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

            if (!staticCannon) {

                if (friendlyBoat)
                {
                    cannonJoint.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
                }
                else
                {
                    cannonJoint.rotation = Quaternion.Euler(new Vector3(0, 0, -225));
                }

            }

            Transform cannonSpawnPoint = cannonSpawnPoints[Random.Range(0, cannonSpawnPoints.Length)];

            float enemyDist = Mathf.Abs(cannonSpawnPoint.position.x - enemy.GetBoatHullXCoord());

            cannonForce = Mathf.Sqrt(enemyDist * 6200)-(cannonSpawnPoint.position.y*8.5f);

            boatRigidBody.AddForce((cannonSpawnPoint.up) * shotRecoil);

            FindObjectOfType<AudioManager>().PlayAtPoint(shotSoundEffects[Random.Range(0, shotSoundEffects.Length)], this.transform.position);

            Instantiate(smokeEffect, cannonSpawnPoint.position, cannonSpawnPoint.rotation);

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

        if (!staticCannon) {

            if (friendlyBoat)
            {
                cannonJoint.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
            }
            else
            {
                cannonJoint.rotation = Quaternion.Euler(new Vector3(0, 0, -225));
            }
        }

        Transform cannonSpawnPoint = cannonSpawnPoints[Random.Range(0, cannonSpawnPoints.Length)];

        float enemyDist = Mathf.Abs(cannonSpawnPoint.position.x - playerBase.GetXCoord());

        cannonForce = Mathf.Sqrt(enemyDist * 5500);

        boatRigidBody.AddForce((cannonSpawnPoint.up) * shotRecoil);

        FindObjectOfType<AudioManager>().PlayAtPoint(shotSoundEffects[Random.Range(0, shotSoundEffects.Length)], this.transform.position);

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
        return hullPiece.transform.position.x;
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