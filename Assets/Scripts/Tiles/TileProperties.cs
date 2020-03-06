using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour
{
    [Header("Properties")]
    public string tileName;
    public string tileDescription;
    [Header("REFS")]
    public SpriteRenderer sR;
    public Sprite icon;
    public Sprite secondaryIcon;

    public Vector2 tileID;
    public bool canSeeThrough;
    public bool isWalkable;
    public bool isOccupied;
    public bool isMovable;
    public bool isActivated;
    public bool isOnFire;
    public int damageToDeal;

    public TilesSpecific specificity;
    public int teleportChannel;

    public TilesOrder order;
    public LayerMask TileLayer;

    public ObjectTypeMetaData ObjectTypeMetaData;
    [HideInInspector] public bool isAllySpawnable = false;
    [HideInInspector] public MeshRenderer mR;
    [HideInInspector] public Material baseMat;
    public Character occupant;



    private void Start()
    {
        gameObject.tag = "Tile";
        mR = GetComponent<MeshRenderer>();
        baseMat = mR.sharedMaterial;

        if (sR != null)
        {
            sR.sprite = icon;
        }


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

        //hitTiles = Physics.RaycastAll(transform.position, transform.TransformDirection(direction), lenght, TileLayer);
        hitTiles = Physics.RaycastAll(transform.position, direction, lenght, TileLayer);

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
        Vector3 originPoint = transform.position + new Vector3(0, 1, 0);
        Debug.DrawRay(originPoint, (direction.normalized * lenght), Color.red, 2);

        return listTilesOnDirection;
    }

    public Vector3 GetCurrentForward()
    {
        Debug.DrawLine(transform.position, (transform.position + (transform.forward * 3)) + new Vector3(0, 1, 0), Color.cyan, 2);
        return transform.forward;
    }

    public float GetRotationOffset(Vector3 directionToTest)
    {
        float multiplier = 1f;
        if (directionToTest.x > transform.forward.x)
        {
            multiplier = -1f;
        }
        Debug.DrawLine(transform.position, (transform.position + (transform.forward * 3)) + new Vector3(0, 1, 0), Color.cyan, 2);
        float angle = (90 - (90 * Vector3.Dot(directionToTest, transform.forward))) * multiplier;
        return angle;
    }

    public TileProperties GetTeleportExit()
    {
        print("try teleport");
        for (int i = 0; i < TilesManager.Instance.teleportList.Count; i++)
        {
            if (TilesManager.Instance.teleportList[i].teleportChannel == this.teleportChannel)
            {
                if (TilesManager.Instance.teleportList[i] != this)
                {
                    print(i);
                    return TilesManager.Instance.teleportList[i];
                }
            }
        }

        Debug.LogError("Il n'y a pas de sortie à ce teleporteur", this);
        return null;
    }

    public void ChangeTilesActivationStatut(bool _isActivated)
    {
        isActivated = _isActivated;
        if (_isActivated)
        {
            sR.sprite = icon;
        }
        else
        {
            sR.sprite = secondaryIcon;
        }
    }

    public enum TilesSpecific
    {
        None,
        Ordre,
        Push,
        Fire,
        Door,
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
