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

<<<<<<< HEAD
    public List<GameObject> boatOrder;

    GameObject currentBoat;
=======
    // FIX LATER: Make this var private since it does not need to be accessed in the inspector:
    [Tooltip("The list of boats that the enemy will spawn in this order")]
    public GameObject currentBoat;
>>>>>>> 5cb43b7eb0c99779cc938faa495ab721d27f49ff

    [Tooltip("The list of playstyles that this enemy can choose from")]
    public enum playStyle { RANDOM, AGRESSIVE, DEFENSIVE, ORDERED };
    
    [Tooltip("The list of boats that the enemy will spawn in this order")]
    public ListGameObject> boatOrder;
    
    [Tooltip("The playstyle that this enemy AI will play")]
    public playStyle enemyPlayStyle;

    GameManager gameManager;

    float decisionMaxTime;
    float decisionTimer = 0;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        decisionMaxTime = 10.5f - enemyDifficulty;
        switch(enemyPlayStyle) {
            case playStyle.RANDOM {
                currentBoat = availableEnemyBoats[Random.Range(0, availableEnemyBoats.Count)];
                break;
            }
            case playStyle.ORDERED {
                if (boatOrder.Count != 0) {
                    currentBoat = boatOrder[0];
                    boatOrder.Remove[0];
                } else {
                    currentBoat = availableEnemyBoats[Random.Range(0, availableEnemyBoats.Count)];
                }
                break;
            }
            default: break;
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
            switch(enemyPlayStyle) {
                case playStyle.RANDOM {
                    currentBoat = availableEnemyBoats[Random.Range(0, availableEnemyBoats.Count)];
                    break;
                }
                case playStyle.ORDERED {
                    if (boatOrder.Count != 0) {
                        currentBoat = boatOrder[0];
                        boatOrder.Remove[0];
                    } else {
                        currentBoat = availableEnemyBoats[Random.Range(0, availableEnemyBoats.Count)];
                    }
                    break;
                }
                default: break;
            }
        }
    }

}
