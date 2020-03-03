using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    private void OnEnable()
    {
        labelWidthBase = EditorGUIUtility.labelWidth;
    }

    private void OnGUI()
    {
        RenderGridParameter();
        RenderGraphEditor();
    }

    private void RenderGridParameter()
    {
        GUILayout.BeginArea(new Rect(0, 0, position.width / 4f, position.height));

        EditorGUIUtility.labelWidth = labelWidthBase / 2f;
        templateName = EditorGUILayout.TextField("Asset Name", templateName);
        width = EditorGUILayout.IntSlider("Width", width, 1, 25);
        heigth = EditorGUILayout.IntSlider("Heigth", heigth, 1, 25);
        EditorGUIUtility.labelWidth = labelWidthBase;

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Generate Grid"))
        {
            int tilesNumber = heigth * width;

            CreateTemplate(tilesNumber);
        }
        GUILayout.EndArea();
    }

    private void CreateTemplate(int tilesNumbers)
    {
        GridTemplate template = ScriptableObject.CreateInstance<GridTemplate>();
        template.datas = new TileEditorData[tilesNumbers];

        string tempName = CheckEmplacement("Assets/_Prefabs/LevelTemplate/" + templateName + ".asset", 1);

        Debug.Log("Grid of dimension" + width + " : " + heigth + " as been generated");
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



    private Material material;
    private TilesType tilesType;
    private void RenderGraphEditor()
    {
        GUILayout.BeginArea(new Rect(position.width / 4f, 0, position.width*3f/4f, position.height));
        EditorGUILayout.BeginHorizontal("Box");
        material = (Material)EditorGUILayout.ObjectField("Material", material, typeof(Material));
        tilesType = (TilesType)EditorGUILayout.EnumPopup("Tiles Type", tilesType);
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    

}
