// ------------------------------
// Coldiron Tools
// Original version of the ConditionalEnumHideAttribute created by Brecht Lecluyse (www.brechtos.com)
// Modified By: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using UnityEditor;
using ColdironTools.Scriptables;

namespace ColdironTools.EditorExtensions
{
    /// <summary>
    /// The property drawer responsible for Conditional Hide Attribue behaviour.
    /// </summary>
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
    public class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        PropertyDrawer propertyDrawer = null;

        /// <summary>
        /// Sets up the GUI for the editor.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;

            if (condHAtt.shouldGrayOut || enabled)
            {
                if(propertyDrawer == null) propertyDrawer = PropertyDrawerHelper.GetPropertyDrawer(fieldInfo);

                if (propertyDrawer != null)
                {
                    propertyDrawer.OnGUI(position, property, label);
                }
                else
                    EditorGUI.PropertyField(position, property, label, true);
            }

            GUI.enabled = wasEnabled;
        }

        /// <summary>
        /// Sets the property height for the editor.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

            if (condHAtt.shouldGrayOut || enabled)
            {
                if (propertyDrawer == null) propertyDrawer = PropertyDrawerHelper.GetPropertyDrawer(fieldInfo);

                if (propertyDrawer != null)
                {
                    return propertyDrawer.GetPropertyHeight(property, label);
                }

                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }

        /// <summary>
        /// Gets the value of whether to hide or show the property.
        /// </summary>
        /// <param name="condHAtt"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
        {
            bool enabled = true;
            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
            string conditionPath = propertyPath.Replace(property.name, condHAtt.conditionalSourceField); //changes the path to the conditionalsource property path
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            //Handles Bool Scriptables
            if (sourcePropertyValue.type == "BoolScriptableReference") {
                SerializedProperty useLocal = sourcePropertyValue.serializedObject.FindProperty(conditionPath + ".useLocalValue");
                SerializedProperty local = sourcePropertyValue.serializedObject.FindProperty(conditionPath + ".localValue");
                SerializedProperty reference = sourcePropertyValue.serializedObject.FindProperty(conditionPath + ".referenceValue");
                if (reference.objectReferenceValue != null && reference.objectReferenceValue.GetType() == typeof(BoolScriptable)) {
                    BoolScriptable value = (BoolScriptable) reference.objectReferenceValue;
                    return value.Value;
                } else 
                {
                    return local.boolValue;
                }
            }

            if (sourcePropertyValue != null)
            {
                enabled = condHAtt.shouldUseEnum ? condHAtt.enumVal != sourcePropertyValue.intValue : sourcePropertyValue.boolValue != condHAtt.hideValue;
            }
            else
            {
                Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching Source Property Value found in object: " + condHAtt.conditionalSourceField);
            }

            return enabled;
        }
    }
}