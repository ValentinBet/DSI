using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "GridTemplate" , menuName = "Kubz/GridTemplate", order = 0  )]
public class GridTemplate : ScriptableObject
{
    public int Heigth, Width;
    public TileEditorData[] datas;

}

[System.Serializable]
public struct TileEditorData
{
    // public int index;
    public TilesType type;
    public GameObject prefab;
    public Material mat;
}



