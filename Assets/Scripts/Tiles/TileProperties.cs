using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour
{
    [Header("Properties")]
    public string tileName;
    public string tileDescription;

    public Vector2 tileID;
    public bool canSeeThrough;
    public bool isWalkable;
    public bool isOccupied;
    public bool isMovable;
    public bool isActivated;
    public bool isOnFire;

    public TilesSpecific specificity;
    public TilesOrder order;
    public LayerMask TileLayer;

    public ObjectTypeMetaData ObjectTypeMetaData;
    [HideInInspector] public bool isAllySpawnable = false;
    [HideInInspector] public MeshRenderer mR;
    [HideInInspector] public Material baseMat;


    private void Start()
    {
        gameObject.tag = "Tile";
        mR = GetComponent<MeshRenderer>();
        baseMat = mR.sharedMaterial;
        if (specificity != TilesSpecific.None)
        {
            switch (specificity)
            {
                case TilesSpecific.Teleport:
                    TilesManager.Instance.teleportList.Add(this);
                    break;
                case TilesSpecific.Trap:
                    TilesManager.Instance.trapList.Add(this);
                    break;
            }
        }
    }
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

        Debug.DrawRay(transform.position, transform.TransformDirection(direction) * lenght, Color.red);

        return listTilesOnDirection;
    }


    public Vector3 GetCurrentForward()
    {
        return transform.forward;
    }

    public float GetRotationOffset(Vector3 directionToTest)
    {

        float multiplier = 1f;
        if (directionToTest.x < 0)
        {
            multiplier = -1f;
        }
        Debug.DrawLine(transform.position, (transform.position + transform.forward) * 3, Color.cyan, 2);
        float angle = (180 * Vector3.Dot(directionToTest, transform.forward)) * multiplier;
        print(angle);
        return angle;
    }


    public enum TilesSpecific
    {
        None,
        Ordre,
        Push,
        Fire,
        Block,
        Wall,
        Teleport,
        Trap,
        PlayerBase,
        EnemyBase
    }

    public enum TilesOrder
    {
        rotate, 
        attack,
        stop
    }
}
