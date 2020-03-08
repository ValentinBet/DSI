using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AllyCharactersPlacer : MonoBehaviour
{
    public UnityAction<bool> PlacingAllyCharacters;

    public bool isPlacingAllys = false;
    private int AllyPriority = 0;

    public void InitPlacing()
    {
        AllyPriority = 0;
        PlacingAllyCharacters?.Invoke(true); // for non singleton
        isPlacingAllys = true;
        UpdateUIToActualAlly();
        UIManager.Instance.isPlacingAlly = true;
    }

    public void StopPlacing()
    {
        PlacingAllyCharacters?.Invoke(false);
        isPlacingAllys = false;
        CharactersManager.Instance.EndAllyPlacing();
        PhaseManager.Instance.NextPhase();
        UIManager.Instance.AllyLifeSetup();
        UIManager.Instance.SetAllyHintState(false);
        UIManager.Instance.isPlacingAlly = false;
        UIManager.Instance.NewTurn();
    }

    public void TryPlaceAlly(TileProperties tile)
    {
        if (isPlacingAllys)
        {
            if (tile.isAllySpawnable && tile.CharacterCanSpawn())
            {
                GameObject _ally = Instantiate(GetCharacterObjByType(GameInfoManager.GameData.allies[AllyPriority].type), tile.transform.position + Vector3.up, Quaternion.identity);
                AllyCharacter _allyChar = _ally.GetComponent<AllyCharacter>();
                GameInfoManager.Instance.UpdateCharacterObjToHisSave(ref _allyChar, GameInfoManager.GameData.allies[AllyPriority]);
                _allyChar.SetOccupiedTile();
                _allyChar.priority = AllyPriority;
                CharactersManager.Instance.allyCharacter.Add(_allyChar);
                UIManager.Instance.SetAllyLevelDisplay(AllyPriority);
                AllyPriority++;
            }
        }

        if (IsAllAllyedSpawned())
        {
            StopPlacing();
            return;
        }
        else
        {
            UpdateUIToActualAlly();
        }
    }

    private void UpdateUIToActualAlly()
    {
        AllyCharacter _ac = GetCharacterObjByType(GameInfoManager.GameData.allies[AllyPriority].type).GetComponent<AllyCharacter>();
        UIManager.Instance.SetAllyHintState(true, _ac.ObjectTypeMetaData.icon);
        UIManager.Instance.SetClusterInfo(_ac.data.name,_ac.data.allyDescription, _ac.ObjectTypeMetaData.icon);
    }

    private bool IsAllAllyedSpawned()
    {
        if (AllyPriority == GameInfoManager.GameData.allies.Count)
        {
            return true;
        }
        return false;
    }

    private GameObject GetCharacterObjByType(CharacterType type = CharacterType.Warrior)
    {
      return GameInfoManager.Instance.charactersGenerator.GetBaseAllyList()[(int) type];
    }
}
