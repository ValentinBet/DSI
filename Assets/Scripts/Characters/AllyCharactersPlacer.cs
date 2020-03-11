using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AllyCharactersPlacer : MonoBehaviour
{
    public UnityAction<bool> PlacingAllyCharacters;

    public bool isPlacingAllys = false;
    private int AllyPriority = 0;

    private GameObject allyHover;
    private Character allyHoverChara;
    private TileProperties lastTile;

    private List<TileProperties> tileOccupedByAllies = new List<TileProperties>();
    private void Update()
    {
        HoverAlly();
    }
    public void InitPlacing()
    {
        AllyPriority = 0;
        SpawnNextAlly();
        PlacingAllyCharacters?.Invoke(true); // for non singleton
        isPlacingAllys = true;
        UpdateUIToActualAlly();
        UIManager.Instance.isPlacingAlly = true;
    }

    public void StopPlacing()
    {
        isPlacingAllys = false;
        PlacingAllyCharacters?.Invoke(false);
        CharactersManager.Instance.EndAllyPlacing();
        PhaseManager.Instance.NextPhase();
        UIManager.Instance.AllyLifeSetup();
        UIManager.Instance.SetAllyHintState(false);
        UIManager.Instance.isPlacingAlly = false;
        UIManager.Instance.NewTurn();
    }

    private void SpawnNextAlly()
    {
        allyHover = Instantiate(GetCharacterObjByType(GameInfoManager.GameData.allies[AllyPriority].type),
            GridManager.Instance.gridSelector.transform.position + Vector3.up,
            Quaternion.Euler(0, -90, 0));
        allyHoverChara = allyHover.GetComponent<AllyCharacter>();
    }

    private void HoverAlly()
    {
        if (isPlacingAllys && (lastTile != GridManager.Instance.GetTileUnderSelector()) && GridManager.Instance.GetTileUnderSelector() != null)
        {
            TileProperties _tile = GridManager.Instance.GetTileUnderSelector();

            if (_tile.isOccupied)
            {
                return;
            }

            if (lastTile != null && !tileOccupedByAllies.Contains(lastTile))
            {
                lastTile.isOccupied = false;
                lastTile.occupant = null;
            }

            lastTile = _tile;

            if (lastTile != null)
            {
                allyHover.transform.position = lastTile.gameObject.transform.position + Vector3.up;
                // Preview pattern purpose >>
                allyHover.GetComponent<Character>().SetOccupiedTile();
            }

        }
    }

    public void TryPlaceAlly(TileProperties tile)
    {
        if (isPlacingAllys)
        {
            if (tile.isAllySpawnable && tile.CharacterCanSpawn(allyHoverChara))
            {
                AllyCharacter _allyChar = allyHover.GetComponent<AllyCharacter>();
                GameInfoManager.Instance.UpdateCharacterObjToHisSave(ref _allyChar, GameInfoManager.GameData.allies[AllyPriority]);
                _allyChar.SetOccupiedTile();
                _allyChar.priority = AllyPriority;
                CharactersManager.Instance.allyCharacter.Add(_allyChar);
                UIManager.Instance.SetAllyLevelDisplay(AllyPriority);
                AllyPriority++;
                tileOccupedByAllies.Add(lastTile);

                if (!IsAllAllyedSpawned())
                {
                    SpawnNextAlly();
                    UpdateUIToActualAlly();
                }
                else
                {
                    StopPlacing();
                    return;
                }
            }
        }
    }

    public Character GetActualAllyToBePlace()
    {
        return GetCharacterObjByType(GameInfoManager.GameData.allies[AllyPriority].type).GetComponent<Character>();
    }

    private void UpdateUIToActualAlly()
    {
        AllyCharacter _ac = GetCharacterObjByType(GameInfoManager.GameData.allies[AllyPriority].type).GetComponent<AllyCharacter>();
        UIManager.Instance.SetAllyHintState(true, _ac.ObjectTypeMetaData.icon);
        UIManager.Instance.SetClusterInfo(_ac.allyName, _ac.allyDescription, _ac.ObjectTypeMetaData.icon);
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
        return GameInfoManager.Instance.charactersGenerator.GetBaseAllyList()[(int)type];
    }
}
