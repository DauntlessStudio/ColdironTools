// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System;

namespace ColdironTools.Scriptables
{
    /// <summary>
    /// Contains both a local value and a scriptable reference.
    /// Using a scriptable reference instead of a standard scriptable extends greater freedom to designers.
    /// </summary>
    [Serializable]
    public class ColorScriptableReference : VarScriptableReference<UnityEngine.Color>
    {
        #region Fields
        /// <summary>
        /// Dafault constructor.
        /// </summary>
        public ColorScriptableReference()
        {
            Value = new UnityEngine.Color();
        }

        /// <summary>
        /// Construcor with param.
        /// </summary>
        /// <param name="val">Sets the local value</param>
        public ColorScriptableReference(UnityEngine.Color val)
        {
            Value = val;
        }
        #endregion
    }
}