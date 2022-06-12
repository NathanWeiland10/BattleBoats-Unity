using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public PlayerBase friendlyBase;
    public PlayerBase enemyBase;

    public float friendlyMoneyPerSecond = 0;
    public float enemyMoneyPerSecond = 0;

    public float friendlyTotalMoney;
    public float enemyTotalMoney;

    public List<PlayerBoat> friendlyBoats;
    public List<PlayerBoat> enemyBoats;

    public float minCameraSize;
    public float maxCameraSize;
    public float cameraScrollSpeed;

    public float cameraMoveSpeed;
    public GameObject mainCameraRig;
    public Camera mainCamera;

    public Transform friendlyBoatSpawn;
    public Transform enemyBoatSpawn;

    public GameObject gameHUD;

    public TMP_Text friendlyTotalMoneyText;

    public float moneyDelayTimerStart;

    public CapturePoint[] capturePoints;

    float moneyTimer = 1;
    float waitTime;

    void Awake()
    {
        friendlyMoneyPerSecond += friendlyBase.GetBaseMoneyPerSecond();
        enemyMoneyPerSecond += enemyBase.GetBaseMoneyPerSecond();
        gameHUD.SetActive(true);    
    }

    void Update()
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


        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (mainCamera.orthographicSize - cameraScrollSpeed > minCameraSize)
            {
                mainCamera.orthographicSize -= cameraScrollSpeed;
                // mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z + (cameraScrollSpeed * 1.8f));
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (mainCamera.orthographicSize + cameraScrollSpeed < maxCameraSize)
            {
                mainCamera.orthographicSize += cameraScrollSpeed;
                // mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z - (cameraScrollSpeed * 1.8f));
            }
        }

        /*
        if (Input.GetMouseButton(0))
        {
            Vector3 dragOrigin = new Vector3(mainCameraRig.transform.position.x, 0f, 0f);
            Vector3 difference = dragOrigin - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mainCameraRig.GetComponent<Rigidbody2D>().AddForce(Vector3.left * difference.x * 2);
        }
        */

        moneyTimer -= Time.deltaTime;
        if (moneyTimer <= 0)
        {
            waitTime = (moneyDelayTimerStart / friendlyMoneyPerSecond);
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
        friendlyTotalMoneyText.text = friendlyTotalMoney.ToString();
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

    IEnumerator AddDelayedMoney(float f)
    {
        if (f != 0f)
        {
            yield return new WaitForSeconds(waitTime);
            friendlyTotalMoney += 1;
            friendlyTotalMoneyText.text = friendlyTotalMoney.ToString();
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