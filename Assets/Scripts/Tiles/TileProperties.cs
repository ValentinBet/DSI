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
    public bool isActivated;

    public LayerMask TileLayer;

    public List<TileProperties> GetTileOnDirection(Vector3 direction, int lenght)
    {
        List<TileProperties> listTilesOnDirection = new List<TileProperties>();

        RaycastHit[] hitTiles = new RaycastHit[lenght];

        hitTiles = Physics.RaycastAll(transform.position, transform.TransformDirection(direction), lenght, TileLayer);

        for (int i = 0; i < hitTiles.Length; i++)
        {
            if (hitTiles[i].collider != null && hitTiles[i].collider.gameObject != this.gameObject)
            {
                if (hitTiles[i].collider.gameObject.GetComponent<TileProperties>() != null)
                {
                    listTilesOnDirection.Add(hitTiles[i].collider.gameObject.GetComponent<TileProperties>());
                }
            }
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(direction) * lenght, Color.red);

        return listTilesOnDirection;
    }
}
