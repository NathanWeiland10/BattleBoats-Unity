using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatButtonManager : MonoBehaviour
{

    public List<GameObject> boatButtons;

    public List<GameObject> selectedBoatPrefabs;
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