using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{
    [MenuItem("Window/Kubz/LevelEditor")]
    static void Init()
    {
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
        window.Show();
    }


    string templateName = "level_X_Template";
    int width = 0, heigth = 0;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, position.width / 4f, position.height));
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Generate Grid"))
        {
            Debug.Log("Grid of dimension" + width + " : " + heigth + " as been generated");
        }
        GUILayout.EndArea();

    }

}
