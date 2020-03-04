using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private Phase actualPhase;
    public static PhaseManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void NextPhase()
    {
        if (actualPhase != Phase.Ennemy)
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
            case Phase.Control:
                //All Allies and ennemies become "Standby"
                break;
            case Phase.Allied:
                //CharactersManager.Instance.allyCharacter
                //FindAllAllies,Ticks
                break;
            case Phase.Enemy:
                for (int i = 0; i < CharactersManager.Instance.enemyCharacters.Count; i++)
                {
                    //CharactersManager.Instance.enemyCharacters[i].Execute()
                }
                //findAllEnnemies,ticks
                break;
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
