using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterCor : MonoBehaviour
{
    [Tooltip("The root / absolute parent of the GameObject you wish to destroy")]
    public GameObject root;

    public void DestroyAfter(float time)
    {
        Destroy(root, time);
    }

}
