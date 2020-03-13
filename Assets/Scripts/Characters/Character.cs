using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public int maxLife;
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

    public GameObject character_sprite;

    public Animator anim;
    public AnimationDatas animAttack;
    public AnimationDatas animDamaged;
    public List<GameObject> lifeObj = new List<GameObject>();
    public TextMeshProUGUI priorityText;
    public GameObject lifeCanvas;

    private void Start()
    {
        Time.timeScale = 0.2f;
        Application.targetFrameRate = 12;
    }

    private void FixedUpdate()
    {
        LookAtCamera();
        priorityText.text = (priority + 1).ToString();
    }
    private void LateUpdate()
    {
        UpdateOrientation();
    }

    /// <summary>
    /// Défini la tile en dessous du personnage comme occupé et l'appartenant
    /// </summary>
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

                if (occupiedTile.spawnable && !occupiedTile.isOccupied)
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

    /// <summary>
    /// Initialise le mouvement du personnage
    /// </summary>
    /// <param name="tileDestination"></param>
    public void InitMovement(TileProperties tileDestination)
    {
        occupiedTile.isOccupied = false;
        occupiedTile.occupant = null;
        AudioManager.Instance.PlayFootsteps();

        transform.position = tileDestination.transform.position + Vector3.up;

        if (tileDestination.isOnFire)
        {
            PlayAnim(animDamaged.Duration, "Damaged", true, animDamaged.AnimRatio);
            TakeDamaged(1, false);
            AudioManager.Instance.PlayProjectileCharacterHit();
        }

        SetOccupiedTile();
    }

    /// <summary>
    /// Initialise l'attaque du personnage
    /// </summary>
    public void InitAttack()
    {
        MeshRenderer tMR = occupiedTile.GetComponent<MeshRenderer>();
        if (tMR != null)
        {
            tMR.material = PatternReader.instance.attackMat;
        }
    }

    /// <summary>
    /// Le personnage prend des dégats
    /// </summary>
    /// <param name="damageAmount">Nombre de dégats</param>
    /// <param name="cancelPattern">Met fin au pattern ?</param>
    /// <returns></returns>
    public bool TakeDamaged(int damageAmount, bool cancelPattern)
    {
        CameraManager.Instance.InitScreenShake(0.3f, 0.2f);
        life = life - damageAmount;
        UpdateLifeDisplay();
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

    /// <summary>
    /// Se fait attaqués
    /// </summary>
    /// <param name="damageAmount">Nombre de dégats</param>
    /// <param name="attacker">Personnage attaquant</param>
    /// <param name="context">Contexte de l'attaque</param>
    public void GotAttacked(int damageAmount, Character attacker, string context)
    {
        CameraManager.Instance.InitScreenShake(0.3f, 0.2f);
        PlayAnim(animDamaged.Duration, "Damaged", true, animDamaged.AnimRatio);
        Debug.Log(this.gameObject + " attacked by " + attacker.gameObject + " CONTEXT : " + context);
        life -= damageAmount;
        UpdateLifeDisplay();
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

            StartCoroutine(KillCharacter(false, animDamaged.Duration));
        }
    }
    /// <summary>
    /// Met à jour l'affichage de la vie du personnage
    /// </summary>
    private void UpdateLifeDisplay()
    {
        for (int i = 0; i < lifeObj.Count; i++)
        {
            if (i > life - 1)
            {
                print(lifeObj[i]);
                lifeObj[i].SetActive(false);
            }
            else
            {
                lifeObj[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Tue le personnage
    /// </summary>
    /// <param name="cancelPattern">Met fin au pattern ?</param>
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

        if (PatternReader.instance.PatternExecuter.currentCharacter == this || cancelPattern)
        {
            Debug.Log("Le current caracter meurt");
            PatternReader.instance.PatternExecuter.CurrentCaracterDead(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Tue le personnage avec un délai
    /// </summary>
    /// <param name="cancelPattern">Met fin au pattern ?</param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator KillCharacter(bool cancelPattern, float duration)
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

        yield return new WaitForSeconds(duration);

        if (PatternReader.instance.PatternExecuter.currentCharacter == this || cancelPattern)
        {
            Debug.Log("Le current caracter meurt");
            PatternReader.instance.PatternExecuter.CurrentCaracterDead(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    public TileProperties GetTileFromTransform(Vector2 tileOffset, int lenght = 1)
    {

        RaycastHit hitTile;
        float tilesSize = 2;
        Vector3 targetPos = transform.position + (transform.right * (tileOffset.x * tilesSize)) + (transform.forward * (tileOffset.y * tilesSize));

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
            numberOfDeadProjectile = 0;
            PatternReader.instance.PatternExecuter.ActionEnd(mouvementPattern, tilesColored, this, index, depth, continuePattern);
            tilesColored.Clear();
        }
    }

    /// <summary>
    /// le personnage regarde la caméra
    /// </summary>
    public void LookAtCamera()
    {
        if (character_sprite != null)
        {
            character_sprite.transform.LookAt(Camera.main.transform);
        }
    }

    /// <summary>
    /// Tourne le sprite du personnage et du canvas en fonction de la rotation du root
    /// </summary>
    private void UpdateOrientation()
    {
        lifeCanvas.transform.rotation = Quaternion.Euler(45, 225, 0);

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

