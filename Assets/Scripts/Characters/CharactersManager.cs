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
                tp.GetComponent<MeshRenderer>().materials = new Material[] { tp.GetComponent<MeshRenderer>().materials[0], highlightedMaterial };
            }
        }
        else
        {
            foreach (TileProperties tp in freeTiles)
            {
                tp.GetComponent<MeshRenderer>().materials = new Material[] { tp.GetComponent<MeshRenderer>().materials[0] };
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
    public void SpawnEnemyCharacterRandomly(int ennemyNumber = 1)
    {
        List<TileProperties> freeTiles = enemySpawnZone.GetTiles();

        if (freeTiles.Count != 0)
        {
            for (int i = 0; i < ennemyNumber; i++)
            {
                GameObject _enemy = Instantiate(enemyTypeList[0], PickTileRandomly(freeTiles).transform.position + Vector3.up, Quaternion.identity);
                EnemyCharacter _enemyChar = _enemy.GetComponent<EnemyCharacter>();
                _enemyChar.SetOccupiedTile();
                _enemyChar.priority = lastEnnemyPriority;
                enemyCharacters.Add(_enemyChar);
                lastEnnemyPriority++;
            }
        }
    }

    //Used for Waves
    public void SpawnEnemyCharacterAtPos(Vector2 gridPos)
    {
        GameObject _enemy = Instantiate(enemyTypeList[0], new Vector3(gridPos.x * 2 + 1, 0, gridPos.y * 2 + 1) + Vector3.up, Quaternion.identity);
        EnemyCharacter _enemyChar = _enemy.GetComponent<EnemyCharacter>();
        _enemyChar.SetOccupiedTile();
        _enemyChar.priority = lastEnnemyPriority;
        enemyCharacters.Add(_enemyChar);
        lastEnnemyPriority++;
    }

    private TileProperties PickTileRandomly(List<TileProperties> listFreeTiles)
    {
        return listFreeTiles[Random.Range(0, listFreeTiles.Count)];
    }
}
