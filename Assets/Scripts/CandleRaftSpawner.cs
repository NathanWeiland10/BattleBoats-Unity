using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRaftSpawner : MonoBehaviour
{

    public Transform leftSpawnPoint;

    public float minMoveSpeed;

    public float maxMoveSpeed;

    public float minSpawnTime;

    public float maxSpawnTime;

    public GameObject candleRaft;

    void Start()
    {
        StartCoroutine(SpawnCandleRaft());
    }

    IEnumerator SpawnCandleRaft()
    {

        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

        GameObject spawnedBoat = Instantiate(candleRaft, leftSpawnPoint.position, leftSpawnPoint.rotation);
        spawnedBoat.GetComponent<CandleRaft>().moveForce = Random.Range(minMoveSpeed, maxMoveSpeed);


        StartCoroutine(SpawnCandleRaft());

    }

}