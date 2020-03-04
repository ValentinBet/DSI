using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GridTemplate GT;

    public static GridGenerator Instance { get { return _instance; } }
    private static GridGenerator _instance;

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
           GenerateMap(GT);
       }
       else
       {
           Debug.LogError("No Grid Template Assigned, generation stopped");
       }
    }

    //Using GridTemplate (Ld Tool format)
    public void GenerateMap(GridTemplate template)
    {
        for (int i = 0; i < template.Heigth; i++)
        {
            for (int j = 0; j < template.Width; j++)
            {
                if (template.datas[i * template.Width + j].prefab != null)
                {
                    SpawnTile(j, i, template.datas[i * template.Width + j].prefab);
                }
                else
                {
                    Debug.LogWarning("Void Tile (" + (i * template.Width + j).ToString() + ") consider assigning a prefab.");
                }
            }
        }
        SetCamSettings(template.Width, template.Heigth);
    }

    //Spawning using GridTemplate Material data
    /*public void SpawnTile(int GridX, int GridY,Material tileMat)
    {
        GameObject GO = Instantiate(tiles[0], new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1), Quaternion.identity);
        GO.GetComponent<MeshRenderer>().sharedMaterial = tileMat;
        GO.GetComponent<TileProperties>().tileID = new Vector2(GridX, GridY);
    }*/

    public void SpawnTile(int GridX, int GridY,GameObject prefab)
    {
        GameObject GO = Instantiate(prefab, new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1), Quaternion.identity);
        GO.GetComponent<TileProperties>().tileID = new Vector2(GridX, GridY);
    }

    //Update camera to center it on the level
    void SetCamSettings(int width,int height)
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
