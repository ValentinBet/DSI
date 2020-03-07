using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternExecuter : MonoBehaviour
{

    public List<TileProperties> tileColoredDuringPattern = new List<TileProperties>();

    public void ExecutePattern(PatternTemplate pattern, Character character)
    {
        int depth = pattern.actions.Length;
        ExecuteAction(character, pattern, 0, depth);
    }

    private void ExecuteAction(Character character, PatternTemplate pattern, int index, int depth)
    {
        tileColoredDuringPattern.Add(character.occupiedTile);
        TileProperties characterTile = character.occupiedTile;

        int rayLength = 2;
        List<TileProperties> tiles = characterTile.GetTileOnDirection(character.transform.forward, rayLength, false);
        if (tiles.Count == 0)
        {
            CharacterReorientation(character, false);
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
                        StartCoroutine(ExtraAttack(pattern, character, index, depth, true));
                        return;
                    }

                    TileCheck(pattern, character, index, depth, testedTile);
                }
                break;

            case ActionType.Rotation:


                switch (pattern.actions[index].rotation)
                {
                    case Rotation.Left:
                        character.transform.Rotate(Vector3.up, -90f);
                        break;
                    case Rotation.Rigth:
                        character.transform.Rotate(Vector3.up, 90f);
                        break;
                    case Rotation.Reverse:
                        character.transform.Rotate(Vector3.up, 180f);
                        break;
                    default:
                        character.transform.Rotate(Vector3.up, 0f);
                        break;
                }
                TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.rotationMat);
                ActionEnd(pattern, character.occupiedTile, character, index, depth);
                break;

            case ActionType.Attack:
                StartCoroutine(ExtraAttack(pattern, character, index, depth, true));
                break;

            default:
                break;

        }
    }

    private void TileCheck(PatternTemplate pattern, Character character, int index, int depth, TileProperties newTile)
    {
        switch (newTile.specificity)
        {
            case TileProperties.TilesSpecific.None:
                MovementOnTile(pattern, character, index, depth, newTile);
                break;

            case TileProperties.TilesSpecific.Ordre:
                switch (newTile.order)
                {
                    case TileProperties.TilesOrder.rotate:

                        character.InitMovement(newTile);
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                        tileColoredDuringPattern.Add(newTile);
                        StartCoroutine(ExtraRotationReview(pattern, character, index, depth));
                        break;
                    case TileProperties.TilesOrder.attack:
                        character.InitMovement(newTile);
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                        tileColoredDuringPattern.Add(newTile);
                        StartCoroutine(ExtraAttack(pattern, character, index, depth, true));
                        break;
                    case TileProperties.TilesOrder.stop:
                        character.InitMovement(newTile);
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                        tileColoredDuringPattern.Add(newTile);
                        StopPattern(character);
                        break;
                    default:
                        break;
                }
                break;

            case TileProperties.TilesSpecific.Push:
                if (newTile.isActivated)
                {
                    newTile.ChangeTilesActivationStatut(false);
                    character.InitMovement(newTile);
                    TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.interactionMat);
                    StartCoroutine(ExtraDeplacementReview(pattern, character, index, depth));
                }
                else
                {
                    MovementOnTile(pattern, character, index, depth, newTile);
                }
                break;

            case TileProperties.TilesSpecific.Door:
                if (newTile.isActivated)
                {
                    newTile.ChangeTilesActivationStatut(false);
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    tileColoredDuringPattern.Add(newTile);
                    StopPattern(character);
                }
                else
                {
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    MovementOnTile(pattern, character, index, depth, newTile);
                }

                break;
            case TileProperties.TilesSpecific.Wall:

                StartCoroutine(ExtraAttack(pattern, character, index, depth, false));

                break;
            case TileProperties.TilesSpecific.Teleport:
                character.InitMovement(newTile);
                TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                tileColoredDuringPattern.Add(newTile);
                StartCoroutine(Teleportation(pattern, character, index, depth));
                break;


            case TileProperties.TilesSpecific.Trap:
                character.InitMovement(newTile);
                //faudra gérer le cas de la zone enflammé
                StartCoroutine(GetDamaged(pattern, character, index, depth, true, newTile.damageToDeal));
                break;

            default:
                CharacterReorientation(character , false);
                break;
        }
    }

    private void CharacterReorientation(Character character, bool continuePattern)
    {
        if (character.isAlly)
        {
            character.transform.Rotate(Vector3.up, 180f);
        }
        else
        {
            Vector3 nexusDirection = Vector3.right;
            Quaternion rotation = Quaternion.Euler(0f, GetRotationOffset(character.transform.forward, nexusDirection), 0f);
            character.transform.forward = rotation * character.transform.forward;
        }

        if (!continuePattern)
        {
            StopPattern(character);
        }
    }


    #region SpecificAction
    private void MovementOnTile(PatternTemplate pattern, Character character, int index, int depth, TileProperties newTile)
    {
        character.InitMovement(newTile);

        if (newTile.isOnFire)
        {
            StartCoroutine(GetDamaged(pattern, character, index, depth, true, newTile.damageToDeal));
            print("OnFIre");
            return;
        }
        TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.mouvementMat);
        ActionEnd(pattern, character.occupiedTile, character, index, depth);
    }

    private IEnumerator GetDamaged(PatternTemplate pattern, Character character, int index, int depth, bool continuePattern, int damagedDeal)
    {
        TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.mouvementMat);
        tileColoredDuringPattern.Add(character.occupiedTile);
        yield return new WaitForSeconds(0.5f);
        if (continuePattern && character.TakeDamaged(damagedDeal))
        {
            ActionEnd(pattern, character.occupiedTile, character, index, depth);
        }
        else
        {
            StopPattern(character);
        }
    }

    private IEnumerator Teleportation(PatternTemplate pattern, Character character, int index, int depth)
    {
        yield return new WaitForSeconds(0.5f);

        TileProperties teleportExit = character.occupiedTile.GetTeleportExit();
        if (teleportExit == null && !teleportExit.isOccupied)
        {
            StopPattern(character);
        }
        else
        {
            TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.interactionMat);
            tileColoredDuringPattern.Add(character.occupiedTile);
            character.InitMovement(teleportExit);
            TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.interactionMat);
            character.transform.Rotate(Vector3.up, teleportExit.GetRotationOffset(character.transform.forward));
            ActionEnd(pattern, character.occupiedTile, character, index, depth);
        }
    }

    private IEnumerator ExtraAttack(PatternTemplate pattern, Character character, int index, int depth, bool continuePatern)
    {
        yield return new WaitForSeconds(0.5f);

        int rayLength = 2;
        List<TileProperties> tiles = character.occupiedTile.GetTileOnDirection(character.transform.forward, rayLength, false);
        if (tiles.Count == 0)
        {
            ActionEnd(pattern, character.occupiedTile, character, index, depth);
        }
        TileProperties testedTile = tiles[0];
        TilesManager.Instance.ChangeTileMaterial(testedTile, PatternReader.instance.attackMat);
        tileColoredDuringPattern.Add(testedTile);

        if (testedTile.specificity == TileProperties.TilesSpecific.Wall)
        {
            testedTile.GetDamaged(character.damage);
        }
        else
        {
            if (testedTile.occupant != null)
            {

                testedTile.occupant.GotAttacked(character.damage);
            }
        }

        if (continuePatern)
        {
            ActionEnd(pattern, testedTile, character, index, depth);
        }
        else
        {
            StopPattern(character);
        }
    }

    private IEnumerator ExtraDeplacementReview(PatternTemplate pattern, Character character, int index, int depth)
    {
        yield return new WaitForSeconds(0.5f);

        int rayLength = 2;
        List<TileProperties> tiles = character.occupiedTile.GetTileOnDirection(character.occupiedTile.GetCurrentForward(), rayLength, false);
        if (tiles.Count == 0)
        {
            CharacterReorientation(character , true);
            StartCoroutine(GetDamaged(pattern, character, index, depth, false, 1));
        }
        else
        {
            TileProperties testedTile = tiles[0];
            TileCheck(pattern, character, index, depth, testedTile);
        }
    }

    private IEnumerator ExtraRotationReview(PatternTemplate pattern, Character character, int index, int depth)
    {
        yield return new WaitForSeconds(0.5f);
        character.transform.Rotate(Vector3.up, character.occupiedTile.GetRotationOffset(character.transform.forward));
        TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.interactionMat);
        ActionEnd(pattern, character.occupiedTile, character, index, depth);
    }

    #endregion

    #region Utility

    private void ActionEnd(PatternTemplate pattern, TileProperties tileToColored, Character character, int index, int depth)
    {
        index++;
        if (index < depth)
        {
            StartCoroutine(NextAction(pattern.actions[index].actionDuration, character, pattern, index, depth));
        }
        else
        {
            tileColoredDuringPattern.Add(tileToColored);
            StopPattern(character);
        }
    }

    private IEnumerator NextAction(float duration, Character character, PatternTemplate pattern, int index, int depth)
    {
        yield return new WaitForSeconds(duration);
        ExecuteAction(character, pattern, index, depth);
    }

    private void StopPattern(Character character)
    {
        for (int i = 0; i < tileColoredDuringPattern.Count; i++)
        {
            TilesManager.Instance.ChangeTileMaterial(tileColoredDuringPattern[i], tileColoredDuringPattern[i].baseMat);
        }

        if (character.myState == CharacterState.Standby)
        {
            character.myState = CharacterState.Finished;
        }
        PatternReader.instance.FinishTurn();
    }


    public float GetRotationOffset(Vector3 directionToTest, Vector3 nexusDirection)
    {

        float dot = Vector3.Dot(directionToTest, nexusDirection);
        if (dot * dot >= 0.1f)
        {
            if (dot >= 0f)
            {
                return 0;
            }
            return 180;
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);
            directionToTest = rotation * directionToTest;
            float dotPerpendiculaire = Vector3.Dot(directionToTest, nexusDirection);
            if (dotPerpendiculaire >= 0f)
            {
                return 90;
            }
            return -90;
        }
    }


    #endregion
}

//switch (pattern.actions[index].actionType)
//{
//    case ActionType.Movement:
//        TileProperties frontTile = characterTile.GetTileOnDirection(character.transform.forward, 2, false)[0];
//        if (frontTile != null)
//        {
//            if (frontTile.isOccupied)
//            {
//                Debug.Log("Attack");
//                character.InitAttack();
//                PatternReader.instance.FinishTurn();
//            }
//        }
//        break;

//    case ActionType.Rotation:
//        float rotationAmount;
//        switch (pattern.actions[index].rotation)
//        {
//            case Rotation.Left:
//                rotationAmount = -90;
//                break;
//            case Rotation.Rigth:
//                rotationAmount = 90;
//                break;
//            case Rotation.Reverse:
//                rotationAmount = 180;
//                break;
//            default:
//                rotationAmount = 0;
//                break;
//        }
//        character.transform.Rotate(character.transform.up, rotationAmount);

//        break;

//    case ActionType.Attack:
//        Debug.Log("Attack");
//        break;

//    default:
//        break;
//}

//index++;
//if (index < depth)
//{
//    StartCoroutine(NextAction(pattern.actions[index].actionDuration, character, pattern, index, depth));
//}
//else
//{
//    character.myState = CharacterState.Finished;
//    PatternReader.instance.FinishTurn();
//}