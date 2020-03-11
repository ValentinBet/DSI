using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesChanger : MonoBehaviour
{
    public GameObject initTile;
    public GameObject lastTile;
    private List<TileProperties> tileRotateList = new List<TileProperties>();

    private Vector3 tempPos;
    [SerializeField] private GameObject swapSprite;
    [SerializeField] private GameObject rotateSprite;
    private List<GameObject> swapSpriteList;

    private void Start()
    {
        PoolObjects();
    }

    private void PoolObjects()
    {
        swapSpriteList = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            GameObject obj = Instantiate(swapSprite, this.transform);
            obj.SetActive(false);
            swapSpriteList.Add(obj);
        }
    }

    private GameObject GetSwapSpriteInPool()
    {
        for (int i = 0; i < swapSpriteList.Count; i++)
        {
            if (!swapSpriteList[i].activeInHierarchy)
            {
                return swapSpriteList[i];
            }
        }
        return null;
    }

    public bool TryChangePos(GameObject tile)
    {
        if (initTile == null)
        {
            initTile = tile;

            GameObject _swapSprite = GetSwapSpriteInPool();

            if (_swapSprite != null)
            {
                _swapSprite.SetActive(true);
                _swapSprite.transform.position = GridManager.Instance.gridSelector.transform.position;
            }
        }
        else if (tile != initTile)
        {
            lastTile = tile;
            return InitChange();
        }

        return false;
    }

    public bool InitChange()
    {
        if (initTile != null && lastTile != null)
        {
            tempPos = initTile.transform.position;
            initTile.transform.position = lastTile.transform.position;
            lastTile.transform.position = tempPos;
            AudioManager.Instance.PlaySwap();
            ClearChoice();
            return true;
        }

        return false;
    }

    public void ClearChoice()
    {
        initTile = null;
        lastTile = null;

        HideAllHints();

    }

    public void HideAllHints()
    {
        for (int i = 0; i < swapSpriteList.Count; i++)
        {
            swapSpriteList[i].SetActive(false);
        }

        rotateSprite.SetActive(false);
    }

    public void DisplayRotateHint()
    {
        rotateSprite.transform.position = GridManager.Instance.gridSelector.transform.position;
    }

    public bool TryRotateTile()
    {
        TileProperties _tp = GridManager.Instance.GetTileUnderSelector();

        if (tileRotateList.Contains(_tp))
        {
            RotateTile(_tp);
            return true;
        }
        else
        {
            if (GameTracker.Instance.IsHavingEnoughtPa())
            {
                RotateTile(_tp);
            }
            return false;
        }
    }

    private void RotateTile(TileProperties _tp)
    {
        _tp.transform.Rotate(new Vector3(0, 90, 0));
        AudioManager.Instance.PlayTileRotate();
        initTile = null;
        tileRotateList.Add(_tp);
    }
}
