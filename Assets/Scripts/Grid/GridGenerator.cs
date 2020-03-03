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
    private GridTemplate GT;
    private int[] tempInfos;

    public static GridGenerator Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        if (GT == null)
        {
            tempInfos = new int[8 * 8];
            for (int i = 0; i < 8 * 8; i++)
            {
                tempInfos[i] = 1;
            }
            GenerateMap(8, 8, tempInfos);
        }
        else
        {
            GenerateMap(GT);
        }
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
        SetCamSettings(width, height);
    }

    void GenerateMap(GridTemplate template)
    {
        for (int i = 0; i < template.Heigth; i++)
        {
            for (int j = 0; j < template.Width; j++)
            {
                switch(template.datas[i * template.Width + j].type)
                {
                    case TilesType.Normal:
                        SpawnTile(j, i, template.datas[i * template.Width + j].mat);
                        break;
                    default:
                        break;
                }
                
            }
        }
        SetCamSettings(template.Width, template.Heigth);
    }

    void SpawnTile(int ID,int GridX, int GridY)
    {
        GameObject GO = Instantiate(tiles[ID], new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1),Quaternion.identity);
        GO.GetComponent<MeshRenderer>().sharedMaterial = debugMaterials[(GridX * 10 + GridY)%debugMaterials.Length];
        GO.GetComponent<TileProperties>().tileID = new Vector2(GridX, GridY);
    }

    void SpawnTile(int GridX, int GridY,Material tileMat)
    {
        GameObject GO = Instantiate(tiles[0], new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1), Quaternion.identity);
        GO.GetComponent<MeshRenderer>().sharedMaterial = tileMat;
        GO.GetComponent<TileProperties>().tileID = new Vector2(GridX, GridY);
    }

    void SetCamSettings(int width,int height)
    {
        CameraManager.Instance.ChangeCamPivot(new Vector3(width / 2 * 2, 0, height / 2 * 2));
        if (width < height)
        {
            CameraManager.Instance.ChangeCamSize(0.83f * height + 0.325f);
        } else
        {
            CameraManager.Instance.ChangeCamSize(0.83f * width + 0.325f);
        }
    }
}
