using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton permettant de lire les différents patterns
/// </summary>
public class PatternReader : MonoBehaviour
{
    public static PatternReader instance { get { return _instance; } }
    private static PatternReader _instance;

    public Material mouvementMat, attackMat, rotationMat, clickMat, interactionMat , deathMat , receiveDamageMat;

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
    /// <summary>
    /// met fin au tour du personnage
    /// </summary>
    public void FinishTurn()
    {
        TilesManager.Instance.ResetTilesStatut();
        if (PhaseManager.Instance != null)
        {
            PhaseManager.Instance.NextUnit();
        }
    }

}
