using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveData(GameData data)
    {
        Debug.Log("Preparing to save...");

        string path = Application.persistentDataPath + "/save.json";
        //GameData data = new GameData();
        string json = JsonUtility.ToJson(data);

        FileStream stream = new FileStream(path, FileMode.Create);
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(json);
        Debug.Log("Game saved!");
        writer.Close();
        stream.Close();
    }

    public static GameData LoadData()
    {
        Debug.Log("Preparing to load...");

        string path = Application.persistentDataPath + "/save.json";

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded!");
            reader.Close();
            stream.Close();
            return data;
        } else
        {
            Debug.Log("No save data found in " + path);
            return null;
        }
    }

    public static void DeleteData()
    {
        Debug.Log("Preparing to delete all save data...");

        string path = Application.persistentDataPath + "/save.json";

        if (File.Exists(path))
        {
            FileUtil.DeleteFileOrDirectory(path);
            Debug.Log("Save data deleted!");
        } else
        {
            Debug.Log("No save data found to delete in " + path);
        }
    }
}
