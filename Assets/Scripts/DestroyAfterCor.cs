using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterCor : MonoBehaviour
{
    public GameObject root;

    public void DestroyAfter(float time)
    {
        Destroy(root, time);
    }

}