using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    private static GameTracker _instance;
    public static GameTracker Instance { get { return _instance; } }

    private int baseLife;
    private int alliesRemaining;
    private int enemiesRemaining;
    private int wavesRemaining;

    [SerializeField]
    private int maxPA = 5;
    private int actualPA;
    private bool InitiatingEndGame = false;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void GatherData()
    {
        baseLife = PlayerBase.Instance.GetLife();
        alliesRemaining = CharactersManager.Instance.allyCharacter.Count;
        enemiesRemaining = CharactersManager.Instance.enemyCharacters.Count;
        wavesRemaining = PhaseManager.Instance.GetRemainingWaves();
    }

    public void TrackerStateUpdate()
    {
        GatherData();

        if (!InitiatingEndGame)
        {
            if ((alliesRemaining < 1 || baseLife < 1))
            {
                //Defeat
                InitiatingEndGame = true;
                GameManager.Instance.LoseActualLevel();

            }
            else if (enemiesRemaining < 1 && wavesRemaining < 1)
            {
                //Win
                InitiatingEndGame = true;
                GameManager.Instance.LoseActualLevel();
            }
        }

    }

    //Default Value
    public bool PlayerAction()
    {
        return PlayerAction(1);
    }
    public bool PlayerAction(int cost)
    {
        if (actualPA - cost > -1)
        {
            actualPA--;
            UIManager.Instance.setPA(actualPA);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RefreshPA()
    {
        actualPA = maxPA;
        UIManager.Instance.setPA(actualPA);
    }

    public void TrackQuest()
    {
        //Waiting for Framework
    }

}
