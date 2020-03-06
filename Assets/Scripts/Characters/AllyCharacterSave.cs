using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyCharacterSave
{
    public string name;
    public float experience;
    public int level;
    public string allyDescription;

    public int life;
    public int damage;
    public float AttackRange;
    public int movementRange;

    public AllyType type;
}
