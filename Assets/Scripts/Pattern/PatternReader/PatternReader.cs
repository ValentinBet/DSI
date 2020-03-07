using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternReader : MonoBehaviour
{
    public static PatternReader instance;

    public Material mouvementMat, attackMat, rotationMat, clickMat, interactionMat;

    public PreviewReader PreviewReader;
    public PatternExecuter PatternExecuter;

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

    public void FinishTurn()
    {
        Debug.Log("Tour Fini");
        TilesManager.Instance.ResetTilesStatut();
        //PhaseManager.Instance.NextUnit();
    }

}
