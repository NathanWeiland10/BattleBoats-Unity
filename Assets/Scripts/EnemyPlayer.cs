using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : MonoBehaviour
{

    [Tooltip("The list of boats (prfabs) that are available to the enemy")]
    public List<GameObject> availableEnemyBoats;

    [Tooltip("The difficulty of this AI; a lower value is easy and a higher value is more difficult")]
    [Range(1, 10)]
    public int enemyDifficulty;

    [Tooltip("The list of boats that the enemy will spawn in this order")]
    public List<GameObject> boatOrder;

    [Tooltip("The list of playstyles that this enemy can choose from")]
    public enum playStyle { RANDOM, AGRESSIVE };

    [Tooltip("The playstyle that this enemy AI will play")]
    public playStyle enemyPlayStyle;

    GameManager gameManager;
    
    GameObject currentBoat;

    float decisionMaxTime;
    float decisionTimer = 0;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        decisionMaxTime = 10.5f - enemyDifficulty;
        if (enemyPlayStyle == playStyle.RANDOM)
        {
            currentBoat = availableEnemyBoats[Random.Range(0, availableEnemyBoats.Count)];
        }
    }

    void Start()
    {
        decisionTimer = decisionMaxTime;
    }

    void Update()
    {
        if (decisionTimer > 0f)
        {
            decisionTimer -= Time.deltaTime;
        }
        else
        {
            decisionTimer = decisionMaxTime;
            AttemptEnemyPurchase();
        }
    }

    public void AttemptEnemyPurchase()
    {
        float boatCost = currentBoat.GetComponent<PlayerBoat>().boatCost;
        if (gameManager.enemyTotalMoney >= boatCost && gameManager.enemySpawnQueue.Count < 5)
        {
            gameManager.UpdateEnemyMoney(-boatCost);
            gameManager.SpawnEnemyBoat(currentBoat);
            currentBoat = availableEnemyBoats[Random.Range(0, availableEnemyBoats.Count)];
        }
    }

}
