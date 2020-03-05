using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternReader : MonoBehaviour
{
    public static PatternReader instance;

    private List<TileProperties> tileColoredDuringPattern = new List<TileProperties>();

    public Material mouvementMat, attackMat, rotationMat, clickMat, interactionMat;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }


    #region Execute Pattern 
    public void ExecutePattern(PatternTemplate pattern, Character character)
    {
        int depth = pattern.actions.Length;
        ExecuteAction(character, pattern, 0, depth);
    }

    private void ExecuteAction(Character character, PatternTemplate pattern, int index, int depth)
    {
        TileProperties characterTile = character.occupiedTile;


        switch (pattern.actions[index].actionType)
        {
            case ActionType.Movement:
                TileProperties frontTile = characterTile.GetTileOnDirection(character.transform.forward, 1, false)[0];
                if (frontTile != null)
                {
                    if (frontTile.isOccupied)
                    {
                        Debug.Log("Attack");
                        character.InitAttack();
                        FinishTurn();
                    }
                }
                break;

            case ActionType.Rotation:
                float rotationAmount;
                switch (pattern.actions[index].rotation)
                {
                    case Rotation.Left:
                        rotationAmount = -90;
                        break;
                    case Rotation.Rigth:
                        rotationAmount = 90;
                        break;
                    case Rotation.Reverse:
                        rotationAmount = 180;
                        break;
                    default:
                        rotationAmount = 0;
                        break;
                }
                character.transform.Rotate(character.transform.up, rotationAmount);

                break;

            case ActionType.Attack:
                Debug.Log("Attack");
                break;

            default:
                break;
        }

        index++;
        if (index < depth)
        {
            StartCoroutine(NextAction(pattern.actions[index].actionDuration, character, pattern, index, depth));
        }
        else
        {
            character.myState = CharacterState.Finished;
            FinishTurn();
        }
    }


    public void InterruptPattern(int index)
    {

    }

    private IEnumerator NextAction(float duration, Character character, PatternTemplate pattern, int index, int depth)
    {
        yield return new WaitForSeconds(duration);
        ExecuteAction(character, pattern, index, depth);
    }
    #endregion

    #region Preview PAttern

    public void PreviewPattern(PatternTemplate pattern, TileProperties tileOrigin, Vector3 direction)
    {
        TileProperties currentTile = tileOrigin;
        int depth = pattern.actions.Length;

        float rotationAmount = 0;


        PreviewAction(currentTile, rotationAmount, pattern, 0, depth);

    }
    private void PreviewAction(TileProperties currentTile, float rotationAmount, PatternTemplate pattern, int index, int depth)
    {
        tileColoredDuringPattern.Add(currentTile);
        print(rotationAmount);
        Vector3 direction = Vector3.forward;
        direction = Quaternion.AngleAxis(rotationAmount, Vector3.up) * direction;

        int rayLength = 1;
        List<TileProperties> tiles = currentTile.GetTileOnDirection(direction, rayLength, false);
        if (tiles.Count == 0)
        {
            print("Rien");
            StopPattern(currentTile);
            return;
        }
        TileProperties testedTile = tiles[0];
        switch (pattern.actions[index].actionType)
        {
            case ActionType.Movement:
                if (testedTile != null)
                {

                    if (testedTile.isOccupied)
                    {
                        TilesManager.Instance.ChangeTileMaterial(testedTile, attackMat);
                        StopPattern(currentTile);
                        return;
                    }

                    TileCheckReview(pattern, currentTile, rotationAmount, index, depth, testedTile, direction);
                }
                break;

            #region Action rotation / Attack 

            case ActionType.Rotation:
                TilesManager.Instance.ChangeTileMaterial(currentTile, rotationMat);


                //float newRotation;
                switch (pattern.actions[index].rotation)
                {
                    case Rotation.Left:
                        rotationAmount = rotationAmount - 90;
                        break;
                    case Rotation.Rigth:
                        rotationAmount = rotationAmount + 90;
                        break;
                    case Rotation.Reverse:
                        rotationAmount = rotationAmount + 180;
                        break;
                    default:
                        rotationAmount = rotationAmount + 0;
                        break;
                }
                ActionPreviewEnd(currentTile, currentTile, rotationAmount, pattern, index, depth);
                break;

            case ActionType.Attack:
                TilesManager.Instance.ChangeTileMaterial(testedTile, attackMat);
                ActionPreviewEnd(currentTile, testedTile, rotationAmount, pattern, index, depth);
                break;

            default:
                break;
                #endregion
        }
    }



    private void TileCheckReview(PatternTemplate pattern, TileProperties currentTile, float rotationAmount, int index, int depth, TileProperties newTile, Vector3 currentDirection)
    {
        switch (newTile.specificity)
        {
            case TileProperties.TilesSpecific.None:
                MovementOnTilePreview(pattern, currentTile, rotationAmount, index, depth, newTile);
                break;

            case TileProperties.TilesSpecific.Ordre:
                switch (newTile.order)
                {
                    case TileProperties.TilesOrder.rotate:
                        StartCoroutine(ExtraRotationReview(currentTile, rotationAmount, currentDirection, pattern, index, depth));
                        break;
                    case TileProperties.TilesOrder.attack:
                        StartCoroutine(ExtraAttackReview(currentTile, rotationAmount, pattern, index, depth, newTile, true));
                        break;
                    case TileProperties.TilesOrder.stop:
                        StopPattern(currentTile);
                        break;
                    default:
                        break;
                }
                break;

            case TileProperties.TilesSpecific.Push:
                StartCoroutine(ExtraDeplacementReview(currentTile, rotationAmount, currentDirection, pattern, index, depth));
                break;

            case TileProperties.TilesSpecific.Door:
                if (newTile.isActivated)
                {
                    newTile.isActivated = false;
                    TilesManager.Instance.ChangeTileMaterial(newTile, interactionMat);
                    StopPattern(currentTile);
                }
                else
                {
                    TilesManager.Instance.ChangeTileMaterial(newTile, interactionMat);
                    MovementOnTilePreview(pattern, currentTile, rotationAmount, index, depth, newTile);
                }

                break;
            case TileProperties.TilesSpecific.Wall:

                StartCoroutine(ExtraAttackReview(currentTile, rotationAmount, pattern, index, depth, newTile, false));

                break;
            case TileProperties.TilesSpecific.Teleport:
                StartCoroutine(TeleportationReview(currentTile, rotationAmount, currentDirection, pattern, index, depth));
                break;


            case TileProperties.TilesSpecific.Trap:
                //faudra prevoir rajouter le prévition de mort
                TilesManager.Instance.ChangeTileMaterial(newTile, mouvementMat);
                StartCoroutine(GetDamagedReview(currentTile, rotationAmount, pattern, index, depth));
                break;

            default:
                StopPattern(currentTile);
                break;
        }
    }
    private void MovementOnTilePreview(PatternTemplate pattern, TileProperties currentTile, float rotationAmount, int index, int depth, TileProperties newTile)
    {
        currentTile = newTile;
        TilesManager.Instance.ChangeTileMaterial(newTile, mouvementMat);
        if (newTile.isOnFire)
        {
            StartCoroutine(GetDamagedReview(currentTile, rotationAmount, pattern, index, depth));
            print("OnFIre");
            return;
        }
        ActionPreviewEnd(currentTile, currentTile, rotationAmount, pattern, index, depth);
    }

    #region Action D'intéruption

    private IEnumerator GetDamagedReview(TileProperties currentTile, float rotationAmount, PatternTemplate pattern, int index, int depth)
    {
        //rajouter un if si le joueur meurt 
        print("ouch");
        yield return new WaitForSeconds(0.5f);
        ActionPreviewEnd(currentTile, currentTile, rotationAmount, pattern, index, depth);
    }

    private IEnumerator ExtraAttackReview(TileProperties currentTile, float rotationAmount, PatternTemplate pattern, int index, int depth, TileProperties targetTile, bool continuePatern)
    {
        print("Attack ");
        yield return new WaitForSeconds(0.5f);
        TilesManager.Instance.ChangeTileMaterial(targetTile, mouvementMat);
        if (continuePatern)
        {
            ActionPreviewEnd(currentTile, targetTile, rotationAmount, pattern, index, depth);
        }
        else
        {
            StopPattern(targetTile);
        }
    }

    private IEnumerator ExtraRotationReview(TileProperties currentTile, float rotationAmount, Vector3 currentDirection, PatternTemplate pattern, int index, int depth)
    {
        print("Rotation ");
        yield return new WaitForSeconds(0.5f);
        TilesManager.Instance.ChangeTileMaterial(currentTile, interactionMat);
        rotationAmount += currentTile.GetRotationOffset(currentDirection);
        ActionPreviewEnd(currentTile, currentTile, rotationAmount, pattern, index, depth);
    }

    private IEnumerator ExtraDeplacementReview(TileProperties currentTile, float rotationAmount, Vector3 currentDirection, PatternTemplate pattern, int index, int depth)
    {
        print("Push ");
        yield return new WaitForSeconds(0.5f);
        TilesManager.Instance.ChangeTileMaterial(currentTile, interactionMat);

        int rayLength = 1;
        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentTile.GetCurrentForward(), rayLength, false);
        if (tiles.Count == 0)
        {
            StartCoroutine(GetDamagedReview(currentTile, rotationAmount, pattern, index, depth));
        }
        else
        {
            TileProperties testedTile = tiles[0];
            MovementOnTilePreview(pattern, currentTile, rotationAmount, index, depth, testedTile);
        }
    }

    private IEnumerator TeleportationReview(TileProperties currentTile, float rotationAmount, Vector3 currentDirection, PatternTemplate pattern, int index, int depth)
    {
        print("Push ");
        yield return new WaitForSeconds(0.5f);

        TilesManager.Instance.ChangeTileMaterial(currentTile, interactionMat);
        TileProperties teleportExit = currentTile.GetTeleportExit();
        if (teleportExit == null)
        {
            StopPattern(currentTile);
        }
        else
        {
            tileColoredDuringPattern.Add(currentTile);
            TilesManager.Instance.ChangeTileMaterial(teleportExit, interactionMat);
            rotationAmount += teleportExit.GetRotationOffset(currentDirection);
            ActionPreviewEnd(teleportExit, teleportExit, rotationAmount, pattern, index, depth);
        }
    }




    #endregion

    #region Coroutine

    /// <summary>
    /// Suprime la preview avec un delay
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator EndPreview(float duration)
    {
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < tileColoredDuringPattern.Count; i++)
        {
            TilesManager.Instance.ChangeTileMaterial(tileColoredDuringPattern[i], tileColoredDuringPattern[i].baseMat);
        }
    }


    /// <summary>
    /// Met un delay et preview le prochaine action
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="currentTile"></param>
    /// <param name="rotationAmount"></param>
    /// <param name="pattern"></param>
    /// <param name="index"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private IEnumerator NextPreview(float duration, TileProperties currentTile, float rotationAmount, PatternTemplate pattern, int index, int depth)
    {
        yield return new WaitForSeconds(duration);
        PreviewAction(currentTile, rotationAmount, pattern, index, depth);
    }
    #endregion

    #region Utility
    /// <summary>
    /// Appelle une courtine soit de fin de pattern soit de fin d'action
    /// </summary>
    /// <param name="currentTile"></param>
    /// <param name="rotationAmount"></param>
    /// <param name="pattern"></param>
    /// <param name="index"></param>
    /// <param name="depth"></param>
    private void ActionPreviewEnd(TileProperties currentTile, TileProperties tileToColored, float rotationAmount, PatternTemplate pattern, int index, int depth)
    {
        index++;
        if (index < depth)
        {
            StartCoroutine(NextPreview(pattern.actions[index].actionDuration, currentTile, rotationAmount, pattern, index, depth));
        }
        else
        {
            tileColoredDuringPattern.Add(tileToColored);
            StartCoroutine(EndPreview(0.5f));
        }
    }

    private void StopPattern(TileProperties tileToColored)
    {

        tileColoredDuringPattern.Add(tileToColored);
        StartCoroutine(EndPreview(0.5f));
    }
    #endregion

    #endregion



    private void FinishTurn()
    {

        PhaseManager.Instance.NextUnit();
    }

}
