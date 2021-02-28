// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;

namespace ColdironTools.Scriptables
{
    /// <summary>
    /// Scriptable object containing a boolean value.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Variables/Bool"), System.Serializable]
    public class BoolScriptable : VarScriptable<bool>
    {
        /// <summary>
        /// Inverts the bool value.
        /// </summary>
        public virtual void InvertValue()
        {
            Value = !Value;
        }
    }
}