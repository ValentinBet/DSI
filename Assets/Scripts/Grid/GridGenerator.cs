using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GridTemplate GT;

    [SerializeField]
    private Transform levelParent;

    public static GridGenerator Instance { get { return _instance; } }
    private static GridGenerator _instance;

    private int debugInt;
    int t_height;
    int t_width;

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

    void Start()
    {
       if (GT != null)
       {
           //GenerateMap(GT);
           //SetCamSettings(GT.Width, GT.Heigth);
        }
       else
       {
           Debug.LogError("No Grid Template Assigned, generation stopped");
       }
    }


    [ContextMenu("EditorGenerate")]
    public void GenerateMap()
    {
        if (levelParent.childCount == 0)
        {
            debugInt = 0;
            GenerateMap(GT);
        }
        else
        {
            Debug.LogWarning("To Generate level, make sure that the level parent is empty");
        }
    }

    //Using GridTemplate (Ld Tool format)
    public void GenerateMap(GridTemplate template)
    {
        t_height = template.Heigth;
        t_width = template.Width;
        for (int i = 0; i < template.Heigth; i++)
        {
            for (int j = 0; j < template.Width; j++)
            {
                if (template.datas[i * template.Width + j].prefab != null)
                {
                    SpawnTile(i, j, template.datas[i * template.Width + j].prefab,(int)template.datas[i*template.Width+j].currentOrientation);
                }
                else
                {
                    Debug.LogWarning("Void Tile (" + (j * template.Width + i).ToString() + ") consider assigning a prefab.");
                }
            }
        }
    }

    //Spawning using GridTemplate Material data
    /*public void SpawnTile(int GridX, int GridY,Material tileMat)
    {
        GameObject GO = Instantiate(tiles[0], new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1), Quaternion.identity);
        GO.GetComponent<MeshRenderer>().sharedMaterial = tileMat;
        GO.GetComponent<TileProperties>().tileID = new Vector2(GridX, GridY);
    }*/

    public void SpawnTile(int GridX, int GridY,GameObject prefab,int orientation)
    {
        GameObject GO = Instantiate(prefab, new Vector3(t_height*2-GridX * 2 + 1, 0,GridY * 2 + 1), prefab.transform.rotation,levelParent);
        switch (orientation)
        {
            case 0:
                GO.transform.localRotation = Quaternion.Euler(0, -90, 0);
                break;
            case 1:
                GO.transform.localRotation = Quaternion.Euler(0, 90, 0);
                break;
            case 2:
                GO.transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case 3:
                GO.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            default:
                break;
        }
        GO.GetComponent<TileProperties>().tileID = new Vector2(GridX, GridY);
        debugInt++;
        GO.name = debugInt.ToString();
    }

    //Update camera to center it on the level
    public void SetCamSettings(int width,int height)
    {
        CameraManager.Instance.ChangeCamPivot(new Vector3(width / 2 * 2, 0, height / 2 * 2));
        if (width < height)
        {
            CameraManager.Instance.ChangeCamSize(0.83f * height + 2.81f);//0.325f);
        } else
        {
            CameraManager.Instance.ChangeCamSize(0.83f * width + 2.81f);
        }
    }
}
