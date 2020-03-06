using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewReader : MonoBehaviour
{
    public List<TileProperties> tileColoredDuringPattern = new List<TileProperties>();

    public void PreviewPattern(PatternTemplate pattern, TileProperties tileOrigin)
    {
        TileProperties currentTile = tileOrigin;
        int depth = pattern.actions.Length;
        Vector3 direction = Vector3.zero;
        switch (pattern.initialDirection)
        {
            case Cardinal.North:
                direction = -Vector3.right;
                break;
            case Cardinal.East:
                direction = Vector3.forward;
                break;
            case Cardinal.South:
                direction = Vector3.right;
                break;
            case Cardinal.West:
                direction = -Vector3.forward;
                break;
            default:
                break;
        }


        PreviewAction(currentTile, pattern, 0, depth, direction);

    }
    private void PreviewAction(TileProperties currentTile, PatternTemplate pattern, int index, int depth, Vector3 currentDirection)
    {
        tileColoredDuringPattern.Add(currentTile);


        int rayLength = 2;
        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentDirection, rayLength, false);
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
                        TilesManager.Instance.ChangeTileMaterial(testedTile, PatternReader.instance.attackMat);
                        StopPattern(currentTile);
                        return;
                    }

                    TileCheckReview(pattern, currentTile, index, depth, testedTile, currentDirection);
                }
                break;

            #region Action rotation / Attack 

            case ActionType.Rotation:
                TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.rotationMat);


                Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                switch (pattern.actions[index].rotation)
                {
                    case Rotation.Left:
                        //rotationAmount = rotationAmount - 90;
                        rotation = Quaternion.Euler(0f, -90f, 0f);
                        break;
                    case Rotation.Rigth:
                        // rotationAmount = rotationAmount + 90;
                        rotation = Quaternion.Euler(0f, 90f, 0f);
                        break;
                    case Rotation.Reverse:
                        //rotationAmount = rotationAmount + 180;
                        rotation = Quaternion.Euler(0f, -180f, 0f);
                        break;
                    default:
                        // rotationAmount = rotationAmount + 0;
                        rotation = Quaternion.Euler(0f, 0f, 0f);
                        break;
                }
                currentDirection = rotation * currentDirection;
                ActionPreviewEnd(currentTile, currentTile, pattern, index, depth, currentDirection);
                break;

            case ActionType.Attack:
                TilesManager.Instance.ChangeTileMaterial(testedTile, PatternReader.instance.attackMat);
                ActionPreviewEnd(currentTile, testedTile, pattern, index, depth, currentDirection);
                break;

            default:
                break;
                #endregion
        }
    }

    private void TileCheckReview(PatternTemplate pattern, TileProperties currentTile, int index, int depth, TileProperties newTile, Vector3 currentDirection)
    {
        switch (newTile.specificity)
        {
            case TileProperties.TilesSpecific.None:
                MovementOnTilePreview(pattern, currentTile, index, depth, newTile, currentDirection);
                break;

            case TileProperties.TilesSpecific.Ordre:
                switch (newTile.order)
                {
                    case TileProperties.TilesOrder.rotate:

                        currentTile = newTile;
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                        tileColoredDuringPattern.Add(currentTile);
                        StartCoroutine(ExtraRotationReview(currentTile, currentDirection, pattern, index, depth));


                        break;
                    case TileProperties.TilesOrder.attack:
                        currentTile = newTile;
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);

                        int rayLength = 2;
                        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentDirection, rayLength, false);
                        if (tiles.Count == 0)
                        {
                            print("Rien");
                            ActionPreviewEnd(currentTile, currentTile, pattern, index, depth, currentDirection);
                        }
                        else
                        {
                            TileProperties testedTile = tiles[0];
                            tileColoredDuringPattern.Add(currentTile);
                            StartCoroutine(ExtraAttackReview(currentTile, pattern, index, depth, testedTile, true, currentDirection));
                        }
                        break;

                    case TileProperties.TilesOrder.stop:
                        currentTile = newTile;
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                        StopPattern(currentTile);
                        break;
                    default:
                        break;
                }
                break;

            case TileProperties.TilesSpecific.Push:
                currentTile = newTile;
                TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                tileColoredDuringPattern.Add(currentTile);
                StartCoroutine(ExtraDeplacementReview(currentTile, currentDirection, pattern, index, depth));
                break;

            case TileProperties.TilesSpecific.Door:
                if (newTile.isActivated)
                {
                    newTile.ChangeTilesActivationStatut(false);
                    tileColoredDuringPattern.Add(newTile);
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    StopPattern(currentTile);
                }
                else
                {
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    MovementOnTilePreview(pattern, currentTile, index, depth, newTile, currentDirection);
                }

                break;
            case TileProperties.TilesSpecific.Wall:

                StartCoroutine(ExtraAttackReview(currentTile, pattern, index, depth, newTile, false, currentDirection));

                break;
            case TileProperties.TilesSpecific.Teleport:
                currentTile = newTile;
                tileColoredDuringPattern.Add(currentTile);
                TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.interactionMat);
                StartCoroutine(TeleportationReview(currentTile, currentDirection, pattern, index, depth));
                break;


            case TileProperties.TilesSpecific.Trap:
                //faudra prevoir rajouter le prévition de mort
                TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                StartCoroutine(GetDamagedReview(currentTile, pattern, index, depth, true, currentDirection));
                break;

            default:
                StopPattern(currentTile);
                break;
        }
    }
    private void MovementOnTilePreview(PatternTemplate pattern, TileProperties currentTile, int index, int depth, TileProperties newTile, Vector3 currentDirection)
    {
        currentTile = newTile;
        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
        if (newTile.isOnFire)
        {
            StartCoroutine(GetDamagedReview(currentTile, pattern, index, depth, true, currentDirection));
            print("OnFIre");
            return;
        }
        ActionPreviewEnd(currentTile, currentTile, pattern, index, depth, currentDirection);
    }

    #region Action D'intéruption

    private IEnumerator GetDamagedReview(TileProperties currentTile, PatternTemplate pattern, int index, int depth, bool continuePattern, Vector3 currentDirection)
    {
        //rajouter un if si le joueur meurt 
        print("ouch");
        yield return new WaitForSeconds(0.5f);
        if (continuePattern)
        {
            ActionPreviewEnd(currentTile, currentTile, pattern, index, depth, currentDirection);
        }
        else
        {
            StopPattern(currentTile);
        }
    }

    private IEnumerator ExtraAttackReview(TileProperties currentTile, PatternTemplate pattern, int index, int depth, TileProperties targetTile, bool continuePatern, Vector3 currentDirection)
    {
        print("Attack ");
        yield return new WaitForSeconds(0.5f);
        tileColoredDuringPattern.Add(targetTile);
        TilesManager.Instance.ChangeTileMaterial(targetTile, PatternReader.instance.attackMat);
        if (continuePatern)
        {
            ActionPreviewEnd(currentTile, targetTile, pattern, index, depth, currentDirection);
        }
        else
        {
            StopPattern(targetTile);
        }
    }

    private IEnumerator ExtraRotationReview(TileProperties currentTile, Vector3 currentDirection, PatternTemplate pattern, int index, int depth)
    {
        yield return new WaitForSeconds(0.5f);
        TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.interactionMat);
        Quaternion rotation = Quaternion.Euler(0, currentTile.GetRotationOffset(currentDirection), 0);

        currentDirection = rotation * currentDirection;
        ActionPreviewEnd(currentTile, currentTile, pattern, index, depth, currentDirection);
    }

    private IEnumerator ExtraDeplacementReview(TileProperties currentTile, Vector3 currentDirection, PatternTemplate pattern, int index, int depth)
    {
        print("Push ");
        yield return new WaitForSeconds(0.5f);
        //TilesManager.Instance.ChangeTileMaterial(currentTile, interactionMat);

        int rayLength = 2;
        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentTile.GetCurrentForward(), rayLength, false);
        if (tiles.Count == 0)
        {
            print("Rien");
            StartCoroutine(GetDamagedReview(currentTile, pattern, index, depth, false, currentDirection));
        }
        else
        {
            TileProperties testedTile = tiles[0];
            TileCheckReview(pattern, currentTile, index, depth, testedTile, currentDirection);
            //Debug.Log(testedTile, testedTile);
        }
    }

    private IEnumerator TeleportationReview(TileProperties currentTile, Vector3 currentDirection, PatternTemplate pattern, int index, int depth)
    {
        yield return new WaitForSeconds(0.5f);

        //TilesManager.Instance.ChangeTileMaterial(currentTile, interactionMat);
        TileProperties teleportExit = currentTile.GetTeleportExit();
        if (teleportExit == null)
        {
            StopPattern(currentTile);
        }
        else
        {
            TilesManager.Instance.ChangeTileMaterial(teleportExit, PatternReader.instance.interactionMat);


            Quaternion rotation = Quaternion.Euler(0, teleportExit.GetRotationOffset(currentDirection), 0);

            currentDirection = rotation * currentDirection;

            ActionPreviewEnd(teleportExit, teleportExit, pattern, index, depth, currentDirection);
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
    private IEnumerator NextPreview(float duration, TileProperties currentTile, PatternTemplate pattern, int index, int depth, Vector3 currentDirection)
    {
        yield return new WaitForSeconds(duration);
        PreviewAction(currentTile, pattern, index, depth, currentDirection);
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
    private void ActionPreviewEnd(TileProperties currentTile, TileProperties tileToColored, PatternTemplate pattern, int index, int depth, Vector3 currentDirection)
    {
        index++;
        if (index < depth)
        {
            StartCoroutine(NextPreview(pattern.actions[index].actionDuration, currentTile, pattern, index, depth, currentDirection));
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
}
