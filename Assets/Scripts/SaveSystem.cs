using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveLevels(LevelSaver lvldata)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levels.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(lvldata);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData LoadLevels()
    {

        string path = Application.persistentDataPath + "/levels.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData lvls = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return lvls;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }

    }

}