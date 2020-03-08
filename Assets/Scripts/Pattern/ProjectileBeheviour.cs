using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBeheviour : MonoBehaviour
{
    private int _index, _depth;
    private Character _shooter;
    private TileProperties currentTile;
    private bool isOnFire;
    private List<TileProperties> tilesColored = new List<TileProperties>();
    private TileProperties lastTeleoprtUsed; 


    public void Init(Character shooter, int index, int depth)
    {
        _depth = depth;
        _index = index;
        _shooter = shooter;

    }


    public void FixedUpdate()
    {

        transform.Translate(Vector3.forward * Time.deltaTime * 6);

        TileProperties testedTile = GetCurrentTile(2);
        if (testedTile == null)
        {
            DestroyProjectile();
        }
        if (currentTile != testedTile)
        {
            CheckTile(testedTile);
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
        _shooter.RegisteredDeathProjectile(_index, _depth , tilesColored);
        Destroy(this.gameObject);

    }

    private void CheckTile(TileProperties testedTile)
    {
        Debug.Log("TileChange");
        if (testedTile == null)
        {
            DestroyProjectile();
            return;
        }

        tilesColored.Add(testedTile);
        TilesManager.Instance.ChangeTileMaterial(testedTile, PatternReader.instance.attackMat);
        currentTile = testedTile;

        if (testedTile.isOccupied)
        {
            if (isOnFire)
            {
                testedTile.occupant.GotAttacked(_shooter.damage + 1, _shooter);
                DestroyProjectile();
            }
            else
            {
                testedTile.occupant.GotAttacked(_shooter.damage, _shooter);
                DestroyProjectile();
            }
        }
        switch (testedTile.specificity)
        {

            case TileProperties.TilesSpecific.Push:

                // 2 = tile Size
                Vector3 pushPos = transform.position + testedTile.transform.forward * 2;
                testedTile.ChangeTilesActivationStatut(false);
                break;
            case TileProperties.TilesSpecific.Fire:
                isOnFire = true;
                break;
            case TileProperties.TilesSpecific.Door:
                if (testedTile.isActivated)
                {
                    testedTile.ChangeTilesActivationStatut(false);
                    DestroyProjectile();
                }
                break;

            case TileProperties.TilesSpecific.Wall:
                testedTile.GetDamaged(1);
                DestroyProjectile();

                break;
            case TileProperties.TilesSpecific.Teleport:
               

                TileProperties teleportExit = testedTile.GetTeleportExit();
                if (lastTeleoprtUsed == teleportExit)
                {
                    return;
                }
                lastTeleoprtUsed = testedTile;
                // Vector added = space between the bullet and the ground;
                transform.position = teleportExit.transform.position + new Vector3(0,0.5f,0);
                transform.rotation = teleportExit.transform.rotation;

                break;
            case TileProperties.TilesSpecific.PlayerBase:
                DestroyProjectile();
                break;
        }
    }
}


