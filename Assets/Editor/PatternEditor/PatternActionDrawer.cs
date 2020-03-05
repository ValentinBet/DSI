using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer (typeof(PatternAction))]
public class PatternActionDrawer : PropertyDrawer
{
    //ActionType actionTypeProp;

    SerializedProperty actionDuration, actionType, rotation;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
    private bool initialized = false;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        if (!initialized)
        {
            Init();
        }

        //actionDuration.floatValue = EditorGUI.FloatField(position, label, actionDuration.floatValue);

        EditorGUI.PropertyField(position, property.FindPropertyRelative("actionType"));
       // actionType.enumValueIndex = (int)((ActionType)EditorGUI.EnumPopup(position, "ActionType", actionType));
       //actionType.enumValueIndex = (int)actionTypeProp;
        //if (actionType.enumValueIndex == (int)ActionType.Rotation)
        //{
        //    GUILayout.TextField("OK");
        //}


        //base.OnGUI(position, property, label);
    }

    private void Init()
    {
        initialized = true;
    }

}
