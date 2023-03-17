using System.IO;
using UnityEngine;

public static class PlayerConfigurationFileIO
{
    private static string filePath = Application.persistentDataPath + "/playerconfigurations.json";

    public static void SavePlayerConfigurations(PlayerConfiguration[] playerConfigs)
    {
        string jsonString = JsonUtility.ToJson(playerConfigs, true);
        File.WriteAllText(filePath, jsonString);
    }

    public static PlayerConfiguration[] LoadPlayerConfigurations()
    {
        if (!File.Exists(filePath))
        {
            return null;
        }

        string jsonString = File.ReadAllText(filePath);
        return JsonUtility.FromJson<PlayerConfiguration[]>(jsonString);
    }
}
