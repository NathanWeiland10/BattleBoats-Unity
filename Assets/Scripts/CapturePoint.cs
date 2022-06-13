using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    [Tooltip("The prfeb of the enemy flag that will be visible once the enemy team captures this capture point")]
    public GameObject friendlyFlag;
    [Tooltip("The prfeb of the friendly flag that will be visible once the friendly team captures this capture point")]
    public GameObject enemyFlag;

    [Tooltip("The slider that shows the current friendly / enemy capture progress of this capture point")]
    public GameObject slider;
    
    [Tooltip("The sprite that displays this capture point as neutral")]
    public Sprite neutralSliderSprite;
    [Tooltip("The list of sprites that displays the enemy progress of this capture point")]
    public Sprite[] enemySliderSprites;
    [Tooltip("The list of sprites that displays the friendly progress of this capture point")]
    public Sprite[] friendlySliderSprites;

    [Tooltip("The name of this capture point")]
    public string capturePointName;
    [Tooltip("The amount of money per second a team gains once they have taken this capture point")]
    public float moneyPerSecond;

    [Tooltip("The total amount of points needed to capture or recapture this capture point (a higher value will take longer to capture)")]
    public float maxCaptureAmount = 5f;

    // FIX LATER:
    // These variables were set from public to private; check to see if this caused any issues:
    // -----
    List<PlayerBoat> friendlyBoats;
    List<PlayerBoat> enemyBoats;
    
    float friendlyCaptureSpeed;
    float enemyCaptureSpeed;
    
    bool friendlyCaptured = false;
    bool enemyCaptured = false;
    
    float timer = 0;
    // -----
    
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
                // FIX LATER: Remove flags / remove captures statuses here:
            }


            if (timer <= -maxCaptureAmount)
            {
                timer = -maxCaptureAmount;

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
    }

    public void EnemyCapture()
    {
        gameManager.UpdateCapture(true, false, this);
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
