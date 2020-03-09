using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private static PlayerControl _instance;
    public static PlayerControl Instance { get { return _instance; } }

    public KeyCode TileSwapKey;
    public KeyCode TileRotateKey;
    public KeyCode TileClearKey;

    private bool isPlacingAllyCharacters = false;
    private bool isOnRotateMode = false;
    private bool isOnSwapMode = false;
    private bool inputsEnabled = true;

    private AllyCharactersPlacer allyCharactersPlacer;
    public PatternTemplate pattern;

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

                    if (isOnSwapMode)
                    {
                        TilesManager.TilesChangerInstance.TryChangePos(_tile.gameObject);
                    }

                    if (isOnRotateMode)
                    {
                        if (!TilesManager.TilesChangerInstance.RotateTile())
                            DoActionWithPANeeded();
                    }
                }
            }

            if (!isPlacingAllyCharacters)
            {
                if (Input.GetKeyDown(TileSwapKey) && isOnSwapMode)
                {
                    if (TilesManager.TilesChangerInstance.InitChange())
                        DoActionWithPANeeded();
                }

                if (Input.GetKeyDown(TileClearKey))
                {
                    TilesManager.TilesChangerInstance.ClearChoice();
                }
            }
        }
    }

    private void DoActionWithPANeeded()
    {
        GameTracker.Instance.PlayerAction();

        if (!GameTracker.Instance.IsHavingEnoughtPa())
        {
            EndAllModes();
        }
    }

    public void SetSwapMode(bool value)
    {
        EndAllModes();

        if (GameTracker.Instance.IsHavingEnoughtPa())
        {
            isOnSwapMode = value;
            UIManager.Instance.DisplaySwap(value);
        }
    }

    public void SetRotateMode(bool value)
    {
        EndAllModes();

        if (GameTracker.Instance.IsHavingEnoughtPa())
        {
            isOnRotateMode = value;
            UIManager.Instance.DisplayRotate(value);
        }
    }

    public void EndAllModes()
    {
        isOnRotateMode = false;
        isOnSwapMode = false;
        UIManager.Instance.HideFollowMouseObj();
    }

    public void EnableInputs(bool newState)
    {
        inputsEnabled = newState;
    }
}
