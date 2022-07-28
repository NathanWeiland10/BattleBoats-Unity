using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicFireworks : MonoBehaviour
{

    public Transform fireworkSpawnPoint;

    public GameObject fireworkPrefab;

    public float startDelay = .5f;

    public float launchForce;

    public string shotSoundEffect;

    public float minShotDelay = .5f;
    public float maxShotDelay = 1f;

    Transform cannonAngle;

    public IEnumerator BeginFirworks()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(LaunchFirework(Random.Range(minShotDelay, maxShotDelay)));
    }

    public IEnumerator LaunchFirework(float t)
    {
        FindObjectOfType<AudioManager>().PlayAtPoint(shotSoundEffect, fireworkSpawnPoint.position);

        Vector3 spawnPos = new Vector3(fireworkSpawnPoint.position.x + Random.Range(-0.5f, 0.5f), fireworkSpawnPoint.position.y, fireworkSpawnPoint.position.z);

        GameObject bulletToShoot = Instantiate(fireworkPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D bulletRB = bulletToShoot.GetComponent<Rigidbody2D>();
        bulletRB.AddForce((-fireworkSpawnPoint.up) * launchForce);

        yield return new WaitForSeconds(t);

        StartCoroutine(LaunchFirework(Random.Range(minShotDelay, maxShotDelay)));
    }

}
