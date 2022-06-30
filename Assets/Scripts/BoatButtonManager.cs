using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatButtonManager : MonoBehaviour
{

    [Tooltip("The list of GameObject buttons used to spawn friendly boats in game")]
    public List<GameObject> boatButtons;

    [Tooltip("The list of GameObjects that were selected by the player in the boat selection screen")]
    public List<GameObject> selectedBoatPrefabs;
    [Tooltip("The list of sprites that represent the boats that were selected in the boat selection screen")]
    public List<Sprite> selectedBoatButtonSprites;

    BoatSelectionManager manager;

    void Awake()
    {
        manager = FindObjectOfType<BoatSelectionManager>();
        if (manager != null) {
            selectedBoatPrefabs = manager.selectedBoats;
            selectedBoatButtonSprites = manager.boatButtonSprites;

            for (int i = 0; i < selectedBoatPrefabs.Count; i++)
            {
                boatButtons[i].GetComponent<Image>().sprite = selectedBoatButtonSprites[i];
                boatButtons[i].GetComponent<CreateBoatButton>().playerBoat = selectedBoatPrefabs[i];
                boatButtons[i].GetComponent<CreateBoatButton>().SetBoatCost(selectedBoatPrefabs[i].GetComponent<PlayerBoat>().boatCost);
            }
            for (int i = selectedBoatPrefabs.Count; i < boatButtons.Count; i++)
            {
                boatButtons[i].SetActive(false);
            }
        }
        else
        {
            foreach(GameObject boat in boatButtons)
            {
                boat.GetComponent<CreateBoatButton>().SetBoatCost(boat.GetComponent<CreateBoatButton>().playerBoat.GetComponent<PlayerBoat>().boatCost);
            }
        }

    }

}
