using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoatSelectionManager : MonoBehaviour
{

    [Tooltip("The list of GameObjects that represent the boats that are selected in the boat selection screen")]
    public List<GameObject> selectedBoats;

    [Tooltip("The list of sprites that represent the boats that have been selected")]
    public List<Sprite> boatButtonSprites;

    [Tooltip("The button used to advance to the game; can be toggled on or off depending on the number of boats the player has currently selected")]
    public GameObject playButton;

    string selectedLevel = "Level1.1";

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

<<<<<<< HEAD
    public string GetSelectedLevel()
    {
        return selectedLevel;
    }

    public void SetSelectedLevel(string level)
    {
        selectedLevel = level;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(selectedLevel);
    }

}
=======
}
>>>>>>> 5cb43b7eb0c99779cc938faa495ab721d27f49ff
