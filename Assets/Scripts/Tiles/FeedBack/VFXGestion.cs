using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXGestion : MonoBehaviour
{
    public VFXAsset SwapTileSlected;
    public VFXAsset SwapTile;
    public VFXAsset SpawnUnit;
    public VFXAsset previewHit;
    public VFXAsset attack;


    public void toggleVFx(GameObject vfx, bool activate, bool haveAWaitingTime = false, float duration = 0)
    {
        if (haveAWaitingTime)
        {
            StartCoroutine(toggleVFx(vfx, activate, duration));
        }
        vfx.SetActive(activate);
    }

    private IEnumerator toggleVFx(GameObject vfx, bool activate, float duration)
    {
        yield return new WaitForSeconds(duration);
        vfx.SetActive(activate);
    }
}

[System.Serializable]
public struct VFXAsset
{
    public GameObject VFXGameObject;
    public float duration;
}