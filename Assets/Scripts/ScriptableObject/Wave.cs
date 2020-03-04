using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : ScriptableObject
{
    public int turnOfActivation;
    public bool needsEmptyBoard;
    public W_Enemy[] enemies;

}

public struct W_Enemy
{
    public EnemyType type;
    public Vector2 gridPosition;
}

public enum EnemyType
{
    Sword,
    Archer
}
