using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioSource audioSource;

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

    public void PlayFootsteps()
    {
        audioSource.PlayOneShot(footstepList[Random.Range(0, footstepList.Count)]);
    }

    public void PlayTileRotate()
    {
        audioSource.PlayOneShot(sfxRotateList[Random.Range(0, sfxRotateList.Count)]);
    }
    public void PlaySwap()
    {
        audioSource.PlayOneShot(swap);
    }
    public void PlaySelectTile()
    {
        audioSource.PlayOneShot(selectTile);
    }

    public void PlayWallDestruct()
    {
        audioSource.PlayOneShot(wallDestruct);
    }
    public void PlayWallHit()
    {
        audioSource.PlayOneShot(wallHit);
    }
    public void PlayProjectileWallHit()
    {
        audioSource.PlayOneShot(projectileWallHit);
    }
    public void PlayTeleport()
    {
        audioSource.PlayOneShot(teleport);
    }
    public void PlayPush()
    {
        audioSource.PlayOneShot(push);
    }
    public void PlayAoeHit()
    {
        audioSource.PlayOneShot(aoeHit);
    }
    public void PlayAoeLaunch()
    {
        audioSource.PlayOneShot(aoeLaunch);
    }
    public void PlayCloseAttack()
    {
        audioSource.PlayOneShot(closeAttack);
    }
    public void PlayShootProjectile()
    {
        audioSource.PlayOneShot(shootProjectile);
    }
    public void Playdefeat()
    {
        audioSource.PlayOneShot(defeat);
    }
    public void PlayVictory()
    {
        audioSource.PlayOneShot(victory);
    }
    public void PlayEndTurn()
    {
        audioSource.PlayOneShot(endTurn);
    }
    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClick);
    }
    public void PlayNewTurn()
    {
        audioSource.PlayOneShot(newTurn);
    }
    public void PlayProjectileCharacterHit()
    {
        audioSource.PlayOneShot(projectileCharacterHit);
    }
    public void PlayCharacterDie()
    {
        audioSource.PlayOneShot(characterDie);
    }
    public void PlayCharacterHit()
    {
        audioSource.PlayOneShot(characterHit);
    }
    public void PlayCharacterRotate()
    {
        audioSource.PlayOneShot(characterRotate);
    }
    public void PlayAllyDie()
    {
        audioSource.PlayOneShot(allyDie);
    }
    public void PlayLevelUp()
    {
        audioSource.PlayOneShot(levelUp);
    }

}
