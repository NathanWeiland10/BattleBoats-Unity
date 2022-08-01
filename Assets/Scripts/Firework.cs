using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{

    public string explosionSFX;
    public GameObject explosionFX;

    public float minStartTime = .5f;
    public float maxStartTime = 1f;

    void Awake()
    {
        StartCoroutine(Explode(Random.Range(minStartTime, maxStartTime)));
    }

    public IEnumerator Explode(float t)
    {
        yield return new WaitForSeconds(t);

        FindObjectOfType<AudioManager>().PlayAtPoint(explosionSFX, transform.position);
        Instantiate(explosionFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
