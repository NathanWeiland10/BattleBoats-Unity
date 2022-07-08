using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Tooltip("The slider used to display the player's boat spawn progression")]
    public Slider spawnSlider;

    [Tooltip("The list of images used to display the current queue of boat spawn progression")]
    public List<Image> spawnListIcons;

    [Tooltip("The sprite used to indicate this spot in the queue is currently in use")]
    public Sprite inUseIcon;
    [Tooltip("The sprite used to indicate this spot in the queue is currently available")]
    public Sprite emptyIcon;

    [Tooltip("The text used to display the boat that is currently being spawned by the player")]
    public TMP_Text spawnerText;

    [Tooltip("The TMP text that is used to display the player (friendly team's) current total money")]
    public TMP_Text friendlyTotalMoneyText;

    [Tooltip("The list of the capture points in the current scene / map")]
    public CapturePoint[] capturePoints;

    [Tooltip("The time it takes in seconds to add the full amount of money per second to the player (money is added once per second)")]
    [Range(0f, 1f)]
    public float moneyDelayTimer = 0.5f;

    [Tooltip("Used to determine whether or not the game is paused")]
    public bool gamePaused = false;

    [Tooltip("The current speed the game is playing at")]
    public float gameSpeed = 1f;

    public TMP_Text gameSpeedText;

    float moneyTimer = 1; // Initial value serves as an initial wait time for the money to start ticking up
    float waitTime;

    [Tooltip("The list of boats that the player is currently awaiting to spawn")]
    public List<GameObject> friendlySpawnQueue = new List<GameObject>();
    float friendlySpawnTimer = 0f;

    [Tooltip("The list of boats that the enemy is currently awaiting to spawn")]
    public List<GameObject> enemySpawnQueue = new List<GameObject>();
    float enemySpawnTimer = 0f;

    void Awake()
    {
        friendlyMoneyPerSecond += friendlyBase.GetBaseMoneyPerSecond();
        enemyMoneyPerSecond += enemyBase.GetBaseMoneyPerSecond();
        gameHUD.SetActive(true);
        spawnerText.text = "";
        Time.timeScale = 1f;
        gameSpeedText.text = "1.00";
    }

    void Update()
    {
        if (friendlySpawnQueue.Count != 0 && friendlySpawnTimer > 0)
        {
            float fillValue = ((friendlySpawnQueue[0].GetComponent<PlayerBoat>().spawnDelay - friendlySpawnTimer) / friendlySpawnQueue[0].GetComponent<PlayerBoat>().spawnDelay);
            spawnSlider.value = fillValue;
            friendlySpawnTimer -= Time.deltaTime;
        }
        else if (friendlySpawnQueue.Count != 0 && friendlySpawnTimer <= 0)
        {
            InstantiateFriendlyBoat(friendlySpawnQueue[0]);
            friendlySpawnQueue.RemoveAt(0);
            if (friendlySpawnQueue.Count != 0)
            {
                // Start spawning the next boat:
                friendlySpawnTimer = friendlySpawnQueue[0].GetComponent<PlayerBoat>().spawnDelay;
                spawnerText.text = "Spawning: " + friendlySpawnQueue[0].GetComponent<PlayerBoat>().boatName;
            }
            else
            {
                spawnSlider.value = 0f;
                friendlySpawnTimer = 0f;
                spawnerText.text = "";
            }
            // Update the spawn queue:
            for (int i = 0; i < spawnListIcons.Count; i++)
            {
                if (i < friendlySpawnQueue.Count)
                {
                    spawnListIcons[i].sprite = inUseIcon;
                }
                else
                {
                    spawnListIcons[i].sprite = emptyIcon;
                }
            }
        }

        if (enemySpawnQueue.Count != 0 && enemySpawnTimer > 0)
        {
            enemySpawnTimer -= Time.deltaTime;
        }
        else if (enemySpawnQueue.Count != 0 && enemySpawnTimer <= 0)
        {
            InstantiateEnemyBoat(enemySpawnQueue[0]);
            enemySpawnQueue.RemoveAt(0);
            if (enemySpawnQueue.Count != 0)
            {
                // Start spawning the next boat:
                enemySpawnTimer = enemySpawnQueue[0].GetComponent<PlayerBoat>().spawnDelay;
            }
            else
            {
                enemySpawnTimer = 0f;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !gamePaused)
        {
            if (mainCamera.orthographicSize - cameraScrollSpeed > minCameraSize)
            {
                mainCamera.orthographicSize -= cameraScrollSpeed / gameSpeed;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && !gamePaused)
        {
            if (mainCamera.orthographicSize + cameraScrollSpeed < maxCameraSize)
            {
                mainCamera.orthographicSize += cameraScrollSpeed / gameSpeed;
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

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
            {
                mainCameraRig.GetComponent<Rigidbody2D>().AddForce((Vector3.left * cameraMoveSpeed * 3) / gameSpeed);
            }
            else
            {
                mainCameraRig.GetComponent<Rigidbody2D>().AddForce((Vector3.left * cameraMoveSpeed) / gameSpeed);
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
            {
                mainCameraRig.GetComponent<Rigidbody2D>().AddForce((Vector3.right * cameraMoveSpeed * 3) / gameSpeed);
            }
            else
            {
                mainCameraRig.GetComponent<Rigidbody2D>().AddForce((Vector3.right * cameraMoveSpeed) / gameSpeed);
            }
        }
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = gameSpeed;
    }

    public void IncreaseGameSpeed()
    {
        if (gameSpeed != 3f)
        {
            gameSpeed += 0.25f;
            switch (gameSpeed)
            {
                case 1:
                    gameSpeedText.text = "1.00";
                    break;
                case 1.5f:
                    gameSpeedText.text = "1.50";
                    break;
                case 2:
                    gameSpeedText.text = "2.00";
                    break;
                case 2.5f:
                    gameSpeedText.text = "2.50";
                    break;
                case 3:
                    gameSpeedText.text = "3.00";
                    break;
                default:
                    gameSpeedText.text = gameSpeed + "";
                    break;
            }
        }
    }

    public void DecreaseGameSpeed()
    {
        if (gameSpeed != .75f)
        {
            gameSpeed -= 0.25f;
            switch (gameSpeed)
            {
                case 1:
                    gameSpeedText.text = "1.00";
                    break;
                case 1.5f:
                    gameSpeedText.text = "1.50";
                    break;
                case 2:
                    gameSpeedText.text = "2.00";
                    break;
                case 2.5f:
                    gameSpeedText.text = "2.50";
                    break;
                case 3:
                    gameSpeedText.text = "3.00";
                    break;
                default:
                    gameSpeedText.text = gameSpeed + "";
                    break;
            }
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
        if (friendlySpawnQueue.Count == 0)
        {
            friendlySpawnTimer = boat.GetComponent<PlayerBoat>().spawnDelay;
            friendlySpawnQueue.Add(boat);
            spawnerText.text = "Spawning: " + friendlySpawnQueue[0].GetComponent<PlayerBoat>().boatName;
        }
        else
        {
            friendlySpawnQueue.Add(boat);
        }
        // Update the spawn queue:
        for (int i = 0; i < spawnListIcons.Count; i++)
        {
            if (i < friendlySpawnQueue.Count)
            {
                spawnListIcons[i].sprite = inUseIcon;
            }
            else
            {
                spawnListIcons[i].sprite = emptyIcon;
            }
        }
    }

    public void SpawnEnemyBoat(GameObject boat)
    {
        if (enemySpawnQueue.Count == 0)
        {
            enemySpawnTimer = boat.GetComponent<PlayerBoat>().spawnDelay;
            enemySpawnQueue.Add(boat);
        }
        else
        {
            enemySpawnQueue.Add(boat);
        }
    }

    public void InstantiateFriendlyBoat(GameObject boat)
    {
        GameObject spawnedBoat = Instantiate(boat, friendlyBoatSpawn.position, friendlyBoatSpawn.rotation);
        friendlyBoats.Add(spawnedBoat.GetComponent<PlayerBoat>());
    }

    public void InstantiateEnemyBoat(GameObject boat)
    {

        GameObject spawnedBoat = Instantiate(boat, enemyBoatSpawn.position, Quaternion.Euler(0, 179.9999f, 0));
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

        if (friend)
        {
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
