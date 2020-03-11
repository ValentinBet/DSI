using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternExecuter : MonoBehaviour
{

    public List<TileProperties> tileColoredDuringPattern = new List<TileProperties>();
    public Character currentCharacter;

    public void ReadPattern(Character character)
    {
        int depth = character.mouvementPattern.actions.Length;
        currentCharacter = character;
        ExecuteAction(character, character.mouvementPattern, 0, depth);
    }

    private void ExecuteAction(Character character, PatternTemplate pattern, int index, int depth)
    {
        if (character.myState != CharacterState.Standby)
        {
            return;
        }

        tileColoredDuringPattern.Add(character.occupiedTile);
        TileProperties characterTile = character.occupiedTile;

        int rayLength = 2;
        List<TileProperties> tiles = characterTile.GetTileOnDirection(character.transform.forward, rayLength, false);
        if (tiles.Count == 0)
        {
            switch (pattern.actions[index].actionType)
            {
                case ActionType.Movement:


                    CharacterReorientation(character, true, index, depth);

                    TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.rotationMat);
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
                    //Anim d'attack à faire manuellement ici 
                    ActionEnd(pattern, character.occupiedTile, character, index, depth);
                    break;
                default:
                    break;
            }

            return;
        }
        TileProperties testedTile = tiles[0];
        switch (pattern.actions[index].actionType)
        {
            case ActionType.Movement:
                if (testedTile != null)
                {
                    if (testedTile.isOccupied || testedTile.specificity == TileProperties.TilesSpecific.Wall)
                    {
                        if (character.combatStyle == CombatStyle.closeCombat)
                        {
                            StartCoroutine(ExtraAttack(pattern, character, index, depth, false, true));
                        }
                        else
                        {
                            StartCoroutine(ExtraAttack(pattern, character, index, depth, false, false));
                        }
                        return;
                    }

                    TileCheck(pattern, character, index, depth, testedTile, false);
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
                StartCoroutine(ExtraAttack(pattern, character, index, depth, true, true));
                break;

            default:
                break;

        }
    }

    private void TileCheck(PatternTemplate pattern, Character character, int index, int depth, TileProperties newTile, bool bonusAction)
    {
        if (character.myState != CharacterState.Standby)
        {
            return;
        }


        if (newTile.isOccupied)
        {
            if (bonusAction)
            {
                newTile.occupant.GotAttacked(1, character , "attacker pushed");
                TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.attackMat);
                tileColoredDuringPattern.Add(newTile);
                StartCoroutine(GetDamaged(pattern, character, index, depth, false, 1));
            }
            else
            {
                if (character.combatStyle == CombatStyle.closeCombat)
                {
                    StartCoroutine(ExtraAttack(pattern, character, index, depth, false, true));
                }
                else
                {
                    StartCoroutine(ExtraAttack(pattern, character, index, depth, false, false));
                }
            }
        }
        else
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

                            TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                            tileColoredDuringPattern.Add(newTile);
                            character.InitMovement(newTile);
                            StartCoroutine(ExtraRotation(pattern, character, index, depth));
                            break;
                        case TileProperties.TilesOrder.attack:
                            TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                            tileColoredDuringPattern.Add(newTile);
                            character.InitMovement(newTile);
                            StartCoroutine(ExtraAttack(pattern, character, index, depth, true, true));
                            break;
                        case TileProperties.TilesOrder.stop:
                            TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                            tileColoredDuringPattern.Add(newTile);
                            character.InitMovement(newTile);
                            StartCoroutine(StopPattern(character));
                            break;
                        default:
                            break;
                    }
                    break;

                case TileProperties.TilesSpecific.Push:
                    if (newTile.isActivated)
                    {
                        newTile.ChangeTilesActivationStatut(false);
                        tileColoredDuringPattern.Add(newTile);
                        character.InitMovement(newTile);
                        TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.interactionMat);
                        StartCoroutine(ExtraDeplacement(pattern, character, index, depth));
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
                        StartCoroutine(StopPattern(character));
                    }
                    else
                    {
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                        MovementOnTile(pattern, character, index, depth, newTile);
                    }
                    break;

                case TileProperties.TilesSpecific.Wall:
                    if (bonusAction)
                    {
                        Debug.Log("Smashed contre un mur");
                        newTile.GetDamaged(1);
                        StartCoroutine(GetDamaged(pattern, character, index, depth, false, 1));
                    }
                    else
                    {
                        if (character.combatStyle == CombatStyle.closeCombat)
                        {
                            StartCoroutine(ExtraAttack(pattern, character, index, depth, false, true));
                        }
                        else
                        {
                            StartCoroutine(ExtraAttack(pattern, character, index, depth, false, false));
                        }

                    }


                    break;
                case TileProperties.TilesSpecific.Teleport:
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    tileColoredDuringPattern.Add(newTile);
                    character.InitMovement(newTile);
                    StartCoroutine(Teleportation(pattern, character, index, depth));
                    break;

                case TileProperties.TilesSpecific.Trap:
                    if (newTile.isActivated)
                    {
                        newTile.ChangeTilesActivationStatut(false);
                        StartCoroutine(GetDamaged(pattern, character, index, depth, true, newTile.damageToDeal));
                        character.InitMovement(newTile);
                    }
                    else
                    {
                        MovementOnTile(pattern, character, index, depth, newTile);
                    }
                    break;

                case TileProperties.TilesSpecific.PlayerBase:
                    if (character.isAlly)
                    {
                        if (bonusAction)
                        {

                            CharacterReorientation(character, false, index, depth);
                            StartCoroutine(GetDamaged(pattern, character, index, depth, false, 1));
                        }
                        else
                        {
                            CharacterReorientation(character, true, index, depth);
                        }
                    }
                    else
                    {
                        //Anim à faire
                        Vector3 newPos = character.transform.position + character.transform.forward * 2;
                        character.transform.position = newPos;
                        PlayerBase.Instance.DamageBase(1);
                        Debug.Log(PlayerBase.Instance.GetLife());
                        StartCoroutine(GetDamaged(pattern, character, index, depth, false, 100));
                    }
                    break;


                default:
                    if (bonusAction)
                    {
                        CharacterReorientation(character, true, index, depth);
                        //StartCoroutine(StopPattern(character));
                    }
                    else
                    {
                        CharacterReorientation(character, true, index, depth);

                    }
                    break;
            }
        }

    }

    private void CharacterReorientation(Character character, bool doNextAction, int index, int depth)
    {
        if (character.myState != CharacterState.Standby)
        {
            return;
        }

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

        TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.interactionMat);
        tileColoredDuringPattern.Add(character.occupiedTile);

        if (doNextAction)
        {
            ActionEnd(character.mouvementPattern, character.occupiedTile, character, index, depth);
        }
    }

    #region SpecificAction
    private void MovementOnTile(PatternTemplate pattern, Character character, int index, int depth, TileProperties newTile)
    {
        TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.mouvementMat);
        tileColoredDuringPattern.Add(character.occupiedTile);
        character.InitMovement(newTile);
        ActionEnd(pattern, character.occupiedTile, character, index, depth);
    }

    private IEnumerator GetDamaged(PatternTemplate pattern, Character character, int index, int depth, bool continuePattern, int receivedDeal)
    {
        TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.mouvementMat);
        tileColoredDuringPattern.Add(character.occupiedTile);
        yield return new WaitForSeconds(0.0f);
        if (character.TakeDamaged(receivedDeal, false) && continuePattern)
        {
            ActionEnd(pattern, character.occupiedTile, character, index, depth);
        }
        else
        {
            Debug.Log("Pattern finsh , get dmaged");
            StartCoroutine(StopPattern(character));
        }
    }

    private IEnumerator Teleportation(PatternTemplate pattern, Character character, int index, int depth)
    {
        yield return new WaitForSeconds(0.5f);

        TileProperties teleportExit = character.occupiedTile.GetTeleportExit();
        if (teleportExit == null || teleportExit.isOccupied)
        {
            ActionEnd(pattern, character.occupiedTile, character, index, depth);
        }
        else
        {
            TilesManager.Instance.ChangeTileMaterial(teleportExit, PatternReader.instance.interactionMat);
            tileColoredDuringPattern.Add(teleportExit);
            character.InitMovement(teleportExit);
            character.transform.Rotate(Vector3.up, teleportExit.GetRotationOffset(character.transform.forward));
            ActionEnd(pattern, character.occupiedTile, character, index, depth);
        }
    }

    private IEnumerator ExtraAttack(PatternTemplate pattern, Character character, int index, int depth, bool continuePatern, bool useCharacterPattern)
    {
        yield return new WaitForSeconds(0.5f);
        List<TileProperties> testedTiles = new List<TileProperties>();

        if (!useCharacterPattern)
        {
            int rayLength = 2;
            List<TileProperties> tiles = character.occupiedTile.GetTileOnDirection(character.transform.forward, rayLength, false);
            if (tiles.Count != 0)
            {
                //ActionEnd(pattern, character.occupiedTile, character, index, depth);
                AttackOnTargetTile(character, testedTiles, tiles[0], 0.5f);
            }
        }

        else
        {
            if (character.AttackPattern.attackType == AttackType.Zone)
            {
                for (int i = 0; i < character.AttackPattern.tilesAffected.Length; i++)
                {
                    TileProperties tileTarget = character.GetTileFromTransform(character.AttackPattern.tilesAffected[i].tilesTargetOffset, 2);
                    if (tileTarget != null)
                    {
                        AttackOnTargetTile(character, testedTiles, tileTarget, character.AttackPattern.tilesAffected[i].impactValue);
                    }
                }
            }
            else
            {
                for (int i = 0; i < character.AttackPattern.tilesAffected.Length; i++)
                {
                    TileProperties tileTarget = character.GetTileFromTransform(character.AttackPattern.tilesAffected[i].tilesTargetOffset, 2);
                    if (tileTarget != null)
                    {
                        Vector3 spawnPos = tileTarget.transform.position + new Vector3(0, 0.5f, 0);
                        GameObject newProj = Instantiate(character.AttackPattern.tilesAffected[i].projectilePrefab, spawnPos, character.transform.rotation);
                        ProjectileBeheviour proj = newProj.GetComponent<ProjectileBeheviour>();
                        proj.Init(character, index, depth, continuePatern);
                    }
                }
            }
        }

        if (character.AttackPattern.attackType == AttackType.Zone || !useCharacterPattern)
        {
            if (continuePatern)
            {
                ActionEnd(pattern, testedTiles, character, index, depth, continuePatern);
            }
            else
            {
                StartCoroutine(StopPattern(character));
            }
        }
    }

    private IEnumerator ExtraDeplacement(PatternTemplate pattern, Character character, int index, int depth)
    {
        yield return new WaitForSeconds(0.5f);

        int rayLength = 2;
        List<TileProperties> tiles = character.occupiedTile.GetTileOnDirection(character.occupiedTile.GetCurrentForward(), rayLength, false);
        if (tiles.Count == 0)
        {
            CharacterReorientation(character, false, index, depth);
            StartCoroutine(GetDamaged(pattern, character, index, depth, false, 1));
        }
        else
        {
            TileProperties testedTile = tiles[0];
            TileCheck(pattern, character, index, depth, testedTile, true);
        }
    }

    private IEnumerator ExtraRotation(PatternTemplate pattern, Character character, int index, int depth)
    {
        yield return new WaitForSeconds(0.5f);
        character.transform.Rotate(Vector3.up, character.occupiedTile.GetRotationOffset(character.transform.forward));
        TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.interactionMat);
        ActionEnd(pattern, character.occupiedTile, character, index, depth);
    }

    #endregion

    #region Utility

    private void AttackOnTargetTile(Character character, List<TileProperties> testedTiles, TileProperties targetTile, float impactValue)
    {
        testedTiles.Add(targetTile);
        TilesManager.Instance.ChangeTileMaterial(targetTile, PatternReader.instance.attackMat);
        tileColoredDuringPattern.Add(targetTile);
        targetTile.tileImpact.ActivateImpact(impactValue);

        if (targetTile.specificity == TileProperties.TilesSpecific.Wall)
        {
            targetTile.GetDamaged(character.damage);
        }
        else
        {
            if (targetTile.occupant != null)
            {
                targetTile.occupant.GotAttacked(character.damage, character , "attack on target");
            }
        }
    }

    private void ActionEnd(PatternTemplate pattern, TileProperties tileToColored, Character character, int index, int depth)
    {
        index++;
        if (index < depth)
        {
            StartCoroutine(NextAction(pattern.actions[index].actionDuration, character, pattern, index, depth));
        }
        else
        {
            Debug.Log("no more action");
            tileColoredDuringPattern.Add(tileToColored);
            StartCoroutine(StopPattern(character));
        }
    }

    public void ActionEnd(PatternTemplate pattern, List<TileProperties> tilesToColored, Character character, int index, int depth, bool continuePattern)
    {
        index++;
        for (int i = 0; i < tilesToColored.Count; i++)
        {
            tileColoredDuringPattern.Add(tilesToColored[i]);
        }

        if (index < depth && continuePattern)
        {
            StartCoroutine(NextAction(pattern.actions[index].actionDuration, character, pattern, index, depth));
        }
        else
        {
            StartCoroutine(StopPattern(character));
        }
    }

    private IEnumerator NextAction(float duration, Character character, PatternTemplate pattern, int index, int depth)
    {
        yield return new WaitForSeconds(duration);
        ExecuteAction(character, pattern, index, depth);
    }

    public IEnumerator StopPattern(Character character)
    {
        tileColoredDuringPattern.Add(character.occupiedTile);
        yield return new WaitForSeconds(0.2f);

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