// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;

namespace ColdironTools.EditorExtensions
{
    /// <summary>
    /// Helper class for property drawers. Allows custom property drawers to be displayed by other custom property drawers.
    /// </summary>
    public static class PropertyDrawerHelper
    {
        #region Fields
        private static Dictionary<Type, PropertyDrawer> customPropDrawers = new Dictionary<Type, PropertyDrawer>();
        #endregion

        #region Methods
        /// <summary>
        /// Attempts to get a chached custom property drawer.
        /// </summary>
        /// <param name="fieldInfo">The field info from the property drawer making the call.</param>
        /// <returns>The custom property drawer for the type. Returns null if a standard property field would be used</returns>
        public static PropertyDrawer GetPropertyDrawer(FieldInfo fieldInfo)
        {
            Type propType = fieldInfo.FieldType;
            if (!customPropDrawers.ContainsKey(propType))
            {
                customPropDrawers.Add(propType, FindPropertyDrawerFromType(fieldInfo));
            }

            return customPropDrawers[propType];
        }

        /// <summary>
        /// Finds the property drawer from a type.
        /// </summary>
        /// <param name="fieldInfo">The field info containing the type</param>
        /// <returns>A custom property drawer. Can be null</returns>
        public static PropertyDrawer FindPropertyDrawerFromType(FieldInfo fieldInfo)
        {
            Type fieldType = fieldInfo.FieldType;

            Type propertyDrawerType = (Type)Type.GetType("UnityEditor.ScriptAttributeUtility,UnityEditor")
                .GetMethod("GetDrawerTypeForType", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Invoke(null, new object[] { fieldType });

            PropertyDrawer propertyDrawer = null;
            if (typeof(PropertyDrawer).IsAssignableFrom(propertyDrawerType))
                propertyDrawer = (PropertyDrawer)Activator.CreateInstance(propertyDrawerType);

            if (propertyDrawer != null)
            {
                typeof(PropertyDrawer)
                    .GetField("m_FieldInfo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .SetValue(propertyDrawer, fieldInfo);
            }

            return propertyDrawer;
        }
        #endregion
    }
}