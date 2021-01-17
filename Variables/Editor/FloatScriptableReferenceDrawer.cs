// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Modified By: Caleb Coldiron
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEditor;

namespace ColdironTools.Scriptables
{
    /// <summary>
    /// A property drawer keeping scriptable references on one line and making thing clear at a glance for designers.
    /// </summary>
    [CustomPropertyDrawer(typeof(FloatScriptableReference))]
    public class FloatScriptableReferenceDrawer : PropertyDrawer
    {
        #region Fields
        private readonly string[] popupOptions = { "Use Local", "Use Reference" };

        private GUIStyle popupStyle;
        #endregion

        #region Methods
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty useLocal = property.FindPropertyRelative("useLocalValue");
            SerializedProperty localValue = property.FindPropertyRelative("localValue");
            SerializedProperty floatScriptable = property.FindPropertyRelative("referenceValue");

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, useLocal.boolValue ? 0 : 1, popupOptions, popupStyle);

            useLocal.boolValue = result == 0;

            EditorGUI.PropertyField(position,
                useLocal.boolValue ? localValue : floatScriptable,
                GUIContent.none);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
        #endregion
    }
}