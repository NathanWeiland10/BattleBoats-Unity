using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DockUpgradeButton : MonoBehaviour
{

    [Tooltip("Determines whether or not this upgrade has already been purchased or not")]
    public bool purchased;

    [Tooltip("The GameObject that covers this button icon if the player cannot afford this upgrade")]
    public GameObject cannotAffordLayer;

    [Tooltip("The GameObject that covers this button icon if the player has already bought this upgrade")]
    public GameObject purchasedLayer;

    [Tooltip("The TMP text that is used to display the cost of this upgrade")]
    public TMP_Text upgradeCostText;

    [Tooltip("The description of this upgrade")]
    public string upgradeDescription;

    [Tooltip("The cost of this upgrade")]
    public float upgradeCost;

    [Tooltip("The sound effect that is played when buying this upgrade")]
    public string buySoundEffect;

    [Tooltip("The sound effect that is played when trying and failing to buy this upgrade")]
    public string cannotBuySoundEffect;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        upgradeCostText.text = "$" + upgradeCost.ToString();
        purchasedLayer.gameObject.SetActive(false);
        purchased = false;
    }

    void Update()
    {
        if (!purchased)
        {
            if (gameManager.GetFriendlyTotalMoney() < upgradeCost)
            {
                cannotAffordLayer.gameObject.SetActive(true);
            }
            else
            {
                cannotAffordLayer.gameObject.SetActive(false);
            }
        }
    }

    public void UpgradeBoatHealth(float f)
    {
        if (gameManager.friendlyTotalMoney >= upgradeCost && !purchased)
        {
            gameManager.UpdateFriendlyMoney(-upgradeCost);
            gameManager.friendlyBoatHealthMultiplier += f;
            purchased = true;
            purchasedLayer.gameObject.SetActive(true);
            upgradeCostText.text = "Bought";
            FindObjectOfType<AudioManager>().Play(buySoundEffect);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play(cannotBuySoundEffect);
        }
    }

    public void UpgradeBoatSpawnTime(float f)
    {
        if (gameManager.friendlyTotalMoney >= upgradeCost && !purchased)
        {
            gameManager.UpdateFriendlyMoney(-upgradeCost);
            gameManager.friendlySpawnTimeMultiplier += f;
            purchased = true;
            purchasedLayer.gameObject.SetActive(true);
            upgradeCostText.text = "Bought";
            FindObjectOfType<AudioManager>().Play(buySoundEffect);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play(cannotBuySoundEffect);
        }
    }

    public void UpgradeMPS(float f)
    {

        if (gameManager.friendlyTotalMoney >= upgradeCost && !purchased)
        {
            gameManager.UpdateFriendlyMoney(-upgradeCost);
            gameManager.friendlyMPSMultiplier += f;
            gameManager.friendlyMoneyPerSecond *= (1 + f);
            purchased = true;
            purchasedLayer.gameObject.SetActive(true);
            upgradeCostText.text = "Bought";
            FindObjectOfType<AudioManager>().Play(buySoundEffect);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play(cannotBuySoundEffect);
        }
    }

    public void UpgradeLoot(float f)
    {
        if (gameManager.friendlyTotalMoney >= upgradeCost && !purchased)
        {
            gameManager.UpdateFriendlyMoney(-upgradeCost);
            gameManager.friendlyLootMultiplier += f;
            purchased = true;
            purchasedLayer.gameObject.SetActive(true);
            upgradeCostText.text = "Bought";
            FindObjectOfType<AudioManager>().Play(buySoundEffect);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play(cannotBuySoundEffect);
        }
    }

}