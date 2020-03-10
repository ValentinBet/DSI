using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPatternV2 : MonoBehaviour
{
    public List<TileProperties> tileColoredDuringPattern = new List<TileProperties>();
    public Character currentCharacter;

    private TileProperties currentTile;
    private PatternTemplate currentMouvmentTemplate;
    private AttackTemplate currentAttackTemplate;
    public float timeBetweenAction = 0.2f;

    private Vector3 currentDirection;
    private int currentLife;

    private bool previewEnd;


    /// <summary>
    /// Affiche la preview du pattern d'un personnage
    /// </summary>
    /// <param name="character"></param>
    public void ReadPattern(Character character)
    {
        previewEnd = false;
        int depth = character.mouvementPattern.actions.Length;
        currentCharacter = character;
        currentTile = currentCharacter.occupiedTile;
        currentDirection = currentCharacter.transform.forward;
        currentMouvmentTemplate = currentCharacter.mouvementPattern;
        currentAttackTemplate = currentCharacter.AttackPattern;
        currentLife = currentCharacter.life;
        ExecuteAction(0, depth);
    }

    private void ExecuteAction(int index, int depth)
    {
        Debug.Log("", currentTile);
        if (previewEnd)
        {
            EndPreview();
            return;
        }
        tileColoredDuringPattern.Add(currentTile);

        int rayLength = 2;
        //Cas ou il n'y a pas de case devant le personnage
        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentDirection, rayLength, false);
        if (tiles.Count == 0)
        {
            switch (currentCharacter.mouvementPattern.actions[index].actionType)
            {
                case ActionType.Movement:

                    PreviewReorientation(true, index, depth);
                    TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.rotationMat);
                    tileColoredDuringPattern.Add(currentTile);
                    //ActionEnd(currentTile, index, depth);
                    break;

                case ActionType.Rotation:
                    Quaternion rotation;
                    switch (currentMouvmentTemplate.actions[index].rotation)
                    {
                        case Rotation.Left:
                            rotation = Quaternion.Euler(0f, -90f, 0f);
                            break;
                        case Rotation.Rigth:
                            rotation = Quaternion.Euler(0f, 90f, 0f);
                            break;
                        case Rotation.Reverse:
                            rotation = Quaternion.Euler(0f, 180f, 0f);
                            break;
                        default:
                            rotation = Quaternion.Euler(0f, 0, 0f);
                            break;
                    }
                    currentDirection = rotation * currentDirection;
                    TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.rotationMat);
                    tileColoredDuringPattern.Add(currentTile);
                    ActionEnd(currentTile, index, depth);
                    break;
                case ActionType.Attack:
                    TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.attackMat);
                    tileColoredDuringPattern.Add(currentTile);
                    ActionEnd(currentTile, index, depth);
                    break;
                default:
                    break;
            }
            return;
        }
        TileProperties testedTile = tiles[0];
        switch (currentMouvmentTemplate.actions[index].actionType)
        {
            case ActionType.Movement:
                if (testedTile != null)
                {
                    if (testedTile.isOccupied && testedTile.occupant != currentCharacter)
                    {
                        if (currentCharacter.combatStyle == CombatStyle.closeCombat)
                        {
                            ExtraAttack(index, depth, false, true);
                        }
                        else
                        {
                            ExtraAttack(index, depth, false, false);
                        }
                        return;
                    }

                    TileCheck(index, depth, testedTile, false);
                }
                break;

            case ActionType.Rotation:
                Quaternion rotation;
                switch (currentMouvmentTemplate.actions[index].rotation)
                {
                    case Rotation.Left:
                        rotation = Quaternion.Euler(0f, -90f, 0f);
                        break;
                    case Rotation.Rigth:
                        rotation = Quaternion.Euler(0f, 90f, 0f);
                        break;
                    case Rotation.Reverse:
                        rotation = Quaternion.Euler(0f, 180f, 0f);
                        break;
                    default:
                        rotation = Quaternion.Euler(0f, 0, 0f);
                        break;
                }
                currentDirection = rotation * currentDirection;
                TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.rotationMat);
                tileColoredDuringPattern.Add(currentTile);
                ActionEnd(currentTile, index, depth);
                break;

            case ActionType.Attack:
                ExtraAttack(index, depth, true, true);
                break;

            default:
                break;

        }
    }

    private void TileCheck(int index, int depth, TileProperties newTile, bool bonusAction)
    {
        if (previewEnd)
        {
            EndPreview();
            return;
        }

        if (bonusAction && newTile.isOccupied)
        {
            if (newTile.occupant != currentCharacter)
            {
                TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.attackMat);
                tileColoredDuringPattern.Add(newTile);
                GetDamaged(index, depth, false, 1);
                return;
            }
        }
        else
        {

            switch (newTile.specificity)
            {
                case TileProperties.TilesSpecific.None:
                    MovementOnTile(index, depth, newTile);
                    break;

                case TileProperties.TilesSpecific.Ordre:
                    switch (newTile.order)
                    {
                        case TileProperties.TilesOrder.rotate:

                            //TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                            //tileColoredDuringPattern.Add(newTile);
                            currentTile = newTile;
                            ExtraRotation(index, depth);
                            break;
                        case TileProperties.TilesOrder.attack:
                            TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                            tileColoredDuringPattern.Add(newTile);
                            currentTile = newTile;
                            ExtraAttack(index, depth, true, true);
                            break;
                        case TileProperties.TilesOrder.stop:
                            TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                            tileColoredDuringPattern.Add(newTile);
                            currentTile = newTile;
                            break;
                        default:
                            break;
                    }
                    break;

                case TileProperties.TilesSpecific.Push:
                    if (newTile.isActivated)
                    {
                        tileColoredDuringPattern.Add(newTile);
                        currentTile = newTile;
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                        ExtraDeplacement(index, depth);
                    }
                    else
                    {
                        MovementOnTile(index, depth, newTile);
                    }
                    break;

                case TileProperties.TilesSpecific.Door:
                    if (newTile.isActivated)
                    {
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                        tileColoredDuringPattern.Add(newTile);
                    }
                    else
                    {
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                        MovementOnTile(index, depth, newTile);
                    }
                    break;

                case TileProperties.TilesSpecific.Wall:
                    if (bonusAction)
                    {
                        GetDamaged(index, depth, false, 1);
                    }
                    else
                    {
                        if (currentCharacter.combatStyle == CombatStyle.closeCombat)
                        {
                            ExtraAttack(index, depth, false, true);
                        }
                        else
                        {
                            ExtraAttack(index, depth, false, false);
                        }

                        if (!currentCharacter.isAlly)
                        {
                            PreviewReorientation(false, index, depth);
                        }
                    }


                    break;
                case TileProperties.TilesSpecific.Teleport:
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    tileColoredDuringPattern.Add(newTile);
                    currentTile = newTile;
                    Teleportation(index, depth);
                    break;

                case TileProperties.TilesSpecific.Trap:
                    if (newTile.isActivated)
                    {
                        currentTile = newTile;
                        GetDamaged(index, depth, true, newTile.damageToDeal);

                        //TilesManager.Instance.ChangeTileMaterial(character.occupiedTile, PatternReader.instance.interactionMat);
                        //tileColoredDuringPattern.Add(character.occupiedTile);
                    }
                    else
                    {
                        MovementOnTile(index, depth, newTile);
                    }
                    break;

                case TileProperties.TilesSpecific.PlayerBase:
                    if (currentCharacter.isAlly)
                    {
                        if (bonusAction)
                        {
                            PreviewReorientation(false, index, depth);
                            GetDamaged(index, depth, false, 1);
                        }
                        else
                        {
                            PreviewReorientation(true, index, depth);
                        }
                    }
                    //else
                    //{
                    //    Anim à faire
                    //    Vector3 newPos = character.transform.position + character.transform.forward;
                    //    character.transform.position = newPos;
                    //    PlayerBase.Instance.DamageBase(1);
                    //    Debug.Log(PlayerBase.Instance.GetLife());
                    //    character.KillCharacter(true);
                    //}
                    break;


                default:
                    if (bonusAction)
                    {
                        PreviewReorientation(false, index, depth);
                        // ActionEnd(currentTile, index, depth);
                    }
                    else
                    {
                        PreviewReorientation(true, index, depth);
                        //ActionEnd(currentTile, index, depth);
                    }
                    break;
            }
        }

    }

    private void PreviewReorientation(bool doNextAction, int index, int depth)
    {

        if (currentCharacter.isAlly)
        {
            Quaternion rotation;
            rotation = Quaternion.Euler(0f, 180f, 0f);
            currentDirection = rotation * currentDirection;
        }
        else
        {

            Vector3 nexusDirection = Vector3.right;
            currentDirection = nexusDirection;
        }

        if (doNextAction)
        {
            ActionEnd(currentTile, index, depth);
        }
    }

    #region SpecificAction
    private void MovementOnTile(int index, int depth, TileProperties newTile)
    {
        if (newTile.isOccupied && newTile.occupant != currentCharacter)
        {
            if (currentCharacter.combatStyle == CombatStyle.closeCombat)
            {
                ExtraAttack(index, depth, false, true);
            }
            else
            {
                ExtraAttack(index, depth, false, false);
            }
            return;
        }
        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
        tileColoredDuringPattern.Add(newTile);
        currentTile = newTile;
        ActionEnd(newTile, index, depth);
    }

    private void GetDamaged(int index, int depth, bool continuePattern, int receivedDeal)
    {
        //damage MAt
        TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.receiveDamageMat);
        tileColoredDuringPattern.Add(currentTile);

        currentLife -= receivedDeal;

        if (continuePattern && currentLife > 0)
        {
            ActionEnd(currentTile, index, depth);
        }
        else
        {
            if (currentLife < 1)
            {
                TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.deathMat);
                tileColoredDuringPattern.Add(currentTile);
            }
            Debug.Log("Pattern finsh , get damaged");
        }
    }

    private void Teleportation(int index, int depth)
    {

        TileProperties teleportExit = currentTile.GetTeleportExit();
        if (teleportExit != null && !teleportExit.isOccupied)
        {
            TilesManager.Instance.ChangeTileMaterial(teleportExit, PatternReader.instance.interactionMat);
            tileColoredDuringPattern.Add(teleportExit);
            currentTile = teleportExit;

            Quaternion rotation = Quaternion.Euler(0f, teleportExit.GetRotationOffset(currentDirection), 0f);
            currentDirection = rotation * currentDirection;

            ActionEnd(currentTile, index, depth);
        }
        ActionEnd(currentTile, index, depth);
    }

    private void ExtraAttack(int index, int depth, bool continuePatern, bool useCharacterPattern)
    {

        if (!useCharacterPattern)
        {
            int rayLength = 2;
            List<TileProperties> tiles = currentTile.GetTileOnDirection(currentDirection, rayLength, false);
            if (tiles.Count != 0)
            {
                //ActionEnd(pattern, character.occupiedTile, character, index, depth);
                PreviewOnTargetTile(tiles[0]);
            }
        }
        else
        {

            for (int i = 0; i < currentAttackTemplate.tilesAffected.Length; i++)
            {
                TileProperties tileTarget = GetTileFromPattern(currentAttackTemplate.tilesAffected[i].tilesTargetOffset, 2);
                if (tileTarget != null)
                {
                    PreviewOnTargetTile(tileTarget);
                }
            }
        }

        if (continuePatern)
        {
            ActionEnd(currentTile, index, depth, continuePatern);
        }

    }

    private void ExtraDeplacement(int index, int depth)
    {

        int rayLength = 2;
        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentTile.GetCurrentForward(), rayLength, false);
        if (tiles.Count == 0)
        {
            PreviewReorientation(false, index, depth);
            GetDamaged(index, depth, false, 1);
        }
        else
        {
            TileProperties testedTile = tiles[0];
            TileCheck(index, depth, testedTile, true);
        }
    }

    private void ExtraRotation(int index, int depth)
    {
        Quaternion rotation = Quaternion.Euler(0f, currentTile.GetRotationOffset(currentDirection), 0f);

        currentDirection = rotation * currentDirection;

        TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.interactionMat);
        tileColoredDuringPattern.Add(currentTile);
        ActionEnd(currentTile, index, depth);
    }

    #endregion

    #region Utility

    private void PreviewOnTargetTile(TileProperties targetTile)
    {
        //testedTiles.Add(targetTile);
        TilesManager.Instance.ChangeTileMaterial(targetTile, PatternReader.instance.attackMat);
        tileColoredDuringPattern.Add(targetTile);
    }

    private void ActionEnd(TileProperties tileToColored, int index, int depth)
    {
        index++;
        if (index < depth)
        {
            StartCoroutine(NextAction(timeBetweenAction, index, depth));
        }
        else
        {
            tileColoredDuringPattern.Add(tileToColored);
        }
    }

    public void ActionEnd(TileProperties tilesToColored, int index, int depth, bool continuePattern)
    {
        index++;
  
        if (index < depth && continuePattern)
        {
            Debug.Log("NextAction");
            StartCoroutine(NextAction(timeBetweenAction, index, depth));
        }
        else
        {
            tileColoredDuringPattern.Add(tilesToColored);
        }
    }

    private IEnumerator NextAction(float duration, int index, int depth)
    {
        yield return new WaitForSeconds(duration);
        ExecuteAction(index, depth);
    }

    public void StopPattern(Character character)
    {
        tileColoredDuringPattern.Add(character.occupiedTile);

        for (int i = 0; i < tileColoredDuringPattern.Count; i++)
        {
            TilesManager.Instance.ChangeTileMaterial(tileColoredDuringPattern[i], tileColoredDuringPattern[i].baseMat);
        }

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

    public TileProperties GetTileFromPattern(Vector2 tileOffset, int lenght = 1)
    {
        List<TileProperties> listTilesOnDirection = new List<TileProperties>();

        RaycastHit hitTile;
        float tilesSize = 2;

        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);

        Vector3 currentRigth = rotation * currentDirection;

        Vector3 targetPos = (currentTile.transform.position + (currentRigth * (tileOffset.x * tilesSize)) + (currentDirection * (tileOffset.y * tilesSize))) + new Vector3 (0,1,0);


        //hitTiles = Physics.RaycastAll(transform.position, transform.TransformDirection(direction), lenght, TileLayer);
        Physics.Raycast(targetPos, Vector3.down, out hitTile, lenght, TilesManager.Instance.tileLayer);

        if (hitTile.collider != null)
        {
            if (hitTile.collider.gameObject.GetComponent<TileProperties>() != null)
            {
                return hitTile.collider.gameObject.GetComponent<TileProperties>();
            }
        }
;
        Debug.DrawRay(targetPos, Vector3.down, Color.yellow, 10);

        return null;
    }

    public void EndPreview()
    {
        StopAllCoroutines();
        previewEnd = true;
        for (int i = 0; i < tileColoredDuringPattern.Count; i++)
        {
            TilesManager.Instance.ChangeTileMaterial(tileColoredDuringPattern[i], tileColoredDuringPattern[i].baseMat);
        }
    }

    #endregion
}
