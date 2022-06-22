using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatSelector : MonoBehaviour
{

    public GameObject selectedBorder;
    public GameObject boatPrefab;

    public Sprite boatButtonSprite;

    public BoatSelectionManager manager;

    public string selectSound;
    public string deselectSound;

    [Tooltip("The name of the boat corresponding to this boat (NOTE: This does not need to be set in the inspectator. The name is grabbed from boat prefab)")]
    public string boatName;

    [Tooltip("A description of the boat corresponding to this button")]
    public string boatDescription;

    public GameObject tooltipTextBox;

    bool boatSelected;

    void Awake()
    {
        selectedBorder.SetActive(false);
        boatName = boatPrefab.GetComponent<PlayerBoat>().boatName;
    }

    public void setBoatSelected(bool b)
    {
        boatSelected = b;
    }

    public bool getBoatSelected()
    {
        return boatSelected;
    }

    public string getBoatName()
    {
        return boatName;
    }

    public void setSelectedBorder(bool b)
    {
        selectedBorder.SetActive(b);
    }

    public void attemptSelection()
    {
        if (boatSelected)
        {
            boatSelected = false;
            selectedBorder.SetActive(false);
            manager.RemoveSelectedBoat(boatPrefab, boatButtonSprite);
            FindObjectOfType<AudioManager>().Play(deselectSound);
        }
        else
        {
            if (manager.selectedBoats.Count >= 8)
            {
                Debug.Log("Cannot have more than 8 boats");
            }
            else
            {
                boatSelected = true;
                selectedBorder.SetActive(true);
                manager.AddSelectedBoat(boatPrefab, boatButtonSprite);
                FindObjectOfType<AudioManager>().Play(selectSound);
            }
        }
        tooltipTextBox.SetActive(true);
    }

}
