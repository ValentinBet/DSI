using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Warrior,
    Archer,
    Mage
}

public class Character : MonoBehaviour
{
    [Header("Properties")]
    public int life;
    public int damage;
    public float AttackRange;
    public int movementRange;

    public Vector3 position;
    public int priority;

    public CharacterState myState = CharacterState.Standby;
    public CharacterType characterType;
    public bool isAlly;

    public PatternTemplate mouvementPattern;

    public List<TileProperties> pathFinding = new List<TileProperties>();
    public TileProperties occupiedTile;

    public ObjectTypeMetaData ObjectTypeMetaData;

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

                occupiedTile.occupant = this;
                occupiedTile.isOccupied = true;
            }
        }
    }

    public void InitMovement(TileProperties tileDestination)
    {
        occupiedTile.isOccupied = false;
        occupiedTile.occupant = null;

        transform.position = tileDestination.transform.position + Vector3.up;
        if (tileDestination.isOnFire)
        {
            TakeDamaged(1);
        }
        SetOccupiedTile();
    }

    public void InitAttack()
    {
        MeshRenderer tMR = occupiedTile.GetComponent<MeshRenderer>();
        if (tMR != null)
        {
            tMR.material = PatternReader.instance.attackMat;
        }
    }

    public bool TakeDamaged(int damageAmount)
    {
        life -= damageAmount;
        if (life <= 0)
        {
            Debug.Log("This character died", this);
            myState = CharacterState.Dead;
            occupiedTile.LostOccupant();
            if (PatternReader.instance.PatternExecuter.currentCharacter == this)
            {
                PatternReader.instance.PatternExecuter.StopPattern(this);
            }

            return false;
        }
        return true;
    }

    public void GotAttacked(int damageAmount)
    {
        life -= damageAmount;
        if (life <= 0)
        {
            Debug.Log("This character died", this);
            myState = CharacterState.Dead;
            occupiedTile.LostOccupant();
            if (PatternReader.instance.PatternExecuter.currentCharacter == this)
            {
                PatternReader.instance.PatternExecuter.StopPattern(this);
            }
        }

    }

}
public enum CharacterState
{
    Standby,
    Finished,
    Dead
}
