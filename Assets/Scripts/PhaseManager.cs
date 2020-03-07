using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public Phase actualPhase = Phase.Initial;
    public static PhaseManager Instance { get { return _instance; } }
    private static PhaseManager _instance;

    private List<int> unitIndexs = new List<int>();
    private int currentUnit = 0;
    private int actualTurn = 0;
    private int actualWave = 0;

    [SerializeField]
    private Wave[] levelWaves;

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
    }

    private void Start()
    {
        //Initial Phase
        PhaseTrigger();

    }

    // Update is called once per frame
    public void NextPhase()
    {
        if (actualPhase != Phase.WaveUpdate)
        {
            actualPhase++;
            GameTracker.Instance.TrackerStateUpdate();
        }
        else
        {
            UIManager.Instance.NewTurn();
            actualPhase = Phase.Control;
        }
        Debug.Log(actualPhase);
        PhaseTrigger();
    }

    void PhaseTrigger()
    {
        switch (actualPhase)
        {
            case Phase.Initial:
                CharactersManager.Instance.InitAllyPlacing();
                CharactersManager.Instance.SpawnWave(levelWaves[0]);
                actualWave++;
                break;
            case Phase.Control:
                actualTurn++;
                GameTracker.Instance.RefreshPA();
                PlayerControl.Instance.EnableInputs(true);
                for (int i = 0; i < CharactersManager.Instance.allyCharacter.Count; i++)
                {
                    if (CharactersManager.Instance.allyCharacter[i].myState == CharacterState.Dead)
                    {
                        Destroy(CharactersManager.Instance.allyCharacter[i].gameObject);
                        CharactersManager.Instance.allyCharacter.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        CharactersManager.Instance.allyCharacter[i].myState = CharacterState.Standby;
                    }
                }
                for (int i = 0; i < CharactersManager.Instance.enemyCharacters.Count; i++)
                {
                    if (CharactersManager.Instance.enemyCharacters[i].myState == CharacterState.Dead)
                    {
                        Destroy(CharactersManager.Instance.enemyCharacters[i].gameObject);
                    }
                    else
                    {
                        CharactersManager.Instance.enemyCharacters[i].myState = CharacterState.Standby;
                    }
                }
                break;
            case Phase.Allied:
                UIManager.Instance.AllyTurn();
                PlayerControl.Instance.EnableInputs(false);
                unitIndexs.Clear();
                currentUnit = 0;
                for (int i = 0; i < CharactersManager.Instance.allyCharacter.Count; i++)
                {
                    if (CharactersManager.Instance.allyCharacter[i].myState == CharacterState.Standby)
                    {
                        unitIndexs.Add(i);
                    }
                }
                NextAlly();
                break;
            case Phase.Enemy:
                UIManager.Instance.EnemyTurn();
                unitIndexs.Clear();
                currentUnit = 0;
                for (int i = 0; i < CharactersManager.Instance.enemyCharacters.Count; i++)
                {
                    if (CharactersManager.Instance.enemyCharacters[i].myState == CharacterState.Standby)
                    {
                        unitIndexs.Add(i);
                        //PatternReader.instance.ReadPattern(CharactersManager.Instance.enemyCharacters[i].mouvementPattern, CharactersManager.Instance.enemyCharacters[i]);
                    }
                }
                NextEnemy();
                break;
            case Phase.WaveUpdate:
                if (actualWave < levelWaves.Length)
                {
                    if (actualTurn > levelWaves[actualWave].turnOfActivation - 1 || CharactersManager.Instance.enemyCharacters.Count == 0) // Occurence mauvaise, à patcher
                    {
                        CharactersManager.Instance.SpawnWave(levelWaves[actualWave]);
                        //Animation ?
                        actualWave++;
                    }
                    else
                    {
                        Debug.LogWarning("No New Wave : " + actualTurn + " aTurn " + levelWaves[actualWave].turnOfActivation + " ToActiv " + CharactersManager.Instance.enemyCharacters.Count + " enemies");
                    }
                }
                Invoke("NextPhase",1.0f);
                break;

        }
    }

    //Used for Debug Only, can cause out of sequence actions
    void PhaseTrigger(Phase manualPhase)
    {
        actualPhase = manualPhase;
        PhaseTrigger();
    }

    public void NextAlly()
    {
        if (currentUnit != unitIndexs.Count)
        {
            if (CharactersManager.Instance.allyCharacter[unitIndexs[currentUnit]].myState == CharacterState.Standby)
            {
                //            CharactersManager.Instance.allyCharacter[unitIndexs[currentUnit]].Execute();
                PatternReader.instance.PatternExecuter.ExecutePattern(CharactersManager.Instance.allyCharacter[unitIndexs[currentUnit]]);
                currentUnit++;
            }
            else
            {
                currentUnit++;
                NextAlly();
            }
        }
        else
        {
            NextPhase();
        }
    }

    public void NextEnemy()
    {
        if (currentUnit != unitIndexs.Count)
        {
            if (CharactersManager.Instance.enemyCharacters[unitIndexs[currentUnit]].myState == CharacterState.Standby)
            {
                // CharactersManager.Instance.enemyCharacters[unitIndexs[currentUnit]].Execute();
                PatternReader.instance.PatternExecuter.ExecutePattern(CharactersManager.Instance.enemyCharacters[unitIndexs[currentUnit]]);
                currentUnit++;
            }
            else
            {
                currentUnit++;
                NextEnemy();
            }
        }
        else
        {
            NextPhase();
        }
    }

    public void NextUnit()
    {
        if (actualPhase == Phase.Allied)
        {
            NextAlly();
        }
        else if (actualPhase == Phase.Enemy)
        {
            NextEnemy();
        }
        else
        {
            Debug.LogError("Phase not fit for \"NextUnit()\" call, check sequence order.");
        }
    }

    public Wave GetWave(int waveNumber)
    {
        if (levelWaves.Length < waveNumber)
        {
            return levelWaves[waveNumber];
        }
        else
        {
            Debug.LogError("waveNumberIndex too High. returning null...");
            return null;
        }
    }

    public void LoadWaves(Wave[] waves)
    {
        levelWaves = waves;
    }

    public int GetRemainingWaves()
    {
        return levelWaves.Length - actualWave;
    }
}

public enum Phase
{
    Initial,
    Control,
    Allied,
    Enemy,
    WaveUpdate
}
