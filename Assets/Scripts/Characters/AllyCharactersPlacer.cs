using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCharactersPlacer : MonoBehaviour
{
    public bool isPlacingAllys = false;
    private int lastAllyPriority = 0;

    public List<GameObject> allyTypeList = new List<GameObject>();

    public void InitPlacing()
    {
        isPlacingAllys = true;
    }

    public void StopPlacing()
    {
        isPlacingAllys = false;
    }

    public void PlaceAlly(TileProperties tile)
    {
        if (isPlacingAllys)
        {
            GameObject _ally = Instantiate(allyTypeList[0], tile.transform.position + Vector3.up, Quaternion.identity);
            AllyCharacter _allyChar = _ally.GetComponent<AllyCharacter>();
            _allyChar.SetOccupiedTile();
            _allyChar.priority = lastAllyPriority;
            CharactersManager.Instance.allyCharacter.Add(_allyChar);
            lastAllyPriority++;
        }
    }
}
