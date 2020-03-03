using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tiles;
    [SerializeField]
    private Material[] debugMaterials;
    [SerializeField]
    private float gridScale=2;
    private int[] tempInfos;

    public GridGenerator Instance;

    void Start()
    {
        if(Instance ==null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
        tempInfos = new int[100];
        for (int i =0; i<100;i++)
        {
            if (Random.Range(0.0f, 1.0f) < 0.95f)
            {
                tempInfos[i] = 1;
            }
            else
            {
                tempInfos[i] = 0;
            }
        }
        GenerateMap(10, 10, tempInfos);
    }

    void GenerateMap(int width, int height, int[] infos)
    {
        for(int i = 0; i< height;i++)
        {
            for(int j = 0; j< width; j++)
            {
                switch (infos[i * width + j])
                {
                    case 0:
                        break;
                    case 1:
                        SpawnTile(0,j,i);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void SpawnTile(int ID,int GridX, int GridY)
    {
        GameObject GO = Instantiate(tiles[ID], new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1),Quaternion.identity);
        GO.transform.localScale = Vector3.one * gridScale;
        GO.GetComponent<MeshRenderer>().sharedMaterial = debugMaterials[(GridX * 10 + GridY) % 3];
    }
}
