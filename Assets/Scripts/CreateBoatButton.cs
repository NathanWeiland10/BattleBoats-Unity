using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateBoatButton : MonoBehaviour
{
    [Tooltip("The prefab of the boat that this button will correspond to")]
    public GameObject playerBoat;

    [Tooltip("The GameObject that covers this button icon if the player cannot afford this boat")]
    public GameObject cannotAffordLayer;

    [Tooltip("The TMP text that is used to display the cost of this boat")]
    public TMP_Text boatCostText;

    [Tooltip("The sound effect that is played when buying this boat")]
    public string buySoundEffect;

    [Tooltip("The sound effect that is played when trying and failing to buy this boat")]
    public string cannotBuySoundEffect;

    GameManager gameManager;

    float boatCost;

    void Awake()
    {
        if (playerBoat != null) {
            gameManager = FindObjectOfType<GameManager>();
            boatCostText.text = "$" + boatCost.ToString();
        }
    }

    void Update()
    {
        if (gameManager.GetFriendlyTotalMoney() < boatCost)
        {
            cannotAffordLayer.gameObject.SetActive(true);
        }
        else
        {
            cannotAffordLayer.gameObject.SetActive(false);
        }
    }
    public GameObject GetBoat()
    {
        return playerBoat;
    }

    public void AttemptFriendlyPurchase()
    {
        if (gameManager.friendlyTotalMoney >= boatCost && gameManager.friendlySpawnQueue.Count < gameManager.spawnListIcons.Count)
        {
            gameManager.UpdateFriendlyMoney(-boatCost);
            gameManager.SpawnFriendlyBoat(playerBoat);
            FindObjectOfType<AudioManager>().Play(buySoundEffect);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play(cannotBuySoundEffect);
        }
    }

    public void SetBoatCost(float f)
    {
        boatCost = f;
        boatCostText.text = "$" + f;
    }

}
