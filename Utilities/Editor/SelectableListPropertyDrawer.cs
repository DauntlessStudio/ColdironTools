using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SelectableListAttribute))]
public class SelectableListPropertyDrawer : PropertyDrawer
{
    #region Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SelectableListAttribute listAttribute = (SelectableListAttribute)attribute;

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        int index = GetIndex(listAttribute, property);
        string[] popupOptions = GetOptions(listAttribute, property);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        int result = EditorGUI.Popup(position, index, popupOptions);

        SetIndex(listAttribute, property, result);

        if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    private string[] GetOptions(SelectableListAttribute attribute, SerializedProperty property)
    {
        string[] options = new string[1];

        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, attribute.list); //changes the path to the conditionalsource property path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            List<string> tempList = new List<string>();

            for (int i = 0; i < sourcePropertyValue.arraySize; i++)
            {
                tempList.Add(sourcePropertyValue.GetArrayElementAtIndex(i).stringValue);
            }

            options = tempList.ToArray();
        }
        else
        {
            Debug.LogWarning("Attempting to use a SelectableListAttribute but no matching Source List found in object: " + attribute);
        }

        return options;
    }

    private int GetIndex(SelectableListAttribute attribute, SerializedProperty property)
    {
        int val = 0;

        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, attribute.index); //changes the path to the conditionalsource property path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            val = sourcePropertyValue.intValue;
        }
        else
        {
            Debug.LogWarning("No Source Property");
        }

        return val;
    }

    private void SetIndex(SelectableListAttribute attribute, SerializedProperty property, int index)
    {
        string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
        string conditionPath = propertyPath.Replace(property.name, attribute.index); //changes the path to the conditionalsource property path
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            sourcePropertyValue.intValue = index;
        }
        else
        {
            Debug.LogWarning("No Source Property");
        }
    }
    #endregion
}
