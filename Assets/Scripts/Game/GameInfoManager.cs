using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameInfoManager : MonoBehaviour
{
    private static GameInfoManager _instance;
    public static GameInfoManager Instance { get { return _instance; } }

    public static GameData GameData;
    private static string gameDataFileName;

    [SerializeField] public CharactersGenerator charactersGenerator;

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
        InitNewCharacters();
        SaveGameDataAsJson();
    }

    private void InitNewCharacters()
    {
        foreach (GameObject baseAlly in charactersGenerator.GetBaseAllyList())
        {
            SaveCharacter(baseAlly);
        }

        for (int i = 0; i < GameData.allies.Count; i++)
        {
            GameData.allies[i] = charactersGenerator.SetBasicStats(GameData.allies[i], (CharacterType)i); // Génère un Guerrier / archer / mage a la suite --> Scope + -> pourra choisir sa composition
        }

    }

    private void SaveCharacter(GameObject ally)
    {
        GameData.allies.Add(ConvertAllyCharacterForSave(ally.GetComponent<AllyCharacter>()));
    }

    public AllyCharacterSave ConvertAllyCharacterForSave(AllyCharacter allyCharacter)
    {
        AllyCharacterSave characterSave = new AllyCharacterSave();

        characterSave.name = allyCharacter.allyName;
        characterSave.experience = allyCharacter.experience;
        characterSave.level = allyCharacter.level;
        characterSave.allyDescription = allyCharacter.allyDescription;
        characterSave.life = allyCharacter.life;
        characterSave.damage = allyCharacter.damage;
        characterSave.AttackRange = allyCharacter.AttackRange;
        characterSave.movementRange = allyCharacter.movementRange;
        characterSave.type = allyCharacter.characterType;
        characterSave.yearSurvived = allyCharacter.yearSurvived;
        characterSave.enemyKilled = allyCharacter.enemyKilled;

        return characterSave;
    }

    public void UpdateCharacterObjToHisSave(ref AllyCharacter allyCharacter, AllyCharacterSave save)
    {
        allyCharacter.characterType = save.type;
        allyCharacter.allyName = save.name;
        allyCharacter.experience = save.experience;
        allyCharacter.level = save.level;
        allyCharacter.allyDescription = save.allyDescription;
        allyCharacter.life = save.life;
        allyCharacter.damage = save.damage;
        allyCharacter.AttackRange = save.AttackRange;
        allyCharacter.movementRange = save.movementRange;
        allyCharacter.yearSurvived = save.yearSurvived;
        allyCharacter.enemyKilled = save.enemyKilled;
    }

    private void UpdateJsonGameDataFile()
    {
        GameData _gameData = new GameData();

        _gameData.AppVersion = GameSettings.APP_VERSION;
        _gameData.lifePoints = GameData.lifePoints;
        _gameData.yearSurvived = GameData.yearSurvived;
        _gameData.Init = GameData.Init;
        _gameData.alliesLost = GameData.alliesLost;
        _gameData.allies = GameData.allies;

        //ADD HERE FOR UPDATES

        GameData = _gameData;

        SaveGameDataAsJson();
    }

    public void InitLoseGame()
    {
        RestartGame();
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
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
