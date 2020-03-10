using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllyCharacter : Character
{
    public string allyName;
    public float experience = 0;
    public int level = 0;
    public int yearSurvived = 0;
    public int enemyKilled = 0;
    public int levelMax = GameSettings.xpNeededPerLevel.Count;

    public string allyDescription;

    public List<Vector2> xpNeededPerLevel = GameSettings.xpNeededPerLevel;


    private void Start()
    {
        gameObject.tag = "AllyCharacter";
        SetOccupiedTile();
    }

    public void AddExperience(int value)
    {
        experience += value;

        CheckLevels();
    }

    private void CheckLevels()
    {
        if (level != levelMax)
        {
            if (experience >= xpNeededPerLevel[level].y)
            {
                level++;
                experience -= xpNeededPerLevel[level - 1].y;
                UIManager.Instance.SetAllyLevelDisplay(priority);
                CheckLevels();
            }
        }
    }
}
