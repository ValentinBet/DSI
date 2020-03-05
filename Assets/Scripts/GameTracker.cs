using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    private static GameTracker _instance;
    public static GameTracker Instance { get {return _instance; } }

    private int baseLife;
    private int alliesRemaining;
    private int ennemiesRemaining;
    private int wavesRemaining;
    private int actionPointsRemaining;

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

    }

    public void TrackerStateUpdate()
    {
        GatherData();
        if (alliesRemaining < 1 || baseLife < 1)
        {
            //Defeat
        } else if (ennemiesRemaining < 1 && wavesRemaining < 1)
        {
            //Win
        }
    }

    public void TrackPA()
    {

    }

    public void TrackQuest()
    {
        //Waiting for Framework
    }

}
