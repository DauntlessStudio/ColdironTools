// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;

namespace ColdironTools.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptable Variables/String")]
    public class StringScriptable : VarScriptable<string>
    {
        #region Methods
        /// <summary>
        /// Adds to the end of the current string.
        /// </summary>
        /// <param name="val">The string to be appended</param>
        public string Append(string val)
        {
            return Value = Value + val;
        }
        #endregion
    }
}