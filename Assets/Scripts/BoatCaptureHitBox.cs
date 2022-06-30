using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCaptureHitBox : MonoBehaviour
{
    [Tooltip("The boat (PlayerBoat script) that corresponds to this script")]
    public PlayerBoat playerBoat;

    public PlayerBoat GetPlayerBoat()
    {
        return playerBoat;
    }

}
