﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cardinal { North, East, South, West }

public class TilesManager : MonoBehaviour
{
    private static TilesManager _instance;
    public static TilesManager Instance { get { return _instance; } }

    public static TilesChanger TilesChangerInstance;

    public List<TileProperties> AllTilesList = new List<TileProperties>();

    #region Tiles Specific

    public List<TileProperties> trapList = new List<TileProperties>();
    public List<TileProperties> teleportList = new List<TileProperties>();
    #endregion

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

        TilesChangerInstance = GetComponent<TilesChanger>();
    }


    public List<TileProperties> GetTileNeighbors(TileProperties tile, Cardinal cardinalPoint, int lenght = 1, bool isIncludingMainTile = false)
    {
        List<TileProperties> _TileNeighbors = new List<TileProperties>();

        Vector3 direction = Vector3.zero;

        switch (cardinalPoint)
        {
            case Cardinal.North:
                direction = Vector3.forward;
                break;
            case Cardinal.East:
                direction = Vector3.right;
                break;
            case Cardinal.South:
                direction = -Vector3.forward;
                break;
            case Cardinal.West:
                direction = -Vector3.right;
                break;
            default:
                print("Error on cardinal point value");
                break;
        }

        return tile.GetTileOnDirection(direction, lenght);
    }

    public List<TileProperties> GetTileAllNeighbors(TileProperties tile, int lenght = 1, bool isIncludingMainTile = false)
    {
        List<TileProperties> _TileNeighbors = new List<TileProperties>();

        _TileNeighbors = tile.GetTileOnDirection(Vector3.forward, lenght);

        foreach (TileProperties Tp in tile.GetTileOnDirection(Vector3.right, lenght))
        {
            _TileNeighbors.Add(Tp);
        }
        foreach (TileProperties Tp in tile.GetTileOnDirection(-Vector3.forward, lenght))
        {
            _TileNeighbors.Add(Tp);
        }
        foreach (TileProperties Tp in tile.GetTileOnDirection(-Vector3.right, lenght))
        {
            _TileNeighbors.Add(Tp);
        }

        return _TileNeighbors;
    }


}
