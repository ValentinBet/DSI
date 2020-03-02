using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cardinal { All, North, East, South, West }

public class TilesManager : MonoBehaviour
{
    private static TilesManager _instance;
    public static TilesManager Instance { get { return _instance; } }

    public List<TilesManager> AllTilesList = new List<TilesManager>();

    public List<TileProperties> GetTileNeighbors(TileProperties tile, Cardinal cardinalPoint, int lenght)
    {
        List<TileProperties> _TileNeighbors = new List<TileProperties>();

        return _TileNeighbors;
    }


}
