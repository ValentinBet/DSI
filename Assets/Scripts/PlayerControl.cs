using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public KeyCode TileClearKey;
    public KeyCode TileSwapKey;
    public KeyCode TileRotateKey;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (GridManager.Instance.GetTileUnderSelector() != null)
            {
                TilesManager.TilesChangerInstance.TryChangePos(GridManager.Instance.GetTileUnderSelector().gameObject);
            }
        }

        if (Input.GetKeyDown(TileClearKey))
        {
            TilesManager.TilesChangerInstance.ClearChoice();

            CharactersManager.Instance.SpawnEnemyCharacterRandomly();
            CharactersManager.Instance.InitAllyPlacing();
        }

        if (Input.GetKeyDown(TileSwapKey))
        {
            TilesManager.TilesChangerInstance.InitChange();
        }
        if (Input.GetKeyDown(TileRotateKey))
        {
            TilesManager.TilesChangerInstance.RotateTile();
        }

    }
}
