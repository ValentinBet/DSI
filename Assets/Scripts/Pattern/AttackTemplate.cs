using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack Pattern", menuName = "KUBZ/Attack Pattern", order = 0)]
public class AttackTemplate : ScriptableObject
{
    public AttackType attackType;
    public AttackParameter[] tilesAffected;

}
    public enum AttackType { Zone , Projectile}

[System.Serializable]
public  struct AttackParameter
{
    public Vector2 tilesTargetOffset;
    public GameObject projectilePrefab;
    public float impactValue;
}
