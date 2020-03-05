using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(PatternAction))]
public class PatternActionDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int numberOfLines = 1;

        SerializedProperty actionTypeProp = property.FindPropertyRelative("actionType");

        numberOfLines++;

        if (actionTypeProp.enumValueIndex == (int)ActionType.Rotation)
        {
            numberOfLines++;
        }

        return (EditorGUIUtility.singleLineHeight + 1) * numberOfLines;

        //return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty actionDurationProp = property.FindPropertyRelative("actionDuration");
        SerializedProperty actionTypeProp = property.FindPropertyRelative("actionType");
        SerializedProperty rotationProp = property.FindPropertyRelative("rotation");

        ActionType newAT = (ActionType)actionTypeProp.enumValueIndex;

        EditorGUI.Slider(position, actionDurationProp, 0, 2f, "Action Duration");

        actionTypeProp.enumValueIndex = EditorGUI.Popup(position, (int)newAT, Enum.GetNames(typeof(ActionType)));
        if (actionTypeProp.enumValueIndex == (int)ActionType.Rotation)
        {
            EditorGUI.LabelField(position, "OUI");
        }
    }



}