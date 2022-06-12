using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoatButton : MonoBehaviour
{
    public string boatName;
    public float boatCost;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();   
    }
    public string GetBoatName()
    {
        return boatName;
    }

    public float GetBoastCost()
    {
        return boatCost;
    }

    public void AttemptPurchase(GameObject boat)
    {
        if (gameManager.friendlyTotalMoney >= boatCost)
        {
            gameManager.UpdateFriendlyMoney(-boatCost);
            gameManager.SpawnFriendlyBoat(boat);
        }
        else
        {
            Debug.Log("Cannot afford");
        }
    }

}