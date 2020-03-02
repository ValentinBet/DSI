using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour
{
    [Header("Properties")]
    public Vector3 tilePosition;
    public bool canSeeThrough;
    public bool isWalkable;
    public bool isOccupied;

    public InteractibleObject occupiedObject;
}
