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

    GameManager gameManager;

    float boatCost;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        boatCost = playerBoat.GetComponent<PlayerBoat>().boatCost;
        boatCostText.text = "$" + boatCost.ToString();
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
        if (gameManager.friendlyTotalMoney >= boatCost)
        {
            gameManager.UpdateFriendlyMoney(-boatCost);
            gameManager.SpawnFriendlyBoat(playerBoat);
        }
        else
        {
            Debug.Log("Broke");
        }
    }

}
