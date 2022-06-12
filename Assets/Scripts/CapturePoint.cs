using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{

    public GameObject friendlyFlag;
    public GameObject enemyFlag;

    public GameObject slider;
    public Sprite neutralSliderSprite;
    public Sprite[] enemySliderSprites;
    public Sprite[] friendlySliderSprites;

    public string capturePointName;
    public float moneyPerSecond;

    public float maxCaptureAmount;

    public List<PlayerBoat> friendlyBoats;
    public List<PlayerBoat> enemyBoats;

    public float friendlyCaptureSpeed;
    public float enemyCaptureSpeed;

    public bool friendlyCaptured = false;
    public bool enemyCaptured = false;

    public float timer = 0;
    bool beginCapture = false;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        slider.GetComponent<SpriteRenderer>().sprite = neutralSliderSprite;
        friendlyFlag.SetActive(false);
        enemyFlag.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        BoatCaptureHitBox boat = collision.gameObject.GetComponent<BoatCaptureHitBox>();

        if (boat != null)
        {

            // if (maxCaptureAmount != Mathf.Abs(timer)) 

                if (boat.GetPlayerBoat().GetBoatFriendlyStatus() && !boat.GetPlayerBoat().IsDead())
                {
                    if (!friendlyBoats.Contains(boat.GetPlayerBoat())) {
                        boat.GetPlayerBoat().SetIsCapturing(true);
                        friendlyBoats.Add(boat.GetPlayerBoat());
                        friendlyCaptureSpeed += boat.GetPlayerBoat().GetCaptureSpeed();
                    }

                }
                else if (!boat.GetPlayerBoat().GetBoatFriendlyStatus() && !boat.GetPlayerBoat().IsDead())
                {
                    if (!enemyBoats.Contains(boat.GetPlayerBoat()))
                    {
                        boat.GetPlayerBoat().SetIsCapturing(true);
                        enemyBoats.Add(boat.GetPlayerBoat());
                        enemyCaptureSpeed += boat.GetPlayerBoat().GetCaptureSpeed();
                    }

                }
           

        }

        if (friendlyCaptureSpeed != enemyCaptureSpeed)
        {
            beginCapture = true;
        }
        else
        {
            beginCapture = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BoatCaptureHitBox boat = collision.gameObject.GetComponent<BoatCaptureHitBox>();

        if (boat != null)
        {
            if (boat.GetPlayerBoat().GetBoatFriendlyStatus())
            {

                if (friendlyBoats.Contains(boat.GetPlayerBoat()))
                {
                    boat.GetPlayerBoat().SetIsCapturing(false);
                    friendlyBoats.Remove(boat.GetPlayerBoat());

                    friendlyCaptureSpeed -= boat.GetPlayerBoat().GetCaptureSpeed();
                }
            }
            else if (!boat.GetPlayerBoat().GetBoatFriendlyStatus())
            {
                if (enemyBoats.Contains(boat.GetPlayerBoat()))
                {
                    boat.GetPlayerBoat().SetIsCapturing(false);
                    enemyBoats.Remove(boat.GetPlayerBoat());

                    enemyCaptureSpeed -= boat.GetPlayerBoat().GetCaptureSpeed();
                }
            }

        }
        if (friendlyCaptureSpeed != enemyCaptureSpeed)
        {
            beginCapture = true;
        }
        else
        {
            beginCapture = false;
        }
    }

    void Update()
    {
        if (beginCapture) {

            timer += (Time.deltaTime * (friendlyCaptureSpeed - enemyCaptureSpeed));


            if (timer >= maxCaptureAmount * 0.25 && timer < maxCaptureAmount * 0.5)
            {
                slider.GetComponent<SpriteRenderer>().sprite = friendlySliderSprites[0];
            } 
            else if (timer >= maxCaptureAmount * 0.5 && timer < maxCaptureAmount * 0.75)
            {
                slider.GetComponent<SpriteRenderer>().sprite = friendlySliderSprites[1];
            }
            else if (timer >= maxCaptureAmount * 0.75 && timer < maxCaptureAmount * 1)
            {
                slider.GetComponent<SpriteRenderer>().sprite = friendlySliderSprites[2];
            }
            else if (timer <= -maxCaptureAmount * 0.25 && timer > -maxCaptureAmount * 0.5)
            {
                slider.GetComponent<SpriteRenderer>().sprite = enemySliderSprites[0];
            }
            else if (timer <= -maxCaptureAmount * 0.5 && timer > -maxCaptureAmount * 0.75)
            {
                slider.GetComponent<SpriteRenderer>().sprite = enemySliderSprites[1];
            }
            else if (timer <= -maxCaptureAmount * 0.75 && timer > -maxCaptureAmount * 1)
            {
                slider.GetComponent<SpriteRenderer>().sprite = enemySliderSprites[2];
            }
            else if (timer < maxCaptureAmount * 0.25 && timer > -maxCaptureAmount * 0.25)
            {
                slider.GetComponent<SpriteRenderer>().sprite = neutralSliderSprite;
            }


            if (timer <= -maxCaptureAmount)
            {
                timer = -maxCaptureAmount;

                // enemyCaptured = true;
                // friendlyCaptured = false;

                // gameManager.UpdateEnemyMoneyPerSecond(moneyPerSecond);
                // gameManager.UpdateFriendlyMoneyPerSecond(-moneyPerSecond);
                EnemyCapture();

                friendlyFlag.SetActive(false);
                enemyFlag.SetActive(true);

                beginCapture = false;

                slider.GetComponent<SpriteRenderer>().sprite = enemySliderSprites[3];

                foreach (PlayerBoat boat in enemyBoats)
                {
                    boat.SetIsCapturing(false);
                }
            }
            else if (timer >= maxCaptureAmount)
            {
                timer = maxCaptureAmount;

                // friendlyCaptured = true;
                // enemyCaptured = false;

                // gameManager.UpdateEnemyMoneyPerSecond(-moneyPerSecond);
                // gameManager.UpdateFriendlyMoneyPerSecond(moneyPerSecond);
                FriendlyCapture();

                friendlyFlag.SetActive(true);
                enemyFlag.SetActive(false);

                beginCapture = false;

                slider.GetComponent<SpriteRenderer>().sprite = friendlySliderSprites[3];

                foreach (PlayerBoat boat in friendlyBoats)
                {
                    boat.SetIsCapturing(false);
                }
            }
        }
    }

    public void FriendlyCapture()
    {
        gameManager.UpdateCapture(true, true, this);
        // gameManager.UpdateEnemyCapture(false, this);
    }

    public void EnemyCapture()
    {
        gameManager.UpdateCapture(true, false, this);
        // gameManager.UpdateEnemyCapture(true, this);
    }

    public bool IsFriendlyCaptured()
    {
        return friendlyCaptured;
    }

    public bool IsEnemyCaptured()
    {
        return enemyCaptured;
    }

    public void SetFriendlyCaptured()
    {
        enemyCaptured = false;
        friendlyCaptured = true;
    }

    public void SetEnemyCaptured()
    {
        enemyCaptured = true;
        friendlyCaptured = false;
    }

    public float GetMoneyPerSecond()
    {
        return moneyPerSecond;
    }

}