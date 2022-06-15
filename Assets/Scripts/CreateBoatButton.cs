using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoatButton : MonoBehaviour
{
    [Tooltip("The prefab of the boat that this button will correspond to")]
    public GameObject playerBoat;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();   
    }
    public GameObject GetBoat()
    {
        return playerBoat;
    }

    public void AttemptFriendlyPurchase()
    {
        if (gameManager.friendlyTotalMoney >= playerBoat.GetComponent<PlayerBoat>().boatCost)
        {
            gameManager.UpdateFriendlyMoney(-playerBoat.GetComponent<PlayerBoat>().boatCost);
            gameManager.SpawnFriendlyBoat(playerBoat);
        }
        else
        {
            Debug.Log("Cannot afford");
        }
    }

}
