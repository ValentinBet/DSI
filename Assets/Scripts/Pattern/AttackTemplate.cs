using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack Pattern", menuName = "KUBZ/Attack Pattern", order = 0)]
public class AttackTemplate : ScriptableObject
{
    public AttackType attackType;
    public Vector2[] tilesTargetOffset;
}
    public enum AttackType { Zone , Projectile}
