using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterType
{
    Warrior,
    Archer,
    Mage
}

public enum CharacterState
{
    Standby,
    Finished,
    Dead
}

public enum CombatStyle { closeCombat, range }


public class Character : MonoBehaviour
{
    [Header("Properties")]
    public int life;
    public int damage;
    public float AttackRange;
    public int movementRange;

    public Vector3 position;
    public int priority;

    private int numberOfDeadProjectile;

    public CharacterState myState = CharacterState.Standby;
    public CharacterType characterType;
    public CombatStyle combatStyle;
    public bool isAlly;

    public PatternTemplate mouvementPattern;
    public AttackTemplate AttackPattern;

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

        if (life < 1)
        {
            KillCharacter();

            return false;
        }
        return true;
    }

    public void GotAttacked(int damageAmount, Character attacker)
    {

        life -= damageAmount;
        if (life < 1)
        {
            if (attacker.isAlly && !this.isAlly)
            {
                attacker.GetComponent<AllyCharacter>().AddExperience(this.GetComponent<EnemyCharacter>().xpEarnWhenKill);
            }

            KillCharacter();
        }
    }

    public void KillCharacter()
    {
        Debug.Log("This character died", this);
        myState = CharacterState.Dead;
        occupiedTile.LostOccupant();

        if (PatternReader.instance.PatternExecuter.currentCharacter == this)
        {
            PatternReader.instance.PatternExecuter.StopPattern(this);
        }

        //if (isAlly && CharactersManager.Instance.allyCharacter.Contains(GetComponent<AllyCharacter>())) // (LINQ)
        //{
        //    CharactersManager.Instance.allyCharacter.Remove(GetComponent<AllyCharacter>());
        //}

        gameObject.SetActive(false);
    }

    public TileProperties GetTileFromTransform(Vector2 tileOffset, int lenght = 1)
    {
        List<TileProperties> listTilesOnDirection = new List<TileProperties>();

        RaycastHit hitTile;
        float tilesSize = 2;
        Vector3 targetPos = transform.position + (transform.right * (tileOffset.x * tilesSize)) + (transform.forward * (tileOffset.y * tilesSize));


        //hitTiles = Physics.RaycastAll(transform.position, transform.TransformDirection(direction), lenght, TileLayer);
        Physics.Raycast(targetPos, Vector3.down, out hitTile, lenght, TilesManager.Instance.tileLayer);

        if (hitTile.collider != null)
        {
            if (hitTile.collider.gameObject.GetComponent<TileProperties>() != null)
            {
                return hitTile.collider.gameObject.GetComponent<TileProperties>();
            }
        }
;
        Debug.DrawRay(targetPos, Vector3.down, Color.red, 2);

        return null;
    }

    public void RegisteredDeathProjectile(int index, int depth)
    {
        numberOfDeadProjectile++;
        if (numberOfDeadProjectile == AttackPattern.tilesAffected.Length)
        {
            numberOfDeadProjectile = 0;
            //PatternReader.instance.PatternExecuter.ActionEnd(mouvementPattern, this, index, depth);
        }
    }
}

