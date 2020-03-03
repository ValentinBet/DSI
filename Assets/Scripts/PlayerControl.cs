using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (GridManager.Instance.GetTileUnderSelector() != null)
            {
                foreach (TileProperties tile in TilesManager.Instance.GetTileAllNeighbors(GridManager.Instance.GetTileUnderSelector(), 3))
                {                   
                    // tile.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }

        }
    }
}
