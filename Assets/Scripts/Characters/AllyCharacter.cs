using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllyCharacter : Character
{
    public AllyCharacterData data;
    public GameObject ally_sprite;

    private void Start()
    {
        gameObject.tag = "AllyCharacter";
        SetOccupiedTile();
    }

    private void FixedUpdate()
    {
        LookAtCamera();
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

    public void LookAtCamera()
    {
        if (ally_sprite != null)
        {
            ally_sprite.transform.LookAt(Camera.main.transform);
        }

    }
}
