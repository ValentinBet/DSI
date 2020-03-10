using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioSource audioSource;


    [SerializeField] private AudioClip aoeHit;
    [SerializeField] private AudioClip aoeHLaunch;
    [SerializeField] private AudioClip closeAttack;
    [SerializeField] private AudioClip wallDestruct;
    [SerializeField] private AudioClip wallHit;
    [SerializeField] private AudioClip newTurn;
    [SerializeField] private AudioClip shootProjectile;
    [SerializeField] private AudioClip projectileWallHit;

    [Header("Tiles")]
    [SerializeField] private AudioClip swap;
    [SerializeField] private List<AudioClip> sfxRotateList = new List<AudioClip>();
    [Header("Actions")]
    [SerializeField] private AudioClip teleport;
    [SerializeField] private AudioClip push;

    [Header("Game")]
    [SerializeField] private AudioClip defeat;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip endTurn;
    [SerializeField] private AudioClip buttonClick;

    [Header("Characters")]
    [SerializeField] private List<AudioClip> footstepList = new List<AudioClip>();
    [SerializeField] private AudioClip projectileCharacterhit;
    [SerializeField] private AudioClip characterDie;
    [SerializeField] private AudioClip characterHit;
    [SerializeField] private AudioClip characterRotate;
    [SerializeField] private AudioClip allyDie;
    [SerializeField] private AudioClip levelUp;

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


}
