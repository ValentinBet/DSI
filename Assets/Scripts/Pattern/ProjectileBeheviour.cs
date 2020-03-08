using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBeheviour : MonoBehaviour
{
    private int _index, _depth;
    private Character _shooter;
    private TileProperties currentTile;


    public void Init(Character shooter, int index , int depth)
    {
        _depth = depth;
        _index = index;
        _shooter = shooter;
        
    }


    public void FixedUpdate()
    {
        if (currentTile != GetCurrentTile(2))
        {

        }
        else
        {
            DestroyProjectile();
        }
    }

    public TileProperties GetCurrentTile(int lenght = 1)
    {
        RaycastHit hitTile;


        Physics.Raycast(transform.position, Vector3.down, out hitTile, lenght, TilesManager.Instance.tileLayer);

        if (hitTile.collider != null)
        {
            if (hitTile.collider.gameObject.GetComponent<TileProperties>() != null)
            {
                return hitTile.collider.gameObject.GetComponent<TileProperties>();
            }
        }

        return null;
    }

    public void DestroyProjectile()
    {
        _shooter.RegisteredDeathProjectile(_index , _depth);

    }
}


