using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PatternReader : MonoBehaviour
{
    public static PatternReader instance { get { return _instance; } }
    private static PatternReader _instance;

    public Material mouvementMat, attackMat, rotationMat, clickMat, interactionMat , deathMat , receiveDamageMat;

    //public PreviewReader PreviewReader;
    public PreviewPatternV2 PreviewPattern;
    public PatternExecuter PatternExecuter;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void FinishTurn()
    {
        Debug.Log("Tour Fini");
        TilesManager.Instance.ResetTilesStatut();
        if (PhaseManager.Instance != null)
        {
            PhaseManager.Instance.NextUnit();
        }
    }

}
