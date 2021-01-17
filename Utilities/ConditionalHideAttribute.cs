// ------------------------------
// Coldiron Tools
// Original version of the ConditionalEnumHideAttribute created by Brecht Lecluyse (www.brechtos.com)
// Modified By: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using System;

namespace ColdironTools.EditorExtensions
{
    /// <summary>
    /// Allows fields to be hidden in the inspector based on a condition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        public string conditionalSourceField = "";
        public bool shouldGrayOut = false;
        public bool hideValue = false;

        public int enumVal = 0;
        public bool shouldUseEnum = false;

        /// <summary>
        /// Hides this field in the inspector if the source field is false.
        /// </summary>
        /// <param name="conditionalSourceField">The name of the field that should be evaluated.</param>
        /// <param name="hideValue">The bool value desired to hide the field.</param>
        /// <param name="shouldGrayOut">Should the field be grayed out or hidden?</param>
        public ConditionalHideAttribute(string conditionalSourceField, bool hideValue = false, bool shouldGrayOut = false)
        {
            this.conditionalSourceField = conditionalSourceField;
            this.shouldGrayOut = shouldGrayOut;
            this.hideValue = hideValue;
        }

        /// <summary>
        /// Hides this field in the inspector based on an enum value.
        /// </summary>
        /// <param name="conditionalSourceField">The name of the field that should be evaluated.</param>
        /// <param name="enumVal">Should the field be grayed out or hidden?</param>
        /// <param name="shouldGrayOut">The bool value desired to hide the field.</param>
        public ConditionalHideAttribute(string conditionalSourceField, int enumVal, bool shouldGrayOut = false)
        {
            this.conditionalSourceField = conditionalSourceField;
            this.shouldGrayOut = shouldGrayOut;
            this.enumVal = enumVal;
            shouldUseEnum = true;
        }
    }
}