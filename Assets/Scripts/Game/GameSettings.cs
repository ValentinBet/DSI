using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{
    public const string APP_VERSION = "0.08";
    public const int LIFE_POINTS = 3;
    public const int FIRST_YEAR = 950;

    public static List<Vector2> xpNeededPerLevel = new List<Vector2>()
    {
        new Vector2(1,10),
        new Vector2(2,20),
        new Vector2(3,40)
    };
}
