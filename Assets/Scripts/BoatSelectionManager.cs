using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatSelectionManager : MonoBehaviour
{

    public List<GameObject> selectedBoats;

    public List<Sprite> boatButtonSprites;

    public GameObject playButton;

    void Awake()
    {
        playButton.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    public void AddSelectedBoat(GameObject boat, Sprite s)
    {
        selectedBoats.Add(boat);
        boatButtonSprites.Add(s);
        playButton.SetActive(true);
    }

    public void RemoveSelectedBoat(GameObject boat, Sprite s)
    {
        selectedBoats.Remove(boat);
        boatButtonSprites.Remove(s);
        if (selectedBoats.Count == 0)
        {
            playButton.SetActive(false);
        }
    }

}