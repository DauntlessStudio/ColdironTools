using UnityEngine;
using UnityEditor;

namespace ColdironTools.Scriptables
{
    [CustomPropertyDrawer(typeof(BoolScriptableReference))]
    public class BoolScriptableReferenceDrawer : PropertyDrawer
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
            SerializedProperty boolScriptable = property.FindPropertyRelative("referenceValue");

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
                useLocal.boolValue ? localValue : boolScriptable,
                GUIContent.none);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
        #endregion
    }
}