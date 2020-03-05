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
}
