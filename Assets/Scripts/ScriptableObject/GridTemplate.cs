using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "GridTemplate" , menuName = "Kubz/GridTemplate", order = 0  )]
public class GridTemplate : ScriptableObject
{
    public TileEditorData[] datas;
    public int Heigth, Width;


}

[System.Serializable]
public struct TileEditorData
{
    // public int index;
    public TilesType type;
    public Material mat;
}



