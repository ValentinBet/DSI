using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private static PlayerControl _instance;
    public static PlayerControl Instance { get { return _instance; } }

    public KeyCode TileClearKey;
    public KeyCode TileSwapKey;
    public KeyCode TileRotateKey;

    private bool isPlacingAllyCharacters = false;
    private AllyCharactersPlacer allyCharactersPlacer;
    private bool inputsEnabled = true;

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

        allyCharactersPlacer = CharactersManager.Instance.allyCharactersPlacer;
    }

    private void OnEnable()
    {
        allyCharactersPlacer.PlacingAllyCharacters += SetPlacingAllyCharacters;
    }

    private void OnDisable()
    {
        allyCharactersPlacer.PlacingAllyCharacters -= SetPlacingAllyCharacters;
    }

    private void SetPlacingAllyCharacters(bool value)
    {
        isPlacingAllyCharacters = value;
    }

    void Update()
    {
        if (inputsEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (GridManager.Instance.GetTileUnderSelector() != null)
                {
                    TileProperties _tile = GridManager.Instance.GetTileUnderSelector();

                    if (isPlacingAllyCharacters)
                    {
                        allyCharactersPlacer.TryPlaceAlly(_tile);
                        return;
                    }

                    TilesManager.TilesChangerInstance.TryChangePos(_tile.gameObject);
                }
            }

        if (Input.GetKeyDown(TileClearKey))
        {
            TilesManager.TilesChangerInstance.ClearChoice();
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

    public void EnableInputs(bool newState)
    {
        inputsEnabled = newState;
    }
}
