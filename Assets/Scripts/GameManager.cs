using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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

        DontDestroyOnLoad(this);
    }
    public void EndGame(bool isWin = false)
    {
        if (isWin)
        {
            WinActualLevel();
        }
        else
        {
            LoseActualLevel();
        }
    }

    public void WinActualLevel()
    {
        LevelPassAlive();
        EndLevel();
    }

    public void LoseActualLevel()
    {
        GameInfoManager.GameData.lifePoints -= 1;

        if (GameInfoManager.GameData.lifePoints < 1)
        {
            GameInfoManager.Instance.InitLoseGame();
        }
        else
        {
            LevelPassAlive();
            EndLevel();
        }
    }

    public void LevelPassAlive()
    {
        GameInfoManager.GameData.yearSurvived += 1;

        for (int i = 0; i < CharactersManager.Instance.allyCharacter.Count; i++)
        {
            AllyCharacter _ac = CharactersManager.Instance.allyCharacter[i];

            if (CharactersManager.Instance.allyCharacter[i].myState == CharacterState.Dead) // Crée un nouveau héros si mort
            {
                GameInfoManager.GameData.allies[i] = GameInfoManager.Instance.charactersGenerator.GetNewCharacterSave(GameInfoManager.GameData.allies[i].type);
            }
            else
            {
                _ac.yearSurvived++;
                _ac.life = _ac.maxLife;
                GameInfoManager.GameData.allies[i] = GameInfoManager.Instance.ConvertAllyCharacterForSave(_ac);

            }
        }
    }

    public void EndLevel()
    {
        GameInfoManager.SaveGameDataAsJson();

        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("SelectLevels", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
