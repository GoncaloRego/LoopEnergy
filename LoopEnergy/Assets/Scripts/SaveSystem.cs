using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


[CreateAssetMenu(fileName = nameof(SaveSystem), menuName = "ScriptableObjects/" + nameof(SaveSystem))]
public class SaveSystem : ScriptableObject
{
    public SaveData saveData;
    private static string savePath => $"{Application.persistentDataPath}/save_game";
    private BinaryFormatter binaryFormatter = new BinaryFormatter();

    void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        var json = JsonUtility.ToJson(savePath);

        using (var stream = new FileStream(savePath, FileMode.Create))
        {
            binaryFormatter.Serialize(stream, EncryptDecrypt(json));
        }
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            saveData = new SaveData()
            {
                currentLevel = 0
            };

            SaveGame();
        }
        using (var stream = new FileStream(savePath, FileMode.Open))
        {
            var data = (string)binaryFormatter.Deserialize(stream);
            saveData = JsonUtility.FromJson<SaveData>(EncryptDecrypt(data));
        }
    }

    private static string EncryptDecrypt(string textToEncrypt)
    {
        var inSb = new StringBuilder(textToEncrypt);
        var outSb = new StringBuilder(textToEncrypt.Length);

        for (var i = 0; i < textToEncrypt.Length; i++)
        {
            var c = inSb[i];
            c = (char)(c ^ 129);
            outSb.Append(c);
        }

        return outSb.ToString();
    }
}


[System.Serializable]
public class SaveData
{
    public int currentLevel;
    //public List<Node> nodes;
}
