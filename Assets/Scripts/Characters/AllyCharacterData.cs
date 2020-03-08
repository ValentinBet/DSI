using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyCharacterData
{
    public string name;
    public float experience = 0;
    public int level = 0;
    public int levelMax = GameSettings.xpNeededPerLevel.Count;

    public string allyDescription;

    public List<Vector2> xpNeededPerLevel = GameSettings.xpNeededPerLevel;
}
