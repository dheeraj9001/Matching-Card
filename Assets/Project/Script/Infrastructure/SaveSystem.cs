using MemoryMatch.Domain;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath =>
        System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, "save.json");

    public static void Save(GameState state)
    {
        string json = JsonUtility.ToJson(state, true);
        File.WriteAllText(SavePath, json);
    }

    public static GameState Load()
    {
        if (!File.Exists(SavePath))
            return null;

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<GameState>(json);
    }

    public static void Clear()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}