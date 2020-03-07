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
        SetOccupiedTile();
    }

    public void AddExperience(int value)
    {
        data.experience += value;

        CheckLevels();
    }

    private void CheckLevels()
    {
        if (data.level != data.levelMax)
        {
            if (data.experience >= data.xpNeededPerLevel[data.level].y)
            {
                data.level++;
                data.experience -= data.xpNeededPerLevel[data.level - 1].y;
                CheckLevels();
            }
        }

    }
}
