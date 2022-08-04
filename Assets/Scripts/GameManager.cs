using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public CinematicFireworks friendlyFireworks;

    public CinematicFireworks enemyFireworks;

    public LevelLoader menuLoader;

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

    [Tooltip("The multiplier used to determine the amount of additional hitpoints added to a spawned friendly boat")]
    public float friendlyBoatHealthMultiplier = 1.0f;
    [Tooltip("The multiplier used to determine the amount of additional hitpoints added to a spawned enemy boat")]
    public float enemyBoatHealthMultiplier = 1.0f;

    [Tooltip("The multiplier used to determine the time it takes for a friendly boat to spawn")]
    public float friendlySpawnTimeMultiplier = 1.0f;
    [Tooltip("The multiplier used to determine the time it takes for an enemy boat to spawn")]
    public float enemySpawnTimeMultiplier = 1.0f;

    [Tooltip("The multiplier used to determine how much money per second the friendly team makes")]
    public float friendlyMPSMultiplier = 1.0f;
    [Tooltip("The multiplier used to determine how much money per second the friendly team makes")]
    public float enemyMPSMultiplier = 1.0f;

    [Tooltip("The multiplier used to determine how much money the friendly team receives when killing an enemy boat")]
    public float friendlyLootMultiplier = 1.0f;
    [Tooltip("The multiplier used to determine how much money the friendly team receives when killing an enemy boat")]
    public float enemyLootMultiplier = 1.0f;

    [Tooltip("The list of all the friendly boats currently on the map")]
    public List<PlayerBoat> friendlyBoats;
    [Tooltip("The list of all the enemy boats currently on the map")]
    public List<PlayerBoat> enemyBoats;

    [Tooltip("The number of friendly boats on screen")]
    public int friendlyBoatCount;
    [Tooltip("The number of enemy boats on screen")]
    public int enemyBoatCount;

    [Tooltip("The maximum number of boats allowed on screen for the friendly team")]
    public int friendlyMaxBoats;
    [Tooltip("The maximum number of boats allowed on screen for the enemy team")]
    public int enemyMaxBoats;

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

    [Tooltip("The text used to display the number of friendly boats spawned in-game")]
    public TMP_Text friendlyBoatCountText;

    [Tooltip("The TMP text that is used to display the player (friendly team's) current total money")]
    public TMP_Text friendlyTotalMoneyText;

    [Tooltip("The list of the capture points in the current scene / map")]
    public CapturePoint[] capturePoints;

    [Tooltip("The time it takes in seconds to add the full amount of money per second to the player (money is added once per second)")]
    [Range(0f, 1f)]
    public float moneyDelayTimer = 0.5f;

    public List<GameObject> pauseSubMenus;

    [Tooltip("Used to determine whether or not the game is paused")]
    public bool gamePaused = false;

    [Tooltip("The current speed the game is playing at")]
    public float gameSpeed = 1f;

    [Tooltip("Determines whether friendly boats will spawned stopped or not")]
    public bool spawnFriendlyStopped = false;

    public TMP_Text gameSpeedText;

    public GameObject FPSText;

    public GameObject levelParticleEffects;

    public GameObject victoryScreen;
    public GameObject defeatScreen;
    public float screenTime = 3f;

    public GameObject pauseButton;

    public GameObject gameScreen;

    public GameObject pauseScreen;

    public bool gameEnded = false;

    float startDelay = 1;
    public bool gameStarted;

    public bool showBoatEffects = true;

    public GameObject postScreenCanvas;

    float moneyTimer = 1; // Initial value serves as an initial wait time for the money to start ticking up
    float waitTime;

    bool friendlyVictory;

    bool manualQuit = false;

    [Tooltip("The list of boats that the player is currently awaiting to spawn")]
    public List<GameObject> friendlySpawnQueue = new List<GameObject>();
    float friendlySpawnTimer = 0f;

    [Tooltip("The list of boats that the enemy is currently awaiting to spawn")]
    public List<GameObject> enemySpawnQueue = new List<GameObject>();
    float enemySpawnTimer = 0f;

    SettingsSaver settingsSaver;

    void Awake()
    {
        settingsSaver = FindObjectOfType<SettingsSaver>();

        friendlyMoneyPerSecond += friendlyBase.GetBaseMoneyPerSecond();
        enemyMoneyPerSecond += enemyBase.GetBaseMoneyPerSecond();
        gameHUD.SetActive(true);
        spawnerText.text = "";
        Time.timeScale = 1f;

        friendlyTotalMoneyText.text = "$" + friendlyTotalMoney;

        if (settingsSaver != null)
        {
            Time.timeScale = settingsSaver.savedGameSpeed;
            gameSpeed = settingsSaver.savedGameSpeed;

            if (settingsSaver.showFPS)
            {
                FPSText.SetActive(true);
            }
            else
            {
                FPSText.SetActive(false);
            }

            if (levelParticleEffects != null) {
                if (settingsSaver.showLevelParticleEffects)
                {
                    levelParticleEffects.SetActive(true);
                }
                else
                {
                    levelParticleEffects.SetActive(false);
                }
            }

            if (settingsSaver.showBoatParticleEffects)
            {
                showBoatEffects = true;
            }
            else
            {
                showBoatEffects = false;
            }
        }
        else
        {
            Time.timeScale = 1f;
        }

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

    void Start()
    {
        BoatSelectionManager selman = FindObjectOfType<BoatSelectionManager>();
        if (selman != null)
        {
            Destroy(selman.gameObject);
        }

        StartCoroutine(StartDelay());
    }

    void Update()
    {
        if (friendlySpawnQueue.Count != 0 && friendlySpawnTimer > 0)
        {
            float fillValue = (((friendlySpawnQueue[0].GetComponent<PlayerBoat>().spawnDelay / friendlySpawnTimeMultiplier) - friendlySpawnTimer) / (friendlySpawnQueue[0].GetComponent<PlayerBoat>().spawnDelay / friendlySpawnTimeMultiplier));
            spawnSlider.value = fillValue;
            friendlySpawnTimer -= Time.deltaTime;
        }
        else if (friendlySpawnQueue.Count != 0 && friendlySpawnTimer <= 0 && !gameEnded)
        {
            InstantiateFriendlyBoat(friendlySpawnQueue[0]);
            friendlySpawnQueue.RemoveAt(0);
            if (friendlySpawnQueue.Count != 0)
            {
                // Start spawning the next boat:
                friendlySpawnTimer = friendlySpawnQueue[0].GetComponent<PlayerBoat>().spawnDelay / friendlySpawnTimeMultiplier;
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
        else if (enemySpawnQueue.Count != 0 && enemySpawnTimer <= 0 && !gameEnded)
        {
            InstantiateEnemyBoat(enemySpawnQueue[0]);
            enemySpawnQueue.RemoveAt(0);
            if (enemySpawnQueue.Count != 0)
            {
                // Start spawning the next boat:
                enemySpawnTimer = enemySpawnQueue[0].GetComponent<PlayerBoat>().spawnDelay / enemySpawnTimeMultiplier;
            }
            else
            {
                enemySpawnTimer = 0f;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !gamePaused && gameStarted && !gameEnded)
        {
            if (mainCamera.orthographicSize - cameraScrollSpeed > minCameraSize)
            {
                mainCamera.orthographicSize -= cameraScrollSpeed;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && !gamePaused && gameStarted && !gameEnded)
        {
            if (mainCamera.orthographicSize + cameraScrollSpeed < maxCameraSize)
            {
                mainCamera.orthographicSize += cameraScrollSpeed;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded && gameStarted)
        {
            if (!gamePaused)
            {
                foreach (PlayerBoat b in friendlyBoats)
                {
                    b.boatUI.GetComponent<HealthHover>().boatCanvas.SetActive(false);
                }

                foreach (GameObject g in pauseSubMenus)
                {
                    g.SetActive(false);
                }
                pauseSubMenus[0].SetActive(true);

                gamePaused = true;
                gameScreen.gameObject.SetActive(false);
                pauseScreen.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                gamePaused = false;
                gameScreen.gameObject.SetActive(true);
                pauseScreen.gameObject.SetActive(false);
                Time.timeScale = gameSpeed;
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
        if (!gameEnded && gameStarted)
        {
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && gameStarted)
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
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && gameStarted)
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
        else if (gameEnded && friendlyVictory && !manualQuit)
        {
            mainCameraRig.GetComponent<Rigidbody2D>().AddForce((Vector3.right * cameraMoveSpeed * 16) / gameSpeed);
        }
        else if (gameEnded && !friendlyVictory && !manualQuit)
        {
            mainCameraRig.GetComponent<Rigidbody2D>().AddForce((Vector3.left * cameraMoveSpeed * 16) / gameSpeed);
        }
    }

    public void PauseGame()
    {
        foreach (PlayerBoat b in friendlyBoats)
        {
            b.boatUI.GetComponent<HealthHover>().boatCanvas.SetActive(false);
        }

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

        if (settingsSaver != null)
        {
            settingsSaver.savedGameSpeed = gameSpeed;
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

        if (settingsSaver != null)
        {
            settingsSaver.savedGameSpeed = gameSpeed;
        }
    }

    public void LoadMenu()
    {
        gamePaused = false;
        gameScreen.gameObject.SetActive(true);
        pauseScreen.gameObject.SetActive(false);

        gameEnded = true;
        manualQuit = true;
        Time.timeScale = 1f;
        StartCoroutine(menuLoader.LoadLevelWithAnimation("MainMenu"));
    }

    public void UpdateOtherCurrentEnemy(PlayerBoat boat)
    {
        if (boat.GetBoatFriendlyStatus())
        {
            friendlyBoats.Remove(boat);
            friendlyBoatCount--;
            friendlyBoatCountText.text = friendlyBoats.Count.ToString() + "/" + friendlyMaxBoats;
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
            enemyBoatCount--;
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
        friendlyBoatCount++;
        if (friendlySpawnQueue.Count == 0)
        {
            friendlySpawnTimer = boat.GetComponent<PlayerBoat>().spawnDelay / friendlySpawnTimeMultiplier;
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
        enemyBoatCount++;
        if (enemySpawnQueue.Count == 0)
        {
            enemySpawnTimer = boat.GetComponent<PlayerBoat>().spawnDelay / enemySpawnTimeMultiplier;
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
        spawnedBoat.GetComponent<PlayerBoat>().maxHealth = spawnedBoat.GetComponent<PlayerBoat>().maxHealth * friendlyBoatHealthMultiplier;
        spawnedBoat.GetComponent<PlayerBoat>().currentHealth = spawnedBoat.GetComponent<PlayerBoat>().maxHealth;

        foreach (GameObject s in spawnedBoat.GetComponent<PlayerBoat>().boatPieces)
        {
            if (s.GetComponent<ShipPartDamage>().nonRemovablePiece && s.GetComponent<ShipPartDamage>().pieceName.ToLower() != "cannon")
            {
                s.GetComponent<ShipPartDamage>().pieceCurrentHealth = s.GetComponent<ShipPartDamage>().playerBoat.maxHealth;
            }
            else
            {
                s.GetComponent<ShipPartDamage>().pieceCurrentHealth = s.GetComponent<ShipPartDamage>().pieceMaxHealth;
            }
        }

        friendlyBoats.Add(spawnedBoat.GetComponent<PlayerBoat>());
        friendlyBoatCountText.text = friendlyBoats.Count.ToString() + "/" + friendlyMaxBoats;
    }

    public void InstantiateEnemyBoat(GameObject boat)
    {
        GameObject spawnedBoat = Instantiate(boat, enemyBoatSpawn.position, Quaternion.Euler(0, 179.9999f, 0));
        spawnedBoat.GetComponent<PlayerBoat>().maxHealth = spawnedBoat.GetComponent<PlayerBoat>().maxHealth * enemyBoatHealthMultiplier;
        spawnedBoat.GetComponent<PlayerBoat>().currentHealth = spawnedBoat.GetComponent<PlayerBoat>().maxHealth;

        foreach (GameObject s in spawnedBoat.GetComponent<PlayerBoat>().boatPieces)
        {
            if (s.GetComponent<ShipPartDamage>().nonRemovablePiece && s.GetComponent<ShipPartDamage>().pieceName.ToLower() != "cannon")
            {
                s.GetComponent<ShipPartDamage>().pieceCurrentHealth = s.GetComponent<ShipPartDamage>().playerBoat.maxHealth;
            }
            else
            {
                s.GetComponent<ShipPartDamage>().pieceCurrentHealth = s.GetComponent<ShipPartDamage>().pieceMaxHealth;
            }
        }

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
        if (f > 0f)
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
                    friendlyMoneyPerSecond += (capturePoint.GetMoneyPerSecond() * friendlyMPSMultiplier);
                    if (capturePoint.IsEnemyCaptured())
                    {
                        enemyMoneyPerSecond -= (capturePoint.GetMoneyPerSecond() * enemyMPSMultiplier);
                    }
                    capturePoint.SetFriendlyCaptured();
                }
                else
                {
                    friendlyMoneyPerSecond -= (capturePoint.GetMoneyPerSecond() * friendlyMPSMultiplier);
                    if (!capturePoint.IsEnemyCaptured())
                    {
                        enemyMoneyPerSecond += (capturePoint.GetMoneyPerSecond() * enemyMPSMultiplier);
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
                    enemyMoneyPerSecond += (capturePoint.GetMoneyPerSecond() * enemyMPSMultiplier);
                    if (capturePoint.IsFriendlyCaptured())
                    {
                        friendlyMoneyPerSecond -= (capturePoint.GetMoneyPerSecond() * friendlyMPSMultiplier);
                    }
                    capturePoint.SetEnemyCaptured();
                }
                else
                {
                    enemyMoneyPerSecond -= (capturePoint.GetMoneyPerSecond() * enemyMPSMultiplier);
                    if (!capturePoint.IsFriendlyCaptured())
                    {
                        friendlyMoneyPerSecond += (capturePoint.GetMoneyPerSecond() * friendlyMPSMultiplier);
                    }
                    capturePoint.SetFriendlyCaptured();
                }
            }
        }
    }

    public void FriendlyWin()
    {
        if (!gameEnded)
        {
            postScreenCanvas.SetActive(true);
            friendlyVictory = true;
            victoryScreen.SetActive(true);
            StartCoroutine(LoadMenuAfter());
            StartCoroutine(enemyFireworks.BeginFirworks());
        }
    }

    public void EnemyWin()
    {
        if (!gameEnded)
        {
            postScreenCanvas.SetActive(true);
            friendlyVictory = false;
            defeatScreen.SetActive(true);
            StartCoroutine(LoadMenuAfter());
            StartCoroutine(friendlyFireworks.BeginFirworks());
        }
    }

    public IEnumerator LoadMenuAfter()
    {
        gameHUD.SetActive(false);
        pauseButton.SetActive(false);
        gameEnded = true;
        Time.timeScale = 1f;
        gameSpeedText.text = "1.00";
        yield return new WaitForSeconds(screenTime);

        StartCoroutine(menuLoader.LoadLevelWithAnimation("MainMenu"));
    }

    public void StopAllFriendlyBoats()
    {
        foreach (PlayerBoat b in friendlyBoats)
        {
            b.StopBoat();
        }
    }

    public void StartAllFriendlyBoats()
    {
        foreach (PlayerBoat b in friendlyBoats)
        {
            b.StartBoat();
        }
    }

    public void UpdatedSpawnedFriendlyStopped(bool b)
    {
        spawnFriendlyStopped = b;
    }

    public IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        gameStarted = true;
    }

}
