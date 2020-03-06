using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllyType
{
    Warrior,
    Archer,
    Mage
}

public class AllyCharacter : Character
{
    public AllyType allyType;

    public AllyCharacterData data;

    private void Start()
    {
        gameObject.tag = "AllyCharacter";
    }
}
