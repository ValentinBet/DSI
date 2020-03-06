using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternExecuter : MonoBehaviour
{
    public void ExecutePattern(PatternTemplate pattern, Character character)
    {
        int depth = pattern.actions.Length;
        ExecuteAction(character, pattern, 0, depth);
    }

    private void ExecuteAction(Character character, PatternTemplate pattern, int index, int depth)
    {
        TileProperties characterTile = character.occupiedTile;



        int rayLength = 2;
        List<TileProperties> tiles = characterTile.GetTileOnDirection(character.transform.forward, rayLength, false);
        if (tiles.Count == 0)
        {
            print("Rien");
            StopPattern(character);
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
                        StartCoroutine(ExtraAttackReview(pattern, character, index, depth, true));
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
                ActionEnd( pattern, character, index, depth);
                break;

            case ActionType.Attack:
                TilesManager.Instance.ChangeTileMaterial(testedTile, PatternReader.instance.attackMat);
                ActionEnd( pattern, character,  index, depth);
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
                        StartCoroutine(ExtraRotationReview(pattern, character, index, depth));
                        break;
                    case TileProperties.TilesOrder.attack:
                        character.InitMovement(newTile);
                        StartCoroutine(ExtraAttackReview(pattern, character, index, depth, true));
                        break;
                    case TileProperties.TilesOrder.stop:
                        character.InitMovement(newTile);
                        StopPattern(character);
                        break;
                    default:
                        break;
                }
                break;

            case TileProperties.TilesSpecific.Push:
                character.InitMovement(newTile);
                StartCoroutine(ExtraDeplacementReview(pattern, character, index, depth));
                break;

            case TileProperties.TilesSpecific.Door:
                if (newTile.isActivated)
                {
                    newTile.ChangeTilesActivationStatut(false);
                    StopPattern(character);
                }
                else
                {
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    MovementOnTile(pattern, character, index, depth, newTile);
                }

                break;
            case TileProperties.TilesSpecific.Wall:

                StartCoroutine(ExtraAttackReview(pattern, character, index, depth, false));

                break;
            case TileProperties.TilesSpecific.Teleport:
                character.InitMovement(newTile);
                StartCoroutine(Teleportation(pattern, character, index, depth));
                break;


            case TileProperties.TilesSpecific.Trap:
                character.InitMovement(newTile);
                //faudra gérer le cas de la zone enflammé
                StartCoroutine(GetDamaged(pattern, character, index, depth, true, newTile.damageToDeal));
                break;

            default:
                StopPattern(character);
                break;
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
        ActionEnd(pattern, character, index, depth);
    }

    private IEnumerator GetDamaged(PatternTemplate pattern, Character character, int index, int depth, bool continuePattern, int damagedDeal)
    {

        yield return new WaitForSeconds(0.5f);
        if (continuePattern && character.TakeDamaged(damagedDeal))
        {
            ActionEnd(pattern, character, index, depth);
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
            character.InitMovement(teleportExit);
            character.transform.Rotate(Vector3.up, teleportExit.GetRotationOffset(character.transform.forward));
            ActionEnd(pattern, character, index, depth);
        }
    }

    private IEnumerator ExtraAttackReview(PatternTemplate pattern, Character character, int index, int depth, bool continuePatern)
    {
        yield return new WaitForSeconds(0.5f);

        int rayLength = 2;
        List<TileProperties> tiles = character.occupiedTile.GetTileOnDirection(character.transform.forward, rayLength, false);
        if (tiles.Count == 0)
        {
            ActionEnd(pattern, character, index, depth);
        }
        TileProperties testedTile = tiles[0];

        if (testedTile.specificity == TileProperties.TilesSpecific.Wall)
        {
            print("Wall Damaged");
        }
        else
        {
            if (testedTile.occupant != null)
            {
                testedTile.occupant.GotAttacked(1);
            }
        }

        if (continuePatern)
        {
            ActionEnd(pattern, character, index, depth);
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
            print("Rien");
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
        ActionEnd(pattern, character, index, depth);
    }

    #endregion


    #region Utility

    private void ActionEnd(PatternTemplate pattern, Character character, int index, int depth)
    {
        index++;
        if (index < depth)
        {
            StartCoroutine(NextAction(pattern.actions[index].actionDuration, character, pattern, index, depth));
        }
        else
        {
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
        if (character.myState == CharacterState.Standby)
        {
            character.myState = CharacterState.Finished;
        }
        PatternReader.instance.FinishTurn();
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