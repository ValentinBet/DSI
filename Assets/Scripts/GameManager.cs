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
        print("here");
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

        GameInfoManager.SaveGameDataAsJson();
    }

    public void EndLevel()
    {
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("SelectLevels", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
