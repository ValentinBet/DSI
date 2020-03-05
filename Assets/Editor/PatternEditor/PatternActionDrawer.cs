using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

//[CustomPropertyDrawer(typeof(PatternAction))]
public class PatternActionDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty actionDurationProp = property.FindPropertyRelative("actionDuration");
        SerializedProperty actionTypeProp = property.FindPropertyRelative("actionType");
        SerializedProperty rotationProp = property.FindPropertyRelative("rotation");

        ActionType newAT = (ActionType)actionTypeProp.enumValueIndex; 

        EditorGUI.Slider(position, actionDurationProp, 0, 2f ,"Action Duration" );

    //    actionTypeProp.enumValueIndex = (ActionType)EditorGUI.EnumPopup(position, newAT);

    }



}