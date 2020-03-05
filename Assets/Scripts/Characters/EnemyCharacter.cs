using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public string enemyName;
    public string enemyDescription;

    private void Start()
    {
        gameObject.tag = "EnemyCharacter";
    }
}
