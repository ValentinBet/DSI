using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoManager : MonoBehaviour
{
    private static GameInfoManager _instance;
    public static GameInfoManager Instance { get { return _instance; } }

    public List<GameObject> allyList = new List<GameObject>();

    public static GameData GameData;
    private static string gameDataFileName;

   [SerializeField] private CharactersGenerator charactersGenerator;

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
            NewGame();
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

    private void NewGame()
    {
        GameData = new GameData();
        allyList = charactersGenerator.GetBaseAllyList();
        SaveCharacters();
    }

    private void SaveCharacters()
    {
        foreach (GameObject allyData in allyList)
        {
            GameData.allies.Add(ConvertAllyCharacterForSave(allyData.GetComponent<AllyCharacter>()));
        }
    }

    public AllyCharacterSave ConvertAllyCharacterForSave(AllyCharacter allyCharacter)
    {
        AllyCharacterSave characterSave = new AllyCharacterSave();

        characterSave.name = allyCharacter.name;
        characterSave.experience = allyCharacter.data.experience;
        characterSave.level = allyCharacter.data.level;
        characterSave.allyDescription = allyCharacter.data.allyDescription;
        characterSave.life = allyCharacter.life;
        characterSave.damage = allyCharacter.damage;
        characterSave.AttackRange = allyCharacter.AttackRange;
        characterSave.movementRange = allyCharacter.movementRange;

        return characterSave;
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
        NewGame();
        SaveGameDataAsJson();
    }

    public static void SaveGameDataAsJson()
    {
        string json = JsonUtility.ToJson(GameData);
        File.WriteAllText(gameDataFileName, json);
    }

}
