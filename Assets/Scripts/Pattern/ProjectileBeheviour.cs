using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBeheviour : MonoBehaviour
{
    private int _index, _depth;
    private Character _shooter;
    private TileProperties currentTile;
    private bool isOnFire;
    private List<TileProperties> tilesColored = new List<TileProperties>();
    private TileProperties lastTeleoprtUsed;
    public int maxTeleportSupported = 3;
    private int teleportNumber;
    private bool _continuePartern;
    private bool isInit = false;

    public void Init(Character shooter, int index, int depth, bool continuePattern)
    {
        _depth = depth;
        _index = index;
        _shooter = shooter;
        _continuePartern = continuePattern;
        isInit = true;
    }


    private void FixedUpdate()
    {
        if (isInit)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 10);

            TileProperties testedTile = GetCurrentTile(2);
            if (testedTile == null)
            {
                DestroyProjectile();
            }
            if (currentTile != testedTile)
            {
                CheckTile(testedTile);
            }
        }

    }

    public TileProperties GetCurrentTile(int lenght = 1)
    {
        RaycastHit hitTile;


        Physics.Raycast(transform.position, Vector3.down, out hitTile, lenght, TilesManager.Instance.tileLayer);

        if (hitTile.collider != null)
        {
            if (hitTile.collider.gameObject.GetComponent<TileProperties>() != null)
            {
                return hitTile.collider.gameObject.GetComponent<TileProperties>();
            }
        }

        return null;
    }

    public void DestroyProjectile()
    {
        if (_shooter != null)
        {
            _shooter.RegisteredDeathProjectile(_index, _depth, tilesColored, _continuePartern);
        }

        Destroy(this.gameObject);
    }

    private void CheckTile(TileProperties testedTile)
    {
        if (testedTile == null)
        {
            //DestroyProjectile();
            return;
        }

        if (testedTile.tileImpact != null)
        {
            testedTile.tileImpact.ActivateImpact(0.4f, 0.3f);
        }


        if (testedTile.specificity != TileProperties.TilesSpecific.PlayerBase)
        {
            tilesColored.Add(testedTile);
            TilesManager.Instance.ChangeTileMaterial(testedTile, PatternReader.instance.attackMat);
            currentTile = testedTile;
        }

        if (testedTile.isOccupied)
        {
            if (testedTile.occupant == _shooter)
            {
                _shooter.RegisteredDeathProjectile(_index, _depth, tilesColored, _continuePartern);
                testedTile.occupant.GotAttacked(_shooter.damage + 1, _shooter, "by projectile on fire");
                testedTile.VFXGestion.toggleVFx(testedTile.VFXGestion.attack.VFXGameObject, true, true, testedTile.VFXGestion.attack.duration);
                AudioManager.Instance.PlayProjectileCharacterHit();
                Destroy(this.gameObject);
            }

            testedTile.occupant.GotAttacked(_shooter.damage, _shooter, "by projectile");
            if (isOnFire)
            {
                testedTile.occupant.GotAttacked(1, _shooter, "by projectile on fire");
            }
            testedTile.VFXGestion.toggleVFx(testedTile.VFXGestion.attack.VFXGameObject, true, true, testedTile.VFXGestion.attack.duration);
            AudioManager.Instance.PlayProjectileCharacterHit();
            DestroyProjectile();

        }
        switch (testedTile.specificity)
        {
            case TileProperties.TilesSpecific.Push:
                // 2 = tile Size
                if (testedTile.isActivated)
                {
                    AudioManager.Instance.PlayPush();
                    Vector3 pushPos = transform.position + testedTile.transform.forward * 2;
                    transform.position = pushPos;
                    testedTile.ChangeTilesActivationStatut(false);
                }
                break;
            case TileProperties.TilesSpecific.Fire:
                isOnFire = true;
                break;
            case TileProperties.TilesSpecific.Door:
                if (testedTile.isActivated)
                {
                    testedTile.ChangeTilesActivationStatut(false);
                    DestroyProjectile();
                }
                break;

            case TileProperties.TilesSpecific.Wall:

                testedTile.GetDamaged(_shooter.damage);
                testedTile.VFXGestion.toggleVFx(testedTile.VFXGestion.attack.VFXGameObject, true, true, testedTile.VFXGestion.attack.duration);
                AudioManager.Instance.PlayProjectileWallHit();
                DestroyProjectile();
                break;
            case TileProperties.TilesSpecific.Teleport:

                AudioManager.Instance.PlayTeleport();
                TileProperties teleportExit = testedTile.GetTeleportExit();
                if (lastTeleoprtUsed == teleportExit)
                {
                    return;
                }
                lastTeleoprtUsed = testedTile;
                teleportNumber++;
                if (teleportNumber == maxTeleportSupported)
                {
                    DestroyProjectile();
                }
                // Vector added = space between the bullet and the ground;
                transform.position = teleportExit.transform.position + new Vector3(0, 0.5f, 0);
                transform.rotation = teleportExit.transform.rotation;

                break;
            case TileProperties.TilesSpecific.PlayerBase:

                Debug.Log("Projectile on base");
                DestroyProjectile();
                break;

                //case TileProperties.TilesSpecific.Ordre:
                //    if (testedTile.order == TileProperties.TilesOrder.rotate)
                //    {
                //        transform.rotation = testedTile.transform.rotation;
                //    }
                //    break;


        }
    }

    private IEnumerator WaitBeforeDeath()
    {
        yield return new WaitForFixedUpdate();
        Destroy(this.gameObject);
    }
}


