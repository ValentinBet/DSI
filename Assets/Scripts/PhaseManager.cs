using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private Phase actualPhase;
    public static PhaseManager Instance { get { return _instance; } }
    private static PhaseManager _instance;

    private List<int> unitIndexs = new List<int>();
    private int currentUnit = 0;

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
    void NextPhase()
    {
        if (actualPhase != Phase.Enemy)
        {
            actualPhase++;
        }
        else
        {
            actualPhase = Phase.Control;
        }
        PhaseTrigger();
    }

    void PhaseTrigger()
    {
        switch(actualPhase)
        {
            case Phase.Initial:
                //Inputs
                break;
            case Phase.Control:
                //PlayerControl.Instance.EnableInputs(true);
                for (int i = 0; i < CharactersManager.Instance.allyCharacter.Count; i++)
                {
                    if (CharactersManager.Instance.allyCharacter[i].myState == CharacterState.Dead)
                    {
                        Destroy(CharactersManager.Instance.allyCharacter[i].gameObject);
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
                //PlayerControl.Instance.EnableInputs(false);
                unitIndexs.Clear();
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
                unitIndexs.Clear();
                for (int i = 0; i < CharactersManager.Instance.enemyCharacters.Count; i++)
                {
                    if(CharactersManager.Instance.enemyCharacters[i].myState == CharacterState.Standby)
                    {
                        unitIndexs.Add(i);
                    }
                }
                NextEnemy();
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
                //            CharactersManager.Instance.enemyCharacters[unitIndexs[currentUnit]].Execute();
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
}

public enum Phase
{
    Initial,
    Control,
    Allied,
    Enemy
}
