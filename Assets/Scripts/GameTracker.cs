using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    private static GameTracker _instance;
    public static GameTracker Instance { get { return _instance; } }
    public GameObject looseVFX;

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
        actualPA = maxPA;
    }

    public void GatherData()
    {
        baseLife = PlayerBase.Instance.GetLife();
        alliesRemaining = CharactersManager.Instance.GetAliveCharacterCount();
        enemiesRemaining = CharactersManager.Instance.GeteEnemyCharacterCount();
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
                Debug.Log("defeat");
                InitiatingEndGame = true;
                UIManager.Instance.InitEndGame(false);
                StartCoroutine(LoseGame());

            }
            else if (enemiesRemaining < 1 && wavesRemaining < 1)
            {
                //Win
                Debug.Log("win");
                InitiatingEndGame = true;
                UIManager.Instance.InitEndGame(true);
                StartCoroutine(WinGame());
            }
        }

    }

    public IEnumerator LoseGame()
    {
        looseVFX.SetActive(true);
        yield return new WaitForSeconds(6);
        looseVFX.SetActive(false);
        GameManager.Instance.LoseActualLevel();
    }

    public IEnumerator WinGame()
    {
        yield return new WaitForSeconds(6);
        GameManager.Instance.WinActualLevel();
    }

    //Default Value
    public bool PlayerAction(int cost = 1)
    {
        StartCoroutine(DisplayPaFx());

        if (IsHavingEnoughtPa(cost))
        {
            actualPA--;
            UIManager.Instance.SetPA(actualPA);
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator DisplayPaFx()
    {
        //UIManager.Instance.PAdisplay[actualPA].transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        //UIManager.Instance.PAdisplay[actualPA].transform.GetChild(0).gameObject.SetActive(false);
    }

    public void RefreshPA()
    {
        actualPA = maxPA;
        UIManager.Instance.SetPA(actualPA);
    }

    public bool IsHavingEnoughtPa(int value = 1)
    {
        if (actualPA - value > -1)
        {
            return true;
        }
        return false;
    }

    public void TrackQuest()
    {
        //Waiting for Framework
    }

}
