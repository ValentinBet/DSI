using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternReader : MonoBehaviour
{
    public static PatternReader instance;

    public Material mouvementMat, attackMat, rotationMat , clickMat;

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

    public void ReadPattern(PatternTemplate pattern, Character character)
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

            // NextAction(pattern.actions[i].actionDuration);
            // FinishTurn();

        }


    }

    public void PreviewPattern(PatternTemplate pattern, TileProperties tileOrigin, Vector3 direction)
    {
        TileProperties currentTile = tileOrigin;
        int depth = pattern.actions.Length;

        float rotationAmount = 0;


        MakeAction(currentTile, rotationAmount, pattern, 0, depth);

    }

    private void MakeAction(TileProperties currentTile, float rotationAmount, PatternTemplate pattern, int index, int depth)
    {
        Vector3 direction = Vector3.forward;
        direction = Quaternion.AngleAxis(rotationAmount, Vector3.up) * direction;
        switch (pattern.actions[index].actionType)
        {
            case ActionType.Movement:
                int rayLength = 3;
                //Debug.DrawLine(currentTile.transform.position + , currentTile.transform.position + direction*rayLength + new Vector3(0, 2, 0));
                List<TileProperties> tiles = currentTile.GetTileOnDirection(direction, rayLength, false);
                if (tiles.Count == 0)
                {
                    print("Rien");
                    return;
                }
                TileProperties frontTile = tiles[0];
                if (frontTile != null)
                {
                    MeshRenderer _tMR = frontTile.GetComponent<MeshRenderer>();

                    if (frontTile.isWalkable && !frontTile.isOccupied)
                    {
                        if (_tMR != null)
                        {
                            _tMR.sharedMaterial = mouvementMat;
                        }
                        currentTile = frontTile;
                    }
                    if (frontTile.isOccupied)
                    {
                        if (_tMR != null)
                        {
                            _tMR.sharedMaterial = attackMat;
                        }
                        return;
                    }
                    if (frontTile.isAWall)
                    {
                        if (_tMR != null)
                        {
                            _tMR.sharedMaterial = attackMat;
                        }
                        return;
                    }
                }
                else
                {
                    return;
                }
                break;

            case ActionType.Rotation:
                MeshRenderer tMR = currentTile.GetComponent<MeshRenderer>();
                if (tMR != null)
                {
                    tMR.sharedMaterial = rotationMat;
                    Debug.Log("Rotation");
                }

                switch (pattern.actions[index].rotation)
                {
                    case Rotation.Left:
                        rotationAmount = +-90;
                        break;
                    case Rotation.Rigth:
                        rotationAmount = +90;
                        break;
                    case Rotation.Reverse:
                        rotationAmount = +180;
                        break;
                    default:
                        rotationAmount = +0;
                        break;
                }
                //character.transform.Rotate(character.transform.up, rotationAmount);

                break;

            case ActionType.Attack:
                MeshRenderer __tMR = currentTile.GetComponent<MeshRenderer>();
                if (__tMR != null)
                {
                    __tMR.sharedMaterial = attackMat;
                    Debug.Log("Attack");
                }
                break;

            default:
                break;

        }
        index++;
        if (index < depth)
        {
            StartCoroutine(NextAction(pattern.actions[index].actionDuration, currentTile, rotationAmount, pattern, index, depth));
        }
    }

    private void FinishTurn()
    {
        PhaseManager.Instance.NextUnit();
    }

    private IEnumerator NextAction(float duration, TileProperties currentTile, float rotationAmount, PatternTemplate pattern, int index, int depth)
    {
        yield return new WaitForSeconds(duration);
        MakeAction(currentTile, rotationAmount, pattern, index, depth);
    }
}
