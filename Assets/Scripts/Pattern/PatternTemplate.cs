using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Movement Pattern", menuName = "KUBZ/Movement Pattern", order = 0)]
public class PatternTemplate : ScriptableObject
{
    public Cardinal initialDirection;  
    public PatternAction[] actions;
}

public enum ActionType { Movement, Rotation, Attack }

public enum Rotation { Left, Rigth, Reverse }

//public enum AttackType { Projectile , Zone , Normal}

[System.Serializable]
public struct PatternAction
{
    public float actionDuration;
    public float previewDuration;
    public ActionType actionType;
    public Rotation rotation;
    // public AttackType AttackType;
}


