using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool Init = false;
    public string AppVersion = GameSettings.APP_VERSION;
    public int lifePoints = GameSettings.LIFE_POINTS;
    public int yearSurvived = 0;
    public int alliesLost = 0;

    public List<AllyCharacterSave> allies = new List<AllyCharacterSave>();
}
