﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesChanger : MonoBehaviour
{
    public GameObject initTile;
    public GameObject lastTile;

    private Vector3 tempPos;

    public void SetSwapPos(GameObject tile)
    {
        if (initTile == null)
        {
            initTile = tile;
        }
        else
        {
            lastTile = tile;
        }
    }

    public void InitChange()
    {
        if (initTile != null && lastTile != null)
        {
            tempPos = initTile.transform.position;
            initTile.transform.position = lastTile.transform.position;
            lastTile.transform.position = tempPos;

            ClearChoice();
        }
    }

    public void ClearChoice()
    {
        initTile = null;
        lastTile = null;
    }

    public void RotateTile()
    {
        initTile.transform.Rotate(new Vector3(0, 90, 0));
    }
}
