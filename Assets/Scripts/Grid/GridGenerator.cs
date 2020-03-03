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
    [SerializeField]
    private bool debugMode = false;

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
        if (debugMode == true)
        {
            if (GT == null)
            {
                tempInfos = new int[100];
                for (int i = 0; i < 100; i++)
                {
                    tempInfos[i] = 1;
                }
                GenerateMap(10, 10, tempInfos);
            }
            else
            {
                GenerateMap(GT);
            }
        }
    }

    //Using int IDS (DEPRECATED)
    public void GenerateMap(int width, int height, int[] infos)
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

    //Using GridTemplate (Ld Tool format)
    public void GenerateMap(GridTemplate template)
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

    //Spawning using Int ID
    public void SpawnTile(int ID,int GridX, int GridY)
    {
        GameObject GO = Instantiate(tiles[ID], new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1),Quaternion.identity);
        GO.GetComponent<MeshRenderer>().sharedMaterial = debugMaterials[(GridX * 10 + GridY)%debugMaterials.Length];
        GO.GetComponent<TileProperties>().tileID = new Vector2(GridX, GridY);
    }

    //Spawning using GridTemplate Material data
    public void SpawnTile(int GridX, int GridY,Material tileMat)
    {
        GameObject GO = Instantiate(tiles[0], new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1), Quaternion.identity);
        GO.GetComponent<MeshRenderer>().sharedMaterial = tileMat;
        GO.GetComponent<TileProperties>().tileID = new Vector2(GridX, GridY);
    }

    //Update camera to center it on the level
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
