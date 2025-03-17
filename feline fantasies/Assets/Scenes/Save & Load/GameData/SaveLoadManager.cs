using System.Collections.Generic;
using System.IO; 
using UnityEngine; 

[System.Serializable]

public class SaveLoadManager : MonoBehaviour
{
    private string saveFilePath;
    private GameData gameData = new GameData();

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/saveData.json";
        Debug.Log("Save Path: " + saveFilePath); // Debug to check if the path is set
        LoadGame();  // Load game on start
    }

    public void SaveGame()
    {
        if (string.IsNullOrEmpty(saveFilePath))
        {
        Debug.LogError("Save path is empty! Cannot save.");
        return;
        }

        // Ensure the save directory exists
        string directory = Path.GetDirectoryName(saveFilePath);
        if (!Directory.Exists(directory))
        {
        Directory.CreateDirectory(directory);
        }

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved at: " + saveFilePath);
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Loaded!");
        }
        else
        {
            Debug.Log("No save file found, creating a new one.");
        }
    } 
}
