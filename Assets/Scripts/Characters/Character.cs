using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Properties")]
    public int life;
    public int damage;
    public float maxAttackRange;
    public float minAttackRange;
    public Vector3 position;

    public List<TileProperties> pathFinding = new List<TileProperties>();
    public TileProperties occupiedTile;

    private void Start()
    {
        SetOccupiedTile();
    }

    public void SetOccupiedTile()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down * 10, out hit, Mathf.Infinity, GridManager.Instance.tilesLayer))
        {
            if (hit.collider.gameObject.GetComponent<TileProperties>() != null)
            {
                occupiedTile = hit.collider.gameObject.GetComponent<TileProperties>();

                occupiedTile.isOccupied = true;
            }
        }
    }
}
