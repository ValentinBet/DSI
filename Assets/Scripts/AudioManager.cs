using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioSource as_main;
    [SerializeField] private AudioSource as_backgroundMusic;
    [SerializeField] private float backgroundMusicBaseVolume = 0f;

    [Header("Tiles")]
    [SerializeField] private AudioClip swap;
    [SerializeField] private AudioClip selectTile;
    [SerializeField] private List<AudioClip> sfxRotateList = new List<AudioClip>();

    [Header("Walls")]
    [SerializeField] private AudioClip wallDestruct;
    [SerializeField] private AudioClip wallHit;
    [SerializeField] private AudioClip projectileWallHit;

    [Header("Traps")]
    [SerializeField] private AudioClip activateTrap;

    [Header("Actions")]
    [SerializeField] private AudioClip teleport;
    [SerializeField] private AudioClip push;
    [SerializeField] private AudioClip aoeHit;
    [SerializeField] private AudioClip aoeLaunch;
    [SerializeField] private AudioClip closeAttack;
    [SerializeField] private AudioClip shootProjectile;

    [Header("Game")]
    [SerializeField] private AudioClip defeat;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip endTurn;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip newTurn;

    [Header("Characters")]
    [SerializeField] private List<AudioClip> footstepList = new List<AudioClip>();
    [SerializeField] private AudioClip projectileCharacterHit;
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
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        as_backgroundMusic.volume = backgroundMusicBaseVolume;
    }

    private void FixedUpdate()
    {
        as_backgroundMusic.volume = Mathf.Lerp(as_backgroundMusic.volume, AudioListener.volume, Time.deltaTime);
    }

    public void PlayFootsteps()
    {
        as_main.PlayOneShot(footstepList[Random.Range(0, footstepList.Count)]);
    }

    public void PlayTileRotate()
    {
        as_main.PlayOneShot(sfxRotateList[Random.Range(0, sfxRotateList.Count)]);
    }
    public void PlaySwap()
    {
        as_main.PlayOneShot(swap);
    }
    public void PlaySelectTile()
    {
        as_main.PlayOneShot(selectTile);
    }
    //
    public void PlayWallDestruct()
    {
        as_main.PlayOneShot(wallDestruct);
    }
    //
    public void PlayWallHit()
    {
        as_main.PlayOneShot(wallHit);
    }
    //
    public void PlayProjectileWallHit()
    {
        as_main.PlayOneShot(projectileWallHit);
    }
    //
    public void PlayTeleport()
    {
        as_main.PlayOneShot(teleport);
    }
    //
    public void PlayPush()
    {
        as_main.PlayOneShot(push);
    }
    //
    public void PlayAoeHit()
    {
        as_main.PlayOneShot(aoeHit);
    }
    //
    public void PlayAoeLaunch()
    {
        as_main.PlayOneShot(aoeLaunch);
    }
    //
    public void PlayCloseAttack()
    {
        as_main.PlayOneShot(closeAttack);
    }
    //
    public void PlayShootProjectile()
    {
        as_main.PlayOneShot(shootProjectile);
    }
    public void Playdefeat()
    {
        as_main.PlayOneShot(defeat);
    }
    public void PlayVictory()
    {
        as_main.PlayOneShot(victory);
    }
    public void PlayEndTurn()
    {
        as_main.PlayOneShot(endTurn);
    }
    public void PlayButtonClick()
    {
        as_main.PlayOneShot(buttonClick);
    }
    public void PlayNewTurn()
    {
        as_main.PlayOneShot(newTurn);
    }
    //
    public void PlayProjectileCharacterHit()
    {
        as_main.PlayOneShot(projectileCharacterHit);
    }
    //
    public void PlayCharacterDie()
    {
        as_main.PlayOneShot(characterDie);
    }
    //
    public void PlayCharacterHit()
    {
        as_main.PlayOneShot(characterHit);
    }
    //
    public void PlayCharacterRotate()
    {
        as_main.PlayOneShot(characterRotate);
    }
    //
    public void PlayAllyDie()
    {
        as_main.PlayOneShot(allyDie);
    }
    public void PlayLevelUp()
    {
        as_main.PlayOneShot(levelUp);
    }

}
