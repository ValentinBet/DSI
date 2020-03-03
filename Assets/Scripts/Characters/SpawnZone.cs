using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private LayerMask tilesLayer;

#if UNITY_EDITOR

    [Header("Editor Only")]
    [SerializeField] private Color editorBoxColor;

    void OnDrawGizmos()
    {
        Gizmos.color = editorBoxColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

#endif

    public List<TileProperties> GetFreeTiles()
    {
        List<TileProperties> freeTiles = new List<TileProperties>();

        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, tilesLayer);

        foreach (Collider tile in hitColliders)
        {
            TileProperties _tp = tile.gameObject.GetComponent<TileProperties>();

            if (_tp.CharacterCanSpawn())
            {
                freeTiles.Add(_tp);
            }
        }

        return freeTiles;
    }


}
