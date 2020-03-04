using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class LevelEditor : EditorWindow
{
    private float labelWidthBase;

    [MenuItem("Window/Kubz/LevelEditor")]
    static void Init()
    {
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
        window.Show();
    }

    private string templateName = "level_X_Template";
    private int width = 0, heigth = 0;
    private bool gridSizeValidation;
    private bool gridCreated;

    //private string[] prefabsName;
    //private Object[] prefabsArray;

    private void OnEnable()
    {

        gridCreated = false;
        labelWidthBase = EditorGUIUtility.labelWidth;
        //prefabsArray = AssetDatabase.LoadAllAssetsAtPath("Assets/_Prefabs/Tiles/");

        //prefabsName = new string[prefabsArray.Length];
        //for (int i = 0; i < prefabsName.Length; i++)
        //{
        //    prefabsName[i] = prefabsArray[i].name;
        //}   
    }

    private void OnGUI()
    {
        GridParameterBehaviour();
        if (gridCreated)
        {
            GraphEditorBehaviour();
        }
    }


    #region WindowsBehaviour
    private void GridParameterBehaviour()
    {
        GUILayout.BeginArea(new Rect(0, 0, position.width / 4f, position.height));


        EditorGUILayout.BeginVertical("box");

        SectionTitle("Grid Parameter", 0.4f);

        EditorGUIUtility.labelWidth = labelWidthBase / 2f;




        templateName = EditorGUILayout.TextField("Asset Name", templateName);
        width = EditorGUILayout.IntSlider("Width", width, 1, 25);
        heigth = EditorGUILayout.IntSlider("Heigth", heigth, 1, 25);
        gridSizeValidation = EditorGUILayout.Toggle("Grid Size Valide", gridSizeValidation);
        EditorGUIUtility.labelWidth = labelWidthBase;
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        if (gridCreated)
        {
            if (GUILayout.Button("Clear Grid"))
            {
                ClearGrid();
            }
        }

        if (gridSizeValidation)
        {
            if (GUILayout.Button("GenerateGrid"))
            {
                GridEditorGeneration();
            }
        }
        if (GUILayout.Button("Generate Template"))
        {
            CreateTemplate(heigth, width);
        }
        GUILayout.EndArea();
    }

    private GameObject currentTile;
    TileEditorData[] tilesDatas;

    private void GraphEditorBehaviour()
    {
        GUILayout.BeginArea(new Rect(position.width / 4f, 0, position.width * 3f / 4f, position.height));

        //material = (Material)EditorGUILayout.ObjectField("Material", material, typeof(Material));
        //tilesType = (TilesType)EditorGUILayout.EnumPopup("Tiles Type", tilesType);
        currentTile = (GameObject)EditorGUILayout.ObjectField("Tiles Prefab", currentTile, typeof(GameObject));

        //draw rect 
        //event.current isMouse
        int tileNumbers = width * heigth;

        for (int i = heigth; i > 0; i--)
        {
            EditorGUILayout.BeginHorizontal("box");

            for (int y = 0; y < width; y++)
            {
                if (GUILayout.Button(((i * width + y) - width + 1).ToString()))
                {
                    tilesDatas[(i * width + y) - width].prefab = currentTile;
                    SpawnTile(i, y, currentTile, ((i * width + y) - width));
                }
            }
            EditorGUILayout.EndHorizontal();
        }


        GUILayout.EndArea();
    }
    #endregion

    #region AssetCreation 

    private void CreateTemplate(int heigth, int width)
    {
        CheckDirectoryPath("Assets/_Prefabs");
        CheckDirectoryPath("Assets/_Prefabs/LevelTemplate/");

        int tilesNumbers = heigth * width;
        GridTemplate template = ScriptableObject.CreateInstance<GridTemplate>();

        template.datas = tilesDatas;
        template.Heigth = heigth;
        template.Width = width;

        string tempName = CheckEmplacement("Assets/_Prefabs/LevelTemplate/" + templateName + ".asset", 1);

        Debug.Log("Grid of dimension " + width + " : " + heigth + " as been generated");
        AssetDatabase.CreateAsset(template, tempName);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = template;
    }

    private string CheckEmplacement(string dataPath, int index)
    {
        GridTemplate emplacement = (GridTemplate)AssetDatabase.LoadAssetAtPath(dataPath, typeof(GridTemplate));

        if (emplacement != null)
        {
            string newPath = "Assets/_Prefabs/LevelTemplate/" + templateName + " (" + index + ")" + ".asset";
            index++;
            return CheckEmplacement(newPath, index);
        }

        return dataPath;
    }

    private void CheckDirectoryPath(string DirectoryPath)
    {
        if (!AssetDatabase.IsValidFolder(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }
    }

    #endregion

    #region Map Creation 


    GameObject[] Currenttiles;
    //private GameObject _tilesFolder;

    public void SpawnTile(int GridX, int GridY, GameObject prefab, int index)
    {

        if (Currenttiles[index] != null)
        {
            DestroyImmediate(Currenttiles[index]);
        }
        if (prefab != null)
        {
            GameObject GO = Instantiate(prefab, new Vector3(GridX * 2 + 1, 0, GridY * 2 + 1), Quaternion.identity);
            Currenttiles[index] = GO;

            if (GameObject.Find("TilesFolder") == null)
            {
                GameObject tilesFolder = new GameObject("TilesFolder");
                Instantiate(tilesFolder);
                GO.transform.parent = tilesFolder.transform;
            }
            else
            {
                GameObject folder = GameObject.Find("TilesFolder");
                GO.transform.parent = folder.transform;
            }
        }
    }

    #endregion

    #region Utility

    private void GridEditorGeneration()
    {
        gridCreated = true;
        gridSizeValidation = false;
        int tileNumbers = width * heigth;
        tilesDatas = new TileEditorData[tileNumbers];
        Currenttiles = new GameObject[tileNumbers];
    }

    private void SectionTitle(string title, float labelSize)
    {
        EditorGUILayout.BeginHorizontal("box");
        GUILayout.FlexibleSpace();
        EditorGUIUtility.labelWidth = labelWidthBase * labelSize;
        EditorGUILayout.LabelField(title);

        EditorGUIUtility.labelWidth = labelWidthBase;
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void ClearGrid()
    {
        for (int i = 0; i < Currenttiles.Length; i++)
        {
            DestroyImmediate(Currenttiles[i]);
        }
        GameObject folder = GameObject.Find("TilesFolder");
        if (folder != null)
        {
            int childCount = folder.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(folder.transform.GetChild(i).gameObject);
                Debug.Log(childCount);
            }
        }

        tilesDatas = new TileEditorData[0];
        Currenttiles = new GameObject[0];
        gridCreated = false;
    }

    #endregion





    private void GetPrefabs()
    {
        //drag and drop 
        //event
    }


}
