using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllyCharacter : Character
{
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
                UIManager.Instance.SetAllyLevelDisplay(priority);
                CheckLevels();
            }
        }

    }
}
