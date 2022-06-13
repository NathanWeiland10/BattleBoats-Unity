using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [Tooltip("The amount of seconds until this object will be destroyed")]
    public float fTimer = 4;

    void Update()
    {
        fTimer -= Time.deltaTime;
        if (fTimer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
