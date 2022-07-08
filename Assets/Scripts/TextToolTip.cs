using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextToolTip : MonoBehaviour
{
    [Tooltip("The TMP_Text used to display information about a specific boat")]
    public TMP_Text TMPtext;

    public void UpdateText(BoatSelector boat)
    {
        PlayerBoat pb = boat.boatPrefab.GetComponent<PlayerBoat>();
        float dps = 0f;
        if (pb.kamikaze)
        {
            // dps = (((pb.minShotDelay + pb.maxShotDelay) / 2) * pb.cannonBall.GetComponent<KamikazeAttack>().GetAttackDamage());
            dps = Mathf.Round((pb.cannonBall.GetComponent<KamikazeAttack>().GetAttackDamage() * (1 / ((pb.minShotDelay + pb.maxShotDelay) / 2))) * 100.0f) * 0.01f;
        }
        else
        {
            dps = Mathf.Round((pb.cannonBall.GetComponent<CannonBall>().GetCannonBallDamage() * (1 / ((pb.minShotDelay + pb.maxShotDelay) / 2))) * 100.0f) * 0.01f;
        }

        TMPtext.text = "<color=#9acd32>Name: </color>" + boat.boatName + "\n<color=#9acd32>Cost: </color> " + pb.boatCost + "\n<color=#9acd32>Max Health: </color> " + pb.maxHealth + "\n<color=#9acd32>DPS: </color> " + dps + "\n<color=#9acd32>Capture Speed: </color> " + pb.captureSpeed + "\n<color=#9acd32>LOS: </color> " + pb.rangeHitBox.radius + "\n<color=#9acd32>Spawn Delay: </color> " + pb.spawnDelay + "\n<color=#9acd32>Description: </color>" + boat.boatDescription;
    }

    public void UpdateName(CreateBoatButton boat)
    {
        TMPtext.text = boat.playerBoat.GetComponent<PlayerBoat>().boatName;
    }

}
