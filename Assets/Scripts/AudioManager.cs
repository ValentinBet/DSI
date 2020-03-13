using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioSource as_main = null;
    [SerializeField] private AudioSource as_backgroundMusic = null;
    [SerializeField] private float backgroundMusicBaseVolume = 0.1f;

    [Header("Tiles")]
    [SerializeField] private AudioClip swap = null;
    [SerializeField] private AudioClip selectTile = null;
    [SerializeField] private List<AudioClip> sfxRotateList = new List<AudioClip>();

    [Header("Walls")]
    [SerializeField] private AudioClip wallDestruct = null;
    [SerializeField] private AudioClip wallHit = null;
    [SerializeField] private AudioClip projectileWallHit = null;

    [Header("Actions")]
    [SerializeField] private AudioClip teleport = null;
    [SerializeField] private AudioClip push = null;
    [SerializeField] private AudioClip aoeHit = null;
    [SerializeField] private AudioClip aoeLaunch = null;
    [SerializeField] private AudioClip closeAttack = null;
    [SerializeField] private AudioClip shootProjectile = null;
    [SerializeField] private AudioClip loosePa = null;
    [SerializeField] private AudioClip noMorePa = null;

    [Header("Game")]
    [SerializeField] private AudioClip defeat = null;
    [SerializeField] private AudioClip victory = null;
    [SerializeField] private AudioClip endTurn = null;
    [SerializeField] private AudioClip buttonClick = null;
    [SerializeField] private AudioClip newTurn = null;

    [Header("Characters")]
    [SerializeField] private List<AudioClip> footstepList = new List<AudioClip>();
    [SerializeField] private AudioClip projectileCharacterHit = null;
    [SerializeField] private AudioClip characterDie = null;
    [SerializeField] private AudioClip characterHit = null;
    [SerializeField] private AudioClip characterRotate = null;
    [SerializeField] private AudioClip allyDie = null;
    [SerializeField] private AudioClip levelUp = null;

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
        as_backgroundMusic.volume = Mathf.Lerp(as_backgroundMusic.volume, AudioListener.volume - 0.1f, 0.2f * Time.deltaTime);
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
    public void PlayLoosePa()
    {
        as_main.PlayOneShot(loosePa);
    }
    //
    public void PlayNoMorePa()
    {
        as_main.PlayOneShot(noMorePa);
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
