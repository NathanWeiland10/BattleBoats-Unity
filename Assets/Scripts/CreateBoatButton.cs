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

    [Tooltip("The GameObject that covers this button icon if the player cannot currently click this button")]
    public GameObject grayedLayer;

    [Tooltip("The TMP text that is used to display the cost of this boat")]
    public TMP_Text boatCostText;

    [Tooltip("The TMP text that is used for the tooltip area")]
    public TMP_Text tooltipText;

    [Tooltip("The sound effect that is played when buying this boat")]
    public string buySoundEffect;

    [Tooltip("The sound effect that is played when trying and failing to buy this boat")]
    public string cannotBuySoundEffect;

    GameManager gameManager;

    float boatCost;

    void Awake()
    {
        if (playerBoat != null)
        {
            gameManager = FindObjectOfType<GameManager>();
            boatCostText.text = "$" + boatCost.ToString();
        }
    }

    void Update()
    {
        if (gameManager.friendlyBoatCount >= gameManager.friendlyMaxBoats)
        {
            grayedLayer.gameObject.SetActive(true);
        }
        else if (gameManager.friendlySpawnQueue.Count >= gameManager.spawnListIcons.Count)
        {
            grayedLayer.gameObject.SetActive(true);
        }
        else if (gameManager.friendlyTotalMoney < boatCost)
        {
            cannotAffordLayer.gameObject.SetActive(true);
        }
        else
        {
            cannotAffordLayer.gameObject.SetActive(false);
            grayedLayer.gameObject.SetActive(false);
        }
    }

    public GameObject GetBoat()
    {
        return playerBoat;
    }

    public void AttemptFriendlyPurchase()
    {
        if (gameManager.friendlyTotalMoney < boatCost)
        {
            tooltipText.text = "Cannot afford";
            FindObjectOfType<AudioManager>().Play(cannotBuySoundEffect);
        }
        else if (gameManager.friendlySpawnQueue.Count >= gameManager.spawnListIcons.Count)
        {
            tooltipText.text = "Spawn queue limit reached";
            FindObjectOfType<AudioManager>().Play(cannotBuySoundEffect);
        }
        else if (gameManager.friendlyBoatCount >= gameManager.friendlyMaxBoats)
        {
            tooltipText.text = "Max number of boats reached";
            FindObjectOfType<AudioManager>().Play(cannotBuySoundEffect);
        }
        else
        {
            gameManager.UpdateFriendlyMoney(-boatCost);
            gameManager.SpawnFriendlyBoat(playerBoat);
            FindObjectOfType<AudioManager>().Play(buySoundEffect);
        }
    }

    public void SetBoatCost(float f)
    {
        boatCost = f;
        boatCostText.text = "$" + f;
    }

}
