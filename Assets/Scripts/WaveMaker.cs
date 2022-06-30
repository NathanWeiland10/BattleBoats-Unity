using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMaker : MonoBehaviour
{
    [Tooltip("The transform that waves will be spawned at")]
    public Transform waveSpawnPoint;
    [Tooltip("The prefab for the wave that will be spawned")]
    public GameObject wavePrefab;

    GameObject instantiatedWave;

    void Start()
    {
        instantiatedWave = Instantiate(wavePrefab, waveSpawnPoint.transform.position, waveSpawnPoint.transform.rotation);
    }

    void FixedUpdate()
    {
        instantiatedWave.transform.position = new Vector3(instantiatedWave.transform.position.x-0.1f, instantiatedWave.transform.position.y, instantiatedWave.transform.position.z);
    }

}
