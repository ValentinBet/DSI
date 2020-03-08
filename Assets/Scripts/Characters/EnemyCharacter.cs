using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    public string enemyName;
    public string enemyDescription;

    public int xpEarnWhenKill = 10;

    private void Start()
    {
        gameObject.tag = "EnemyCharacter";
    }
}
