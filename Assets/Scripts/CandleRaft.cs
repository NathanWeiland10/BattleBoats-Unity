using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRaft : MonoBehaviour
{
    public Rigidbody2D rb;

    public float moveForce;

    void FixedUpdate()
    {
        rb.AddForce(Vector3.right * moveForce);
    }

}