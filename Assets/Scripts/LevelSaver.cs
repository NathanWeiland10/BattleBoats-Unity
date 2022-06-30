using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaver : MonoBehaviour
{

    public bool[] unlockedLevels = new bool[2];

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        unlockedLevels[0] = true;
        unlockedLevels[1] = false;
    }

    public void SaveUnlockedLevels()
    {
        SaveSystem.SaveLevels(this);
    }

    public void LoadUnlockedLevels()
    {
        LevelData data = SaveSystem.LoadLevels();
        unlockedLevels = data.lvls;
    }

    public void UnlockLevel(int ind)
    {
        unlockedLevels[ind] = true;
    }

    public bool[] GetUnlockedLevels()
    {
        return unlockedLevels;
    }

}