﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyCharacterSave
{
    public string name;
    public float experience;
    public int level;
    public string allyDescription = "Never participated in a single battle";

    public int life;
    public int damage;
    public float AttackRange;
    public int movementRange;

    public int yearSurvived;
    public int enemyKilled;

    public CharacterType type;
}
