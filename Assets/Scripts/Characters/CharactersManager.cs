using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    private static CharactersManager _instance;
    public static CharactersManager Instance { get { return _instance; } }

    public List<EnemyCharacter> enemyCharacters = new List<EnemyCharacter>();
    public List<AllyCharacter> allyCharacter = new List<AllyCharacter>();
    public List<GameObject> enemyTypeList = new List<GameObject>();

    public Material highlightedMaterial;
    public AllyCharactersPlacer allyCharactersPlacer;

    [SerializeField] private SpawnZone enemySpawnZone;
    [SerializeField] private SpawnZone allySpawnZone;

    private int lastEnnemyPriority = 0;
    private int lastAllyPriority = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void InitAllyPlacing()
    {
        List<TileProperties> freeTiles = allySpawnZone.GetTiles();

        ResetAllTilesSpawnableState();
        SetAllTilesInAllySpawnAsSpawnable(freeTiles);
        SetHighlightAllySpawnZone(freeTiles, true);

        allyCharactersPlacer.InitPlacing();
    }

    public void EndAllyPlacing()
    {
        List<TileProperties> freeTiles = allySpawnZone.GetTiles(false);
        SetHighlightAllySpawnZone(freeTiles, false);
    }

    public void SetHighlightAllySpawnZone(List<TileProperties> freeTiles, bool value)
    {
        if (value)
        {
            foreach (TileProperties tp in freeTiles)
            {
                tp.GetComponentInChildren<MeshRenderer>().materials = new Material[] { tp.GetComponentInChildren<MeshRenderer>().materials[0], highlightedMaterial };
            }
        }
        else
        {
            foreach (TileProperties tp in freeTiles)
            {
                tp.GetComponentInChildren<MeshRenderer>().materials = new Material[] { tp.GetComponentInChildren<MeshRenderer>().materials[0] };
            }
        }

    }

    private void SetAllTilesInAllySpawnAsSpawnable(List<TileProperties> freeTiles)
    {
        foreach (TileProperties tp in freeTiles)
        {
            tp.isAllySpawnable = true;
        }
    }

    private void ResetAllTilesSpawnableState()
    {
        foreach (TileProperties tp in TilesManager.Instance.AllTilesList)
        {
            tp.isAllySpawnable = false;
        }
    }

    // Spawn Ennemy characters
    public void SpawnEnemyCharacterRandomly(int ennemyNumber = 1,int ennemyType=0)
    {
        List<TileProperties> freeTiles = enemySpawnZone.GetTiles();

        if (freeTiles.Count != 0)
        {
            for (int i = 0; i < ennemyNumber; i++)
            {
                GameObject _enemy = Instantiate(enemyTypeList[ennemyType], PickTileRandomly(freeTiles).transform.position + Vector3.up, Quaternion.Euler(0,90,0));
                EnemyCharacter _enemyChar = _enemy.GetComponent<EnemyCharacter>();
                _enemyChar.SetOccupiedTile();
                _enemyChar.priority = lastEnnemyPriority;
                enemyCharacters.Add(_enemyChar);
                lastEnnemyPriority++;
            }
        }
    }

    //Used for Waves
    public void SpawnEnemyCharacterAtPos(Vector2 gridPos, int ennemyType = 0)
    {
        GameObject _enemy = Instantiate(enemyTypeList[ennemyType], new Vector3(gridPos.x * 2 + 1, 1, gridPos.y * 2 + 1), Quaternion.Euler(0, 90, 0));
        EnemyCharacter _enemyChar = _enemy.GetComponent<EnemyCharacter>();
        if (_enemyChar.GetSpawnableTile())
        {
            _enemyChar.SetOccupiedTile();
            _enemyChar.priority = lastEnnemyPriority;
            enemyCharacters.Add(_enemyChar);
            lastEnnemyPriority++;
        }
        else
        {
            Destroy(_enemy);
            SpawnEnemyCharacterRandomly(1,ennemyType);
        }
    }

    public void SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            SpawnEnemyCharacterAtPos(wave.enemies[i].gridPosition, (int)wave.enemies[i].type);
        }
    }

    public void DestroyEnemy(int index)
    {
        Destroy(enemyCharacters[index].gameObject);
        enemyCharacters.RemoveAt(index);
        Debug.Log("destroyedEnemy");
    }

    public void KillCharacter(Character character)
    {
        character.myState = CharacterState.Dead;

        //Do something when character die

    }

    private void DestroyAlly(int index)
    {
        Destroy(allyCharacter[index].gameObject);
        allyCharacter.RemoveAt(index);
        Debug.Log("destroyedAlly");
    }

    private TileProperties PickTileRandomly(List<TileProperties> listFreeTiles)
    {
        return listFreeTiles[Random.Range(0, listFreeTiles.Count)];
    }

    public void AddXpToAllAllies(int value)
    {
        foreach (AllyCharacter ally in allyCharacter)
        {
            ally.AddExperience(value);
        }
    }

    public int GetAliveCharacterCount()
    {
        int count = 0;

        for (int i = 0; i < allyCharacter.Count; i++)
        {
            if (allyCharacter[i].myState != CharacterState.Dead)
            {
                count++;
            }
        }
        return count;
    }

    public int GeteEnemyCharacterCount()
    {
        int count = 0;

        for (int i = 0; i < enemyCharacters.Count; i++)
        {
            if (enemyCharacters[i].myState != CharacterState.Dead)
            {
                count++;
            }
        }
        return count;
    }


}
