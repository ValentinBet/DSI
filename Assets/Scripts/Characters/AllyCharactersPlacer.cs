using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AllyCharactersPlacer : MonoBehaviour
{
    public UnityAction<bool> PlacingAllyCharacters;

    public bool isPlacingAllys = false;
    private int lastAllyPriority = 0;

    public List<GameObject> allyList = new List<GameObject>();

    public void InitPlacing()
    {
        lastAllyPriority = 0;
        PlacingAllyCharacters?.Invoke(true);
        isPlacingAllys = true;
    }

    public void StopPlacing()
    {
        PlacingAllyCharacters?.Invoke(false);
        isPlacingAllys = false;

        CharactersManager.Instance.EndAllyPlacing();
    }

    public void TryPlaceAlly(TileProperties tile)
    {
        if (IsAllAllyedSpawned())
        {
            StopPlacing();
            return;
        }

        if (isPlacingAllys)
        {
            if (tile.isAllySpawnable && tile.CharacterCanSpawn())
            {
                GameObject _ally = Instantiate(allyList[lastAllyPriority], tile.transform.position + Vector3.up, Quaternion.identity);
                AllyCharacter _allyChar = _ally.GetComponent<AllyCharacter>();
                _allyChar.SetOccupiedTile();
                _allyChar.priority = lastAllyPriority;
                CharactersManager.Instance.allyCharacter.Add(_allyChar);
                lastAllyPriority++;
            }
        }
    }

    private bool IsAllAllyedSpawned()
    {
        if (lastAllyPriority == allyList.Count)
        {
            return true;
        }
        return false;
    }
}
