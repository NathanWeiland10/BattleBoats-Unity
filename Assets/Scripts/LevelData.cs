using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{

    public bool[] lvls;

    public LevelData(LevelSaver saver)
    {
        lvls = saver.GetUnlockedLevels();
    }

}