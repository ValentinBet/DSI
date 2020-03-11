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
    public KeyCode TileQuitKey;

    private bool isPlacingAllyCharacters = false;
    private bool isOnRotateMode = false;
    private bool isOnSwapMode = false;
    private bool inputsEnabled = true;

    private AllyCharactersPlacer allyCharactersPlacer;
    private TileProperties lastTileHit;
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

                    if (_tile.isMovable && !_tile.isOccupied)
                    {
                        if (isOnSwapMode)
                        {
                            if (TilesManager.TilesChangerInstance.TryChangePos(_tile.gameObject))
                            {
                                DoActionWithPANeeded();
                            }

                            UIManager.Instance.DisplayCancelHelpKey(true);
                        }

                        if (isOnRotateMode)
                        {
                            TilesManager.TilesChangerInstance.DisplayRotateHint();
                            if (!TilesManager.TilesChangerInstance.TryRotateTile())
                                DoActionWithPANeeded();
                        }
                    }
                }
            }

            if (!isPlacingAllyCharacters)
            {
                if (Input.GetKeyDown(TileClearKey))
                {
                    TilesManager.TilesChangerInstance.ClearChoice();
                    UIManager.Instance.DisplayCancelHelpKey(false);
                }

                if (Input.GetKeyDown(TileQuitKey))
                {
                    EndAllModes();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isPlacingAllyCharacters)
        {
            TileProperties _tile = GridManager.Instance.GetTileUnderSelector();

            if (_tile != lastTileHit)
            {
                lastTileHit = _tile;
                //PatternReader.instance.PreviewPattern.EndPreview();
                //PatternReader.instance.PreviewPattern.ReadPattern(CharactersManager.Instance.allyCharactersPlacer.GetActualAllyToBePlace());

            }
        }
    }

    private void DoActionWithPANeeded(int pa = 1)
    {
        GameTracker.Instance.PlayerAction(pa);
        UIManager.Instance.DisplayCancelHelpKey(false);
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

        isOnRotateMode = value;
        UIManager.Instance.DisplayRotate(value);

    }

    public void EndAllModes()
    {
        isOnRotateMode = false;
        isOnSwapMode = false;
        TilesManager.TilesChangerInstance.HideAllHints();
        UIManager.Instance.EndMode();
    }

    public void EnableInputs(bool newState)
    {
        inputsEnabled = newState;
    }
}
