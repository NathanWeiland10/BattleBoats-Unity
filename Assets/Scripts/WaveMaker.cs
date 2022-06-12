using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMaker : MonoBehaviour
{

    public Transform waveSpawnPoint;
    public GameObject wavePrefab;

    GameObject instantiatedWave;

    void Start()
    {
        instantiatedWave = Instantiate(wavePrefab, waveSpawnPoint.transform.position, waveSpawnPoint.transform.rotation);
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        instantiatedWave.transform.position = new Vector3(instantiatedWave.transform.position.x-0.1f, instantiatedWave.transform.position.y, instantiatedWave.transform.position.z);
    }

}