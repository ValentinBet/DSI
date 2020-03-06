using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewReader : MonoBehaviour
{
    public void PreviewPattern(PatternTemplate pattern, TileProperties tileOrigin, Vector3 direction)
    {
        TileProperties currentTile = tileOrigin;
        int depth = pattern.actions.Length;

        float rotationAmount = 0;


        PreviewAction(currentTile, rotationAmount, pattern, 0, depth);

    }
    private void PreviewAction(TileProperties currentTile, float rotationAmount, PatternTemplate pattern, int index, int depth)
    {
        PatternReader.instance.tileColoredDuringPattern.Add(currentTile);

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
                direction = Vector3.zero;
                break;
        }


        direction = Quaternion.AngleAxis(rotationAmount, Vector3.up) * direction;

        int rayLength = 2;
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
                        TilesManager.Instance.ChangeTileMaterial(testedTile, PatternReader.instance.attackMat);
                        StopPattern(currentTile);
                        return;
                    }

                    TileCheckReview(pattern, currentTile, rotationAmount, index, depth, testedTile, direction);
                }
                break;

            #region Action rotation / Attack 

            case ActionType.Rotation:
                TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.rotationMat);


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
                TilesManager.Instance.ChangeTileMaterial(testedTile, PatternReader.instance.attackMat);
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

                        currentTile = newTile;
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                        PatternReader.instance.tileColoredDuringPattern.Add(currentTile);
                        StartCoroutine(ExtraRotationReview(currentTile, rotationAmount, currentDirection, pattern, index, depth));


                        break;
                    case TileProperties.TilesOrder.attack:
                        currentTile = newTile;
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);

                        int rayLength = 2;
                        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentDirection, rayLength, false);
                        if (tiles.Count == 0)
                        {
                            print("Rien");
                            ActionPreviewEnd(currentTile, currentTile, rotationAmount, pattern, index, depth);
                        }
                        else
                        {
                            TileProperties testedTile = tiles[0];
                            PatternReader.instance.tileColoredDuringPattern.Add(currentTile);
                            StartCoroutine(ExtraAttackReview(currentTile, rotationAmount, pattern, index, depth, testedTile, true));
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
                PatternReader.instance.tileColoredDuringPattern.Add(currentTile);
                StartCoroutine(ExtraDeplacementReview(currentTile, rotationAmount, currentDirection, pattern, index, depth));
                break;

            case TileProperties.TilesSpecific.Door:
                if (newTile.isActivated)
                {
                    newTile.ChangeTilesActivationStatut(false);
                    PatternReader.instance.tileColoredDuringPattern.Add(newTile);
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    StopPattern(currentTile);
                }
                else
                {
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    MovementOnTilePreview(pattern, currentTile, rotationAmount, index, depth, newTile);
                }

                break;
            case TileProperties.TilesSpecific.Wall:

                StartCoroutine(ExtraAttackReview(currentTile, rotationAmount, pattern, index, depth, newTile, false));

                break;
            case TileProperties.TilesSpecific.Teleport:
                currentTile = newTile;
                PatternReader.instance.tileColoredDuringPattern.Add(currentTile);
                TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.interactionMat);
                StartCoroutine(TeleportationReview(currentTile, rotationAmount, currentDirection, pattern, index, depth));
                break;


            case TileProperties.TilesSpecific.Trap:
                //faudra prevoir rajouter le prévition de mort
                TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                StartCoroutine(GetDamagedReview(currentTile, rotationAmount, pattern, index, depth, true));
                break;

            default:
                StopPattern(currentTile);
                break;
        }
    }
    private void MovementOnTilePreview(PatternTemplate pattern, TileProperties currentTile, float rotationAmount, int index, int depth, TileProperties newTile)
    {
        currentTile = newTile;
        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
        if (newTile.isOnFire)
        {
            StartCoroutine(GetDamagedReview(currentTile, rotationAmount, pattern, index, depth, true));
            print("OnFIre");
            return;
        }
        ActionPreviewEnd(currentTile, currentTile, rotationAmount, pattern, index, depth);
    }

    #region Action D'intéruption

    private IEnumerator GetDamagedReview(TileProperties currentTile, float rotationAmount, PatternTemplate pattern, int index, int depth, bool continuePattern)
    {
        //rajouter un if si le joueur meurt 
        print("ouch");
        yield return new WaitForSeconds(0.5f);
        if (continuePattern)
        {
            ActionPreviewEnd(currentTile, currentTile, rotationAmount, pattern, index, depth);
        }
        else
        {
            StopPattern(currentTile);
        }
    }

    private IEnumerator ExtraAttackReview(TileProperties currentTile, float rotationAmount, PatternTemplate pattern, int index, int depth, TileProperties targetTile, bool continuePatern)
    {
        print("Attack ");
        yield return new WaitForSeconds(0.5f);
        PatternReader.instance.tileColoredDuringPattern.Add(targetTile);
        TilesManager.Instance.ChangeTileMaterial(targetTile, PatternReader.instance.attackMat);
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
        TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.interactionMat);
        rotationAmount += currentTile.GetRotationOffset(currentDirection);
        ActionPreviewEnd(currentTile, currentTile, rotationAmount, pattern, index, depth);
    }

    private IEnumerator ExtraDeplacementReview(TileProperties currentTile, float rotationAmount, Vector3 currentDirection, PatternTemplate pattern, int index, int depth)
    {
        print("Push ");
        yield return new WaitForSeconds(0.5f);
        //TilesManager.Instance.ChangeTileMaterial(currentTile, interactionMat);

        int rayLength = 2;
        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentTile.GetCurrentForward(), rayLength, false);
        if (tiles.Count == 0)
        {
            print("Rien");
            StartCoroutine(GetDamagedReview(currentTile, rotationAmount, pattern, index, depth, false));
        }
        else
        {
            TileProperties testedTile = tiles[0];
            TileCheckReview(pattern, currentTile, rotationAmount, index, depth, testedTile, currentDirection);
            //Debug.Log(testedTile, testedTile);
        }
    }

    private IEnumerator TeleportationReview(TileProperties currentTile, float rotationAmount, Vector3 currentDirection, PatternTemplate pattern, int index, int depth)
    {
        print("Push ");
        yield return new WaitForSeconds(0.5f);

        //TilesManager.Instance.ChangeTileMaterial(currentTile, interactionMat);
        TileProperties teleportExit = currentTile.GetTeleportExit();
        if (teleportExit == null)
        {
            StopPattern(currentTile);
        }
        else
        {
            //tileColoredDuringPattern.Add(currentTile);
            TilesManager.Instance.ChangeTileMaterial(teleportExit, PatternReader.instance.interactionMat);
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
        for (int i = 0; i < PatternReader.instance.tileColoredDuringPattern.Count; i++)
        {
            TilesManager.Instance.ChangeTileMaterial(PatternReader.instance.tileColoredDuringPattern[i], PatternReader.instance.tileColoredDuringPattern[i].baseMat);
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
            PatternReader.instance.tileColoredDuringPattern.Add(tileToColored);
            StartCoroutine(EndPreview(0.5f));
        }
    }

    private void StopPattern(TileProperties tileToColored)
    {

        PatternReader.instance.tileColoredDuringPattern.Add(tileToColored);
        StartCoroutine(EndPreview(0.5f));
    }
    #endregion
}
