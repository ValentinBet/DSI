using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour
{
    [Header("Properties")]
    public Vector2 tileID;
    public bool canSeeThrough;
    public bool isWalkable;
    public bool isOccupied;
    public bool isMovable;
    public bool isAWall;
    public bool isActivated;
    [HideInInspector] public bool isAllySpawnable = false;

    public LayerMask TileLayer;

    public bool CharacterCanSpawn()
    {
        if (isWalkable && !isOccupied)
        {
            return true;
        }
        return false;
    }


    public List<TileProperties> GetTileOnDirection(Vector3 direction, int lenght = 1, bool isIncludingMainTile = false)
    {
        List<TileProperties> listTilesOnDirection = new List<TileProperties>();

        RaycastHit[] hitTiles = new RaycastHit[lenght];

        hitTiles = Physics.RaycastAll(transform.position, transform.TransformDirection(direction), lenght, TileLayer);

        for (int i = 0; i < hitTiles.Length; i++)
        {
            if (hitTiles[i].collider != null)
            {
                if (hitTiles[i].collider.gameObject.GetComponent<TileProperties>() != null)
                {
                    if (hitTiles[i].collider.gameObject == this.gameObject && !isIncludingMainTile)
                        continue;
                    listTilesOnDirection.Add(hitTiles[i].collider.gameObject.GetComponent<TileProperties>());
                }
            }
        }

        Debug.DrawRay(transform.position , transform.TransformDirection(direction) * lenght , Color.red);

        return listTilesOnDirection;
    }
}
