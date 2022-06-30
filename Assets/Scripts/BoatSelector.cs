using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatSelector : MonoBehaviour
{

    [Tooltip("The GameObject that is used to represent whether this boat has been selected or not (Ex: A yellow glow / outline)")]
    public GameObject selectedBorder;
    [Tooltip("The prefab of the boat that this button corresponds to")]
    public GameObject boatPrefab;

    [Tooltip("The sprite that is used to display what boat this button corresponds to")]
    public Sprite boatButtonSprite;

    [Tooltip("The manager that handles transferring the boats selected from the selection screen into the game")]
    public BoatSelectionManager manager;

    [Tooltip("The name of the sound effect that will play if this button gets selected")]
    public string selectSound;
    [Tooltip("The name of the sound effect that will play if this button gets deselected")]
    public string deselectSound;

    [Tooltip("The name of the sound effect that will play if the player tries to choose more than eight boats")]
    public string errorSound;

    [Tooltip("The name of the boat corresponding to this boat (NOTE: This does not need to be set in the inspectator. The name is grabbed from boat prefab)")]
    public string boatName;

    [Tooltip("A description of the boat corresponding to this button")]
    public string boatDescription;

    [Tooltip("The GameObject of the tooltip that displays information about the boat corresponding to this button")]
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
                FindObjectOfType<AudioManager>().Play(errorSound);
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
