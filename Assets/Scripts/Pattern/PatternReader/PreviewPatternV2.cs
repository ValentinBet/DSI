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

    private Vector3 currentDirection;
    private int currentLife;

    public void ReadPattern(Character character)
    {
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
        tileColoredDuringPattern.Add(currentTile);

        int rayLength = 2;


        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentDirection, rayLength, false);
        if (tiles.Count == 0)
        {
            switch (currentCharacter.mouvementPattern.actions[index].actionType)
            {
                case ActionType.Movement:

                    CharacterReorientation(true, index, depth);
                    TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.rotationMat);
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
                    ActionEnd(currentTile, index, depth);
                    break;
                case ActionType.Attack:
                    //Anim d'attack à faire manuellement ici 
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
                    if (testedTile.isOccupied)
                    {
                        if (currentCharacter.combatStyle == CombatStyle.closeCombat)
                        {
                            StartCoroutine(ExtraAttack(index, depth, false, true));
                        }
                        else
                        {
                            StartCoroutine(ExtraAttack(index, depth, false, false));
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
                ActionEnd(currentTile, index, depth);
                break;

            case ActionType.Attack:
                StartCoroutine(ExtraAttack(index, depth, true, true));
                break;

            default:
                break;

        }
    }

    private void TileCheck(int index, int depth, TileProperties newTile, bool bonusAction)
    {


        if (bonusAction && newTile.isOccupied)
        {

            TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.attackMat);
            tileColoredDuringPattern.Add(newTile);
            StartCoroutine(GetDamaged(index, depth, false, 1));
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

                            TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                            tileColoredDuringPattern.Add(newTile);
                            currentTile = newTile;
                            StartCoroutine(ExtraRotation(index, depth));
                            break;
                        case TileProperties.TilesOrder.attack:
                            TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
                            tileColoredDuringPattern.Add(newTile);
                            currentTile = newTile;
                            StartCoroutine(ExtraAttack(index, depth, true, true));
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
                        newTile.ChangeTilesActivationStatut(false);
                        tileColoredDuringPattern.Add(newTile);
                        currentTile = newTile;
                        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                        StartCoroutine(ExtraDeplacement(index, depth));
                    }
                    else
                    {
                        MovementOnTile(index, depth, newTile);
                    }
                    break;

                case TileProperties.TilesSpecific.Door:
                    if (newTile.isActivated)
                    {
                        newTile.ChangeTilesActivationStatut(false);

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
                        newTile.GetDamaged(1);
                        StartCoroutine(GetDamaged(index, depth, false, 1));
                    }
                    else
                    {

                        //if (!character.isAlly)
                        //{
                        //    CharacterReorientation(character, false, index, depth);
                        //}

                        if (currentCharacter.combatStyle == CombatStyle.closeCombat)
                        {
                            StartCoroutine(ExtraAttack(index, depth, false, true));
                        }
                        else
                        {
                            StartCoroutine(ExtraAttack(index, depth, false, false));
                        }

                    }


                    break;
                case TileProperties.TilesSpecific.Teleport:
                    TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.interactionMat);
                    tileColoredDuringPattern.Add(newTile);
                    currentTile = newTile;
                    StartCoroutine(Teleportation(index, depth));
                    break;

                case TileProperties.TilesSpecific.Trap:
                    if (newTile.isActivated)
                    {
                        newTile.ChangeTilesActivationStatut(false);
                        StartCoroutine(GetDamaged( index, depth, true, newTile.damageToDeal));
                        currentTile = newTile;

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

                            CharacterReorientation(false, index, depth);
                            StartCoroutine(GetDamaged( index, depth, false, 1));
                        }
                        else
                        {
                            CharacterReorientation(true, index, depth);
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
                        CharacterReorientation(false, index, depth);
                    }
                    else
                    {
                        CharacterReorientation(true, index, depth);

                    }
                    break;
            }
        }

    }

    private void CharacterReorientation(bool doNextAction, int index, int depth)
    {
        Quaternion rotation;
        if (currentCharacter.isAlly)
        {
            rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            Vector3 nexusDirection = Vector3.right;
            rotation = Quaternion.Euler(0f, GetRotationOffset(currentDirection, nexusDirection), 0f);
        }
        currentDirection = rotation * currentDirection;
        if (doNextAction)
        {
            ActionEnd(currentTile, index, depth);
        }
    }

    #region SpecificAction
    private void MovementOnTile(int index, int depth, TileProperties newTile)
    {
        TilesManager.Instance.ChangeTileMaterial(newTile, PatternReader.instance.mouvementMat);
        tileColoredDuringPattern.Add(newTile);
        currentTile = newTile;
        ActionEnd(newTile, index, depth);
    }

    private IEnumerator GetDamaged(int index, int depth, bool continuePattern, int receivedDeal)
    {
        TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.attackMat);
        tileColoredDuringPattern.Add(currentTile);

        currentLife -= receivedDeal;

        yield return new WaitForSeconds(0);
        if (continuePattern && currentLife > 0)
        {
            ActionEnd(currentTile, index, depth);
        }
        else
        {
            Debug.Log("Pattern finsh , get damaged");
        }
    }

    private IEnumerator Teleportation(int index, int depth)
    {
        yield return new WaitForSeconds(0.5f);

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

    }

    private IEnumerator ExtraAttack(int index, int depth, bool continuePatern, bool useCharacterPattern)
    {
        yield return new WaitForSeconds(0);
        List<TileProperties> testedTiles = new List<TileProperties>();

        if (!useCharacterPattern)
        {
            int rayLength = 2;
            List<TileProperties> tiles = currentTile.GetTileOnDirection(currentDirection, rayLength, false);
            if (tiles.Count != 0)
            {
                //ActionEnd(pattern, character.occupiedTile, character, index, depth);
                AttackOnTargetTile(testedTiles, tiles[0]);
            }
        }

        else
        {

            for (int i = 0; i < currentAttackTemplate.tilesAffected.Length; i++)
            {
                TileProperties tileTarget = GetTileFromPattern(currentAttackTemplate.tilesAffected[i].tilesTargetOffset, 2);
                if (tileTarget != null)
                {
                    AttackOnTargetTile(testedTiles, tileTarget);
                }
            }
        }

        if (continuePatern)
        {
            ActionEnd(testedTiles, index, depth, continuePatern);
        }

    }


    private IEnumerator ExtraDeplacement(int index, int depth)
    {
        yield return new WaitForSeconds(0);

        int rayLength = 2;
        List<TileProperties> tiles = currentTile.GetTileOnDirection(currentDirection, rayLength, false);
        if (tiles.Count == 0)
        {
            CharacterReorientation(false, index, depth);
            StartCoroutine(GetDamaged(index, depth, false, 1));
        }
        else
        {
            TileProperties testedTile = tiles[0];
            TileCheck(index, depth, testedTile, true);
        }
    }

    private IEnumerator ExtraRotation(int index, int depth)
    {
        yield return new WaitForSeconds(0);
        Quaternion rotation = Quaternion.Euler(0f, currentTile.GetRotationOffset(currentDirection), 0f);

        currentDirection = rotation * currentDirection;

        TilesManager.Instance.ChangeTileMaterial(currentTile, PatternReader.instance.interactionMat);
        ActionEnd(currentTile, index, depth);
    }

    #endregion

    #region Utility

    private void AttackOnTargetTile(List<TileProperties> testedTiles, TileProperties targetTile)
    {
        testedTiles.Add(targetTile);
        TilesManager.Instance.ChangeTileMaterial(targetTile, PatternReader.instance.attackMat);
        tileColoredDuringPattern.Add(targetTile);
    }

    private void ActionEnd(TileProperties tileToColored, int index, int depth)
    {
        index++;
        if (index < depth)
        {
            StartCoroutine(NextAction(0, index, depth));
        }
        else
        {
            tileColoredDuringPattern.Add(tileToColored);
        }
    }

    public void ActionEnd(List<TileProperties> tilesToColored, int index, int depth, bool continuePattern)
    {
        index++;
        for (int i = 0; i < tilesToColored.Count; i++)
        {
            tileColoredDuringPattern.Add(tilesToColored[i]);
        }

        if (index < depth && continuePattern)
        {
            Debug.Log("NextAction");
            StartCoroutine(NextAction(currentMouvmentTemplate.actions[index].actionDuration,  index, depth));
        }
    }

    private IEnumerator NextAction(float duration, int index, int depth)
    {
        yield return new WaitForSeconds(duration);
        ExecuteAction( index, depth);
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

        Vector3 targetPos = currentTile.transform.position + (currentRigth * (tileOffset.x * tilesSize)) + (currentDirection * (tileOffset.y * tilesSize));


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
        Debug.DrawRay(targetPos, Vector3.down, Color.red, 2);

        return null;
    }

    public void EndPreview()
    {
        for (int i = 0; i < tileColoredDuringPattern.Count; i++)
        {
            TilesManager.Instance.ChangeTileMaterial(tileColoredDuringPattern[i], tileColoredDuringPattern[i].baseMat);
        }
    }

    #endregion
}
