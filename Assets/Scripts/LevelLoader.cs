using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [Tooltip("The animation that plays when going from the game to the main menu")]
    public Animator transition;

    [Tooltip("The time for the transition from the game to the main menu")]
    public float transitionTime = 1f;

    public IEnumerator LoadLevelWithAnimation(string sceneName)
    {
        // Play loading animation:
        transition.SetTrigger("Start");

        // Wait:
        yield return new WaitForSeconds(transitionTime);

        // Load MainMenu:
        SceneManager.LoadScene(sceneName);
    }

}
