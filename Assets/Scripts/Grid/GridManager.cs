using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager Instance { get { return _instance; } }

    public LayerMask tilesLayer;
    public GridSelector gridSelector;

    private RaycastHit hit;
    private GameObject lastObjHit;
    private Character lastCharacterHit;

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
                PatternReader.instance.PreviewPattern.EndPreview();

                lastObjHit = hit.collider.gameObject;

                if (hit.collider.CompareTag("AllyCharacter") || hit.collider.CompareTag("EnemyCharacter"))
                {
                    lastCharacterHit = hit.collider.GetComponent<Character>();
                    PatternReader.instance.PreviewPattern.ReadPattern(lastCharacterHit);
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
        if (gridSelector.gameObject.activeInHierarchy)
        {
            return gridSelector.GetTile();
        } else
        {
            return null;
        }

    }
}
