using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEditor.SceneManagement;

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

    private Editor currentTemplateEditor;
    private GridTemplate currentTemplate;
    //private Orientation objectOrientation;
    private int objectOrientation;



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
        //currentTemplateEditor = Editor.CreateEditor(currentTemplate);
    }

    private void OnGUI()
    {
        GridParameterBehaviour();
        if (gridCreated)
        {
            EditorGUI.BeginChangeCheck();
            GraphEditorBehaviour();
            if (EditorGUI.EndChangeCheck())
            {
                EditorSceneManager.MarkAllScenesDirty();
            }
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


        EditorGUI.BeginChangeCheck();
        width = EditorGUILayout.IntSlider("Width", width, 1, 25);
        heigth = EditorGUILayout.IntSlider("Heigth", heigth, 1, 25);
        if (EditorGUI.EndChangeCheck())
        {
            // Do something when the property changes 
            gridCreated = false;
            gridSizeValidation = false;
        }

        gridSizeValidation = EditorGUILayout.Toggle("Grid Size Valide", gridSizeValidation);
        currentTemplate = EditorGUILayout.ObjectField("Current Template", currentTemplate, typeof(GridTemplate), false) as GridTemplate;
        EditorGUIUtility.labelWidth = labelWidthBase;
        EditorGUILayout.EndVertical();

        EditorGUI.BeginChangeCheck();
        if (currentTemplate != null)
        {
            if (GUILayout.Button("Load Template"))
            {
                // ClearGrid();
                LoadTemplate();
            }
        }

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
        if (EditorGUI.EndChangeCheck())
        {
            EditorSceneManager.MarkAllScenesDirty();
        }


        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Overwrite Template"))
        {
            CreateTemplate(heigth, width, true);
        }

        if (GUILayout.Button("Create Template"))
        {
            CreateTemplate(heigth, width, false);
        }

        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }



    private GameObject currentTile;
    TileEditorData[] tilesDatas;
    private void GraphEditorBehaviour()
    {
        GUILayout.BeginArea(new Rect(position.width / 4f, 0, position.width * 3f / 4f, position.height));

        //material = (Material)EditorGUILayout.ObjectField("Material", material, typeof(Material));
        //tilesType = (TilesType)EditorGUILayout.EnumPopup("Tiles Type", tilesType);
        currentTile = EditorGUILayout.ObjectField("Tiles Prefab", currentTile, typeof(GameObject), false) as GameObject;
        objectOrientation = EditorGUILayout.Popup("Object Orientation", objectOrientation, Enum.GetNames(typeof(Orientation)));

        if (GUILayout.Button("Fill"))
        {
            for (int i = heigth; i > 0; i--)
            {
                for (int y = 0; y < width; y++)
                {
                    tilesDatas[(i * width + y) - width].prefab = currentTile;
                    tilesDatas[(i * width + y) - width].currentOrientation = (Orientation)objectOrientation;

                    SpawnTile(i, y, currentTile, ((i * width + y) - width), objectOrientation);
                }
            }

        }

        //draw rect 
        //event.current isMouse
        int tileNumbers = width * heigth;

        for (int i = heigth; i > 0; i--)
        {
            EditorGUILayout.BeginHorizontal("box");

            for (int y = 0; y < width; y++)
            {
                if (GUILayout.Button(((i * width + y) - width + 1).ToString(), GUILayout.MaxWidth(60)))
                {
                    tilesDatas[(i * width + y) - width].prefab = currentTile;
                    tilesDatas[(i * width + y) - width].currentOrientation = (Orientation)objectOrientation;

                    SpawnTile(i, y, currentTile, ((i * width + y) - width), objectOrientation);

                }
            }
            EditorGUILayout.EndHorizontal();
        }


        GUILayout.EndArea();
    }


    #endregion

    #region AssetCreation 

    private void CreateTemplate(int heigth, int width, bool overridePattern)
    {
        CheckDirectoryPath("Assets/_Prefabs");
        CheckDirectoryPath("Assets/_Prefabs/LevelTemplate/");

        int tilesNumbers = heigth * width;
        GridTemplate template = ScriptableObject.CreateInstance<GridTemplate>();

        template.datas = tilesDatas;
        template.Heigth = heigth;
        template.Width = width;

        string tempName;
        if (!overridePattern)
        {

            tempName = CheckEmplacement("Assets/_Prefabs/LevelTemplate/" + templateName + ".asset", 1);
        }
        else
        {

            tempName = "Assets/_Prefabs/LevelTemplate/" + templateName + ".asset";
        }

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

    public void SpawnTile(int GridX, int GridY, GameObject prefab, int index, int orientation)
    {

        if (Currenttiles[index] != null)
        {
            DestroyImmediate(Currenttiles[index]);
        }

        if (prefab != null)
        {
            //GameObject GO = Instantiate(prefab, new Vector3(heigth * 2 - (GridX * 2) + 1, 0, GridY * 2 + 1), Quaternion.identity);
            GameObject folder = GameObject.Find("TilesFolder");
            if (folder == null)
            {
                folder = new GameObject("TilesFolder");
                Instantiate(folder);
            }

            GameObject G0 = PrefabUtility.InstantiatePrefab(prefab, folder.transform) as GameObject;
            G0.transform.position = new Vector3(heigth * 2 - (GridX * 2) + 1, 0, GridY * 2 + 1);
            Currenttiles[index] = G0;

            //if (GameObject.Find("TilesFolder") == null)
            //{
            //    GameObject tilesFolder = new GameObject("TilesFolder");
            //    Instantiate(tilesFolder);
            //    GO.transform.parent = tilesFolder.transform;

            //}
            //else
            //{
            //    GameObject folder = GameObject.Find("TilesFolder");
            //    GO.transform.parent = folder.transform;

            //}

            switch (orientation)
            {
                case 0:
                    G0.transform.rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case 1:
                    G0.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case 2:
                    G0.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 3:
                    G0.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                default:
                    break;
            }
        }

    }

    #endregion

    #region Utility

    private void GridEditorGeneration(TileEditorData[] _tilesDatas = null)
    {
        ClearGrid();
        gridCreated = true;
        gridSizeValidation = true;
        int tileNumbers = width * heigth;
        Currenttiles = new GameObject[tileNumbers];
        if (_tilesDatas == null)
        {
            tilesDatas = new TileEditorData[tileNumbers];
        }
        else
        {
            tilesDatas = _tilesDatas;
            for (int i = heigth; i > 0; i--)
            {

                for (int y = 0; y < width; y++)
                {
                    SpawnTile(i, y, tilesDatas[((i * width + y) - width)].prefab, ((i * width + y) - width), (int)tilesDatas[((i * width + y) - width)].currentOrientation);
                }
            }
        }
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
        //for (int i = 0; i < Currenttiles.Length; i++)
        //{
        //    DestroyImmediate(Currenttiles[i]);
        //}
        GameObject folder = GameObject.Find("TilesFolder");
        if (folder != null)
        {
            int childCount = folder.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(folder.transform.GetChild(0).gameObject);
                Debug.Log(childCount);
            }
        }

        tilesDatas = new TileEditorData[0];
        Currenttiles = new GameObject[0];
        gridCreated = false;
    }

    private void LoadTemplate()
    {
        templateName = currentTemplate.name;
        heigth = currentTemplate.Heigth;
        width = currentTemplate.Width;

        GridEditorGeneration(currentTemplate.datas);
    }

    #endregion




}
