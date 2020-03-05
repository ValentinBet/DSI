using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoManager : MonoBehaviour
{
    private static GameInfoManager _instance;
    public static GameInfoManager Instance { get { return _instance; } }

    public static GameData GameData;
    private static string gameDataFileName;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        gameDataFileName = Application.persistentDataPath + "GameData" + ".json";
        VerifyGameData();

        DontDestroyOnLoad(this);
    }



    private void VerifyGameData()
    {
        if (!File.Exists(gameDataFileName))
        {
            Debug.Log("Creating new game data");
            GameData = new GameData();

            SaveGameDataAsJson();
        }
        else
        {
            using (StreamReader r = new StreamReader(gameDataFileName))
            {
                var dataAsJson = r.ReadToEnd();
                GameData = JsonUtility.FromJson<GameData>(dataAsJson);
            }

            if (GameData.AppVersion != GameSettings.APP_VERSION)
            {
                Debug.Log("Updating config version installed");
                UpdateJsonGameDataFile();
            }
        }
    }

    private void UpdateJsonGameDataFile()
    {
        GameData _gameData = new GameData();

        _gameData.AppVersion = GameSettings.APP_VERSION;
        _gameData.lifePoints = GameData.lifePoints;
        _gameData.yearSurvived = GameData.yearSurvived;
        _gameData.Init = GameData.Init;

        //ADD HERE FOR UPDATES

        GameData = _gameData;

        SaveGameDataAsJson();
    }

    public void RestartGame()
    {
        File.Delete(gameDataFileName);
        VerifyGameData();
        SaveGameDataAsJson();
    }

    public static void SaveGameDataAsJson()
    {
        string json = JsonUtility.ToJson(GameData);
        File.WriteAllText(gameDataFileName, json);
    }

}
