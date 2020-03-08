using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager Instance { get { return _instance; } }

    public LayerMask tilesLayer;
    [SerializeField] GridSelector gridSelector;

    private RaycastHit hit;
    private GameObject lastObjHit;

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
    }


    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            gridSelector.gameObject.SetActive(true);

            if (lastObjHit != hit.collider.gameObject)
            {
                lastObjHit = hit.collider.gameObject;

                if (hit.collider.CompareTag("AllyCharacter"))
                {
                    AllyCharacter allyCharacter = hit.collider.GetComponent<AllyCharacter>();
                    PatternReader.instance.PreviewReader.PreviewPattern(allyCharacter.mouvementPattern, allyCharacter.occupiedTile);
                }
            }

        }
        else
        {
            gridSelector.gameObject.SetActive(false);
        }
    }

    public TileProperties GetTileUnderSelector()
    {
        return gridSelector.GetTile();
    }
}
