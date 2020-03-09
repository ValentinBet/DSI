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
    public int life;

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
                case TilesSpecific.Push:
                    TilesManager.Instance.pusherList.Add(this);
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
        

        Vector3 transf = transform.position + new Vector3(0, 0.6f, 0);
        Vector3 transf2 = transform.position + new Vector3(0, 1f, 0);


        float dot = Vector3.Dot(directionToTest, transform.forward);
        Debug.DrawLine(transf2, transf2 + (directionToTest * 3), Color.green, 2);
        Debug.DrawLine(transf, transf + (transform.forward * 3), Color.cyan, 2);
        if (dot * dot >= 0.1f)
        {
            if (dot >= 0f)
            {
                return 0;
            }
            return 180;

        }
        else
        {
            Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
            directionToTest = rotation * directionToTest;
            float dotPerpendiculaire = Vector3.Dot(directionToTest, transform.forward);
            return dotPerpendiculaire * 90;
       
        }
    }

    public TileProperties GetTeleportExit()
    {
        for (int i = 0; i < TilesManager.Instance.teleportList.Count; i++)
        {
            if (TilesManager.Instance.teleportList[i].teleportChannel == this.teleportChannel)
            {
                if (TilesManager.Instance.teleportList[i] != this)
                {
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

    public void GetDamaged(int amount)
    {
        if (life <= 0)
        {
            specificity = TilesSpecific.None;
            sR.sprite = secondaryIcon;
        }
    }

    public void LostOccupant()
    {
        occupant = null;
        isOccupied = false;
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
    }

    public enum TilesOrder
    {
        rotate,
        attack,
        stop
    }
}
