using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[CreateAssetMenu(fileName = nameof(GameSaveManager), menuName = "ScriptableObjects/" + nameof(GameSaveManager))]
public class GameSaveManager : ScriptableObject
{
    public GameManager gameManager;
    public GridController gridController;

    public bool SavedFileCreate()
    {
        return Directory.Exists(Application.persistentDataPath + "/save");
    }

    public void SaveGame()
    {
        if(SavedFileCreate() == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/save");
        }

        if(!Directory.Exists(Application.persistentDataPath + "/save/game_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/save/game_data");
        }

        if (!Directory.Exists(Application.persistentDataPath + "/save/grid_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/save/grid_data");
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream gameFile = File.Create(Application.persistentDataPath + "/save/game_data/game_save.txt");
        FileStream gridFile = File.Create(Application.persistentDataPath + "/save/grid_data/grid_save.txt");

        var gameJson = JsonUtility.ToJson(gameManager);
        bf.Serialize(gameFile, gameManager);
        gameFile.Close();

        var gridJson = JsonUtility.ToJson(gridController);
        bf.Serialize(gridFile, gridController);
        gridFile.Close();
    }

    public void LoadGame()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/save/game_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/save/game_data");
        }

        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/save/game_data/game_save.txt"))
        {
            FileStream gameFile = File.Open(Application.persistentDataPath + "/save/game_data/game_save.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(gameFile), gameManager);
            gameFile.Close();

            FileStream gridFile = File.Open(Application.persistentDataPath + "/save/grid_data/grid_save.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(gridFile), gridController);
            gridFile.Close();
        }
    }
}
