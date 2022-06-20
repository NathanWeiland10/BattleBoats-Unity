using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Tooltip("The GameObject that serves as / holds the main camera")]
    public GameObject mainCameraRig;
    [Tooltip("The main camera for the scene")]
    public Camera mainCamera;

    [Tooltip("The PlayerBase (script) for the friendly team's base")]
    public PlayerBase friendlyBase;
    [Tooltip("The PlayerBase (script) for the enemy team's base")]
    public PlayerBase enemyBase;

    [Tooltip("The amount of money per second the friendly team currently makes (initial / starting value can be set here)")]
    public float friendlyMoneyPerSecond = 0;
    [Tooltip("The amount of money per second the enemy team currently makes (initial / starting value can be set here)")]
    public float enemyMoneyPerSecond = 0;

    [Tooltip("The total amount of money the friendly team currently has (initial / starting value can be set here)")]
    public float friendlyTotalMoney = 0;
    [Tooltip("The total amount of money the enemy team currently has (initial / starting value can be set here)")]
    public float enemyTotalMoney = 0;

    [Tooltip("The list of all the friendly boats currently on the map")]
    public List<PlayerBoat> friendlyBoats;
    [Tooltip("The list of all the enemy boats currently on the map")]
    public List<PlayerBoat> enemyBoats;

    [Tooltip("The max distance the camera can be scrolled in")]
    public float minCameraSize;
    [Tooltip("The max distance the camera can be scrolled out")]
    public float maxCameraSize;
    [Tooltip("The speed at which the camera scrolls in and out")]
    public float cameraScrollSpeed;

    [Tooltip("The speed at which the camera moves around the map")]
    public float cameraMoveSpeed;

    [Tooltip("The transform that friendly boats will be spawned at")]
    public Transform friendlyBoatSpawn;
    [Tooltip("The transform that enemy boats will be spawned at")]
    public Transform enemyBoatSpawn;

    [Tooltip("The HUD for the game (can be switched to enabled or disabled)")]
    public GameObject gameHUD;
    
    [Tooltip("The TMP text that is used to display the player (friendly team's) current total money")]
    public TMP_Text friendlyTotalMoneyText;

    [Tooltip("The list of the capture points in the current scene / map")]
    public CapturePoint[] capturePoints;

    [Tooltip("The time it takes in seconds to add the full amount of money per second to the player (money is added once per second)")]
    [Range(0f, 1f)]
    public float moneyDelayTimer = 0.5f;

    float moneyTimer = 1; // Initial value serves as an initial wait time for the money to start ticking up
    float waitTime;

    void Awake()
    {
        friendlyMoneyPerSecond += friendlyBase.GetBaseMoneyPerSecond();
        enemyMoneyPerSecond += enemyBase.GetBaseMoneyPerSecond();
        gameHUD.SetActive(true);    
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
            {
                mainCameraRig.GetComponent<Rigidbody2D>().AddForce(Vector3.left * cameraMoveSpeed * 3);
            }
            else
            {
                mainCameraRig.GetComponent<Rigidbody2D>().AddForce(Vector3.left * cameraMoveSpeed);
            }
        }    
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
            {
                mainCameraRig.GetComponent<Rigidbody2D>().AddForce(Vector3.right * cameraMoveSpeed * 3);
            }
            else
            {
                mainCameraRig.GetComponent<Rigidbody2D>().AddForce(Vector3.right * cameraMoveSpeed);
            }
        }
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (mainCamera.orthographicSize - cameraScrollSpeed > minCameraSize)
            {
                mainCamera.orthographicSize -= cameraScrollSpeed;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (mainCamera.orthographicSize + cameraScrollSpeed < maxCameraSize)
            {
                mainCamera.orthographicSize += cameraScrollSpeed;
            }
        }

        moneyTimer -= Time.deltaTime;
        if (moneyTimer <= 0)
        {
            waitTime = (moneyDelayTimer / friendlyMoneyPerSecond);
            StartCoroutine(AddDelayedMoney(friendlyMoneyPerSecond));
            UpdateEnemyMoney(enemyMoneyPerSecond);
            moneyTimer = 1;
        }
    }

    public void UpdateOtherCurrentEnemy(PlayerBoat boat)
    {
        if (boat.GetBoatFriendlyStatus())
        {
            friendlyBoats.Remove(boat);
            // If the boat that was killed was a friendly boat, check the enemy boats and find its current enemy:
            if (enemyBoats.Count < 0)
            {
                foreach (PlayerBoat b in enemyBoats)
                {
                    if (b.GetCurrentEnemy() == boat)
                    {
                        // Update the boat's current enemy:
                        b.UpdateCurrentEnemy();
                    }
                    // Check if the boat that was killed was in the encounteredEnemyList:
                    if (b.GetEncounteredEnemies().Contains(boat))
                    {
                        b.RemoveEnemyFromList(boat);
                    }
                }
            }
        }
        else
        {
            enemyBoats.Remove(boat);
            // If the boat that was killed was a enemy boat, check the friendly boats and find its current enemy:
            if (friendlyBoats.Count < 0)
            {
                foreach (PlayerBoat b in friendlyBoats)
                {
                    if (b.GetCurrentEnemy() == boat)
                    {
                        // Update the boat's current enemy:
                        b.UpdateCurrentEnemy();
                    }
                    // Check if the boat that was killed was in the encounteredEnemyList:
                    if (b.GetEncounteredEnemies().Contains(boat))
                    {
                        b.RemoveEnemyFromList(boat);
                    }
                }
            }
        }
    }

    public void SpawnFriendlyBoat(GameObject boat)
    {
        GameObject spawnedBoat = Instantiate(boat, friendlyBoatSpawn.position, friendlyBoatSpawn.rotation);
        friendlyBoats.Add(spawnedBoat.GetComponent<PlayerBoat>());
    }

    public void SpawnEnemyBoat(GameObject boat)
    {
        GameObject spawnedBoat = Instantiate(boat, enemyBoatSpawn.position, enemyBoatSpawn.rotation);
        enemyBoats.Add(spawnedBoat.GetComponent<PlayerBoat>());
    }

    public void UpdateFriendlyMoney(float f)
    {
        friendlyTotalMoney += f;
        friendlyTotalMoneyText.text = "$" + friendlyTotalMoney.ToString();
    }

    public void UpdateEnemyMoney(float f)
    {
        enemyTotalMoney += f;
    }

    public void UpdateFriendlyMoneyPerSecond(float f)
    {
        friendlyMoneyPerSecond += f;
    }

    public void UpdateEnemyMoneyPerSecond(float f)
    {
        enemyMoneyPerSecond += f;
    }

    public float GetFriendlyTotalMoney()
    {
        return friendlyTotalMoney;
    }

    public float GetEnemyTotalMoney()
    {
        return enemyTotalMoney;
    }

    IEnumerator AddDelayedMoney(float f)
    {
        if (f != 0f)
        {
            yield return new WaitForSeconds(waitTime);
            friendlyTotalMoney += 1;
            friendlyTotalMoneyText.text = "$" + friendlyTotalMoney.ToString();
            StartCoroutine(AddDelayedMoney(f - 1));
        }
    }

    public void UpdateCapture(bool b, bool friend, CapturePoint capture)
    {
        CapturePoint capturePoint = null;
        foreach (CapturePoint c in capturePoints) 
        {
            if (c == capture)
            {
                capturePoint = c;
                break;
            }
        }
        
        if (friend) {
            // Check to see if a new bool value is being used:
            if (b != capturePoint.IsFriendlyCaptured())
            {
                // If b is true, then increase the money per second for the friendly team:
                if (b == true)
                {
                    friendlyMoneyPerSecond += capturePoint.GetMoneyPerSecond();
                    if (capturePoint.IsEnemyCaptured())
                    {
                        enemyMoneyPerSecond -= capturePoint.GetMoneyPerSecond();
                    }
                    capturePoint.SetFriendlyCaptured();
                }
                else
                {
                    friendlyMoneyPerSecond -= capturePoint.GetMoneyPerSecond();
                    if (!capturePoint.IsEnemyCaptured())
                    {
                        enemyMoneyPerSecond += capturePoint.GetMoneyPerSecond();
                    }
                    capturePoint.SetEnemyCaptured();
                }
            }
        } 
        else
        {
            // Check to see if a new bool value is being used:
            if (b != capturePoint.IsEnemyCaptured())
            {
                // If b is true, then increase the money per second for the friendly team:
                if (b == true)
                {
                    enemyMoneyPerSecond += capturePoint.GetMoneyPerSecond();
                    if (capturePoint.IsFriendlyCaptured())
                    {
                        friendlyMoneyPerSecond -= capturePoint.GetMoneyPerSecond();
                    }
                    capturePoint.SetEnemyCaptured();
                }
                else
                {
                    enemyMoneyPerSecond -= capturePoint.GetMoneyPerSecond();
                    if (!capturePoint.IsFriendlyCaptured())
                    {
                        friendlyMoneyPerSecond += capturePoint.GetMoneyPerSecond();
                    }
                    capturePoint.SetFriendlyCaptured();
                }
            }
        }
    }

    public void FriendlyWin()
    {
        // Do victory screen here:
        Debug.Log("Victory!");
    }

    public void EnemyWin()
    {
        // Do game over / defeat screen here:
        Debug.Log("Defeat!");
    }

}
