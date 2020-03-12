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
    public List<TileProperties> tilesColored = new List<TileProperties>();
    public TileProperties occupiedTile;

    public ObjectTypeMetaData ObjectTypeMetaData;

    public GameObject character_sprite;
    public Animator anim;
    public AnimationDatas animAttack;

    private void Start()
    {
        Time.timeScale = 0.2f;
        Application.targetFrameRate = 12;
    }

    private void FixedUpdate()
    {
        LookAtCamera();
    }
    private void LateUpdate()
    {
        UpdateOrientation();
    }

    public void SetOccupiedTile()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down * 10, out hit, Mathf.Infinity, GridManager.Instance.tilesLayer))
        {
            if (hit.collider.gameObject.GetComponent<TileProperties>() != null)
            {
                occupiedTile = hit.collider.gameObject.GetComponent<TileProperties>();

                if (!occupiedTile.isOccupied)
                {
                    occupiedTile.occupant = this;
                    occupiedTile.isOccupied = true;
                }
            }
        }
    }
    public bool GetSpawnableTile()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down * 10, out hit, Mathf.Infinity, GridManager.Instance.tilesLayer))
        {
            if (hit.collider.gameObject.GetComponent<TileProperties>() != null)
            {
                occupiedTile = hit.collider.gameObject.GetComponent<TileProperties>();

                if (occupiedTile.spawnable)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        Debug.LogError("Not A Tile");
        return false;
    }


    public void InitMovement(TileProperties tileDestination)
    {
        occupiedTile.isOccupied = false;
        occupiedTile.occupant = null;
        AudioManager.Instance.PlayFootsteps();

        transform.position = tileDestination.transform.position + Vector3.up;

        if (tileDestination.isOnFire)
        {
            TakeDamaged(1, true);
            AudioManager.Instance.PlayProjectileCharacterHit();
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

    public bool TakeDamaged(int damageAmount, bool cancelPattern)
    {
        life = life - damageAmount;
        if (isAlly)
        {
            UIManager.Instance.AllyLifeUpdate(priority, life);
        }
        if (life < 1)
        {
            KillCharacter(cancelPattern);

            return false;
        }
        return true;
    }

    public void GotAttacked(int damageAmount, Character attacker, string context)
    {

        Debug.Log(this.gameObject + " attacked by " + attacker.gameObject + " CONTEXT : " + context);
        life -= damageAmount;
        if (isAlly)
        {
            UIManager.Instance.AllyLifeUpdate(priority, life);
        }
        if (life < 1)
        {
            if (attacker.isAlly && !this.isAlly)
            {
                AllyCharacter _ac = attacker.GetComponent<AllyCharacter>();
                _ac.AddExperience(this.GetComponent<EnemyCharacter>().xpEarnWhenKill);
                _ac.enemyKilled++;
            }

            KillCharacter(false);
        }
    }

    public void KillCharacter(bool cancelPattern)
    {
        Debug.Log("This character died", this);
        myState = CharacterState.Dead;
        occupiedTile.LostOccupant();

        if (isAlly)
        {
            AudioManager.Instance.PlayAllyDie();
        }
        else
        {
            AudioManager.Instance.PlayCharacterDie();
        }

        if (PatternReader.instance.PatternExecuter.currentCharacter == this && cancelPattern)
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
        // List<TileProperties> listTilesOnDirection = new List<TileProperties>();

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
        Debug.DrawRay(targetPos, Vector3.down, Color.yellow, 10);

        return null;
    }

    public void RegisteredDeathProjectile(int index, int depth, List<TileProperties> tilesToColored, bool continuePattern)
    {
        numberOfDeadProjectile++;
        if (tilesToColored != null)
        {
            for (int i = 0; i < tilesToColored.Count; i++)
            {
                tilesColored.Add(tilesToColored[i]);
            }
        }

        if (numberOfDeadProjectile == AttackPattern.tilesAffected.Length)
        {
            // Debug.Log(continuePattern);
            numberOfDeadProjectile = 0;
            PatternReader.instance.PatternExecuter.ActionEnd(mouvementPattern, tilesColored, this, index, depth, continuePattern);
            tilesColored.Clear();
        }
    }


    public void LookAtCamera()
    {
        if (character_sprite != null)
        {
            character_sprite.transform.LookAt(Camera.main.transform);

        }

    }

    private void UpdateOrientation()
    {
        if ((transform.rotation.eulerAngles.y > -135.0f && transform.rotation.eulerAngles.y < 45.0f) || (transform.rotation.eulerAngles.y > 225.0f && transform.rotation.eulerAngles.y < 405.0f))
        {
            character_sprite.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else
        {
            character_sprite.transform.localScale = 0.5f * Vector3.one;
        }
    }

    public void PlayAnim(float duration, string key, bool keyIsStrigger, float durationRatio = 1)
    {
        float animSpeed = 1 / (duration * durationRatio);
        anim.speed = animSpeed;
        if (keyIsStrigger)
        {
            anim.SetTrigger(key);
        }
        else
        {
            anim.Play(key);
        }

    }

    public void EndAnim(string key)
    {
        anim.SetTrigger(key);
    }
}

[System.Serializable]
public struct AnimationDatas
{
    public float Duration;
    public float AnimRatio;
    public float SoundRatio;

}

