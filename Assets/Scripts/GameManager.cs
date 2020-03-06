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
        }
    }

    public void LevelPassAlive()
    {
        GameInfoManager.GameData.yearSurvived++;

        GameInfoManager.SaveGameDataAsJson();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
