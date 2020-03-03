using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tiles;
    [SerializeField]
    private Material[] debugMaterials;
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
        tempInfos = new int[8*8];
        for (int i =0; i<8*8;i++)
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
        GenerateMap(8, 8, tempInfos);
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

    /*void GenerateMap(GridTemplate)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                switch (infos[i * width + j])
                {
                    case 0:
                        break;
                    case 1:
                        SpawnTile(0, j, i);
                        break;
                    default:
                        break;
                }
            }
        }
        SetCamSettings(width, height);
    }*/

    void SpawnTile(int ID,int GridX, int GridY)
    {
        GameObject GO = Instantiate(tiles[ID], new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1),Quaternion.identity);
        GO.GetComponent<MeshRenderer>().sharedMaterial = debugMaterials[(GridX * 10 + GridY)%1];
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
