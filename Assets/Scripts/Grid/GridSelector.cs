using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour
{

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, GridManager.Instance.tilesLayer))
        {
            transform.position = hit.collider.transform.position + Vector3.up * 0.3f;
        }

        //new Vector3(Mathf.RoundToInt(hit.point.x)/2 *2+1, 0.5f, Mathf.RoundToInt(hit.point.z)/2 *2+1);
    }

    public TileProperties GetTile()
    {
        RaycastHit hit;

        TileProperties _tp = null;

        if (Physics.Raycast(transform.position, Vector3.down * 10, out hit, Mathf.Infinity, GridManager.Instance.tilesLayer))
        {      
            if (hit.collider.gameObject.GetComponent<TileProperties>() != null)
            {
                _tp = hit.collider.gameObject.GetComponent<TileProperties>();
            }
        }

        return _tp;
    }
}
