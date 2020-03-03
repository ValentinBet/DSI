using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    public List<EnemyCharacter> enemyCharacters = new List<EnemyCharacter>();
    public List<AllyCharacter> allyCharacter = new List<AllyCharacter>();

    private void Start()
    {
        SpawnEnemyCharacter();
    }

    private void SpawnEnemyCharacter()
    {

    }
}
