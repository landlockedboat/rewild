using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LoadSaveGame : BitController<LoadSaveGame>
{
    private string _loadGamePath;

    public string LoadGamePath
    {
        get
        {
            if (string.IsNullOrEmpty(_loadGamePath))
            {
                _loadGamePath = Path.Combine(Application.persistentDataPath, "town.json");
            }

            return _loadGamePath;
        }
    }
    
    public void ResetGame()
    {
        print("Reloading game...");
        LevelManager.Instance.LoadLevel(1);
    }
    
    public void ReloadGame()
    {
        print("Reloading game...");
        SaveGameData();
        PlayerPrefs.SetInt(PlayerPrefsKeys.HasActiveGame.ToString(), 1);
        LevelManager.Instance.LoadLevel(1);
    }

    public void EraseGame()
    {
        print("Erasing game...");
        PlayerPrefs.SetInt(PlayerPrefsKeys.HasActiveGame.ToString(), 0);
        LevelManager.Instance.LoadLevel(1);
    }

    public static void SaveGameData()
    {
        var townData = TownModel.Instance.ToData();
        var jsonString = JsonConvert.SerializeObject(townData);
        File.WriteAllText(Instance.LoadGamePath, jsonString);
        print($"Game data saved at {Instance.LoadGamePath}");
    }

    public static TownData LoadGameData()
    {
        print($"Loading game data from {Instance.LoadGamePath}");
        if (!File.Exists(Instance.LoadGamePath))
        {
            throw new FileNotFoundException("Cannot load game data!");
        }

        var dataAsJson = File.ReadAllText(Instance.LoadGamePath);
        return JsonConvert.DeserializeObject<TownData>(dataAsJson);
    }
}