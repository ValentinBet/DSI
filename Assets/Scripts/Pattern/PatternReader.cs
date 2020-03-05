using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PatternReader 
{
    public static void ReadPattern(PatternTemplate pattern, Character character)
    {
        TileProperties characterTile = character.occupiedTile;


        for (int i = 0; i < pattern.actions.Length; i++)
        {
            switch (pattern.actions[i].actionType)
            {
                case ActionType.Movement:
                    TileProperties frontTile = characterTile.GetTileOnDirection(character.transform.forward, 1, false)[0];
                    if (frontTile != null)
                    {
                        if (frontTile.isWalkable && !frontTile.isOccupied)
                        {
                            // Faudra une anim qui fasse lerp la pos 
                            character.InitMovement(frontTile);                      
                            //character.occupiedTile.isWalkable = true;
                        }
                        if (frontTile.isOccupied)
                        {
                            Debug.Log("Attack");
                            character.InitAttack();
                            FinishTurn();
                        }
                        if (frontTile.isAWall)
                        {
                            Debug.Log("AttackWall");
                            character.InitAttack();
                            FinishTurn();
                        }


                    }
                    break;

                case ActionType.Rotation:
                    float rotationAmount;
                    switch (pattern.actions[i].rotation)
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

            actionDuration(pattern.actions[i].actionDuration);
            FinishTurn();
          
        }


    }

    private static void FinishTurn()
    {
       PhaseManager.Instance.NextUnit();
    }

    private static IEnumerator actionDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
    }


}
