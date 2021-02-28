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
    /// Using a scriptable reference instead of scriptable bool extends greater freedom to designers.
    /// </summary>
    [Serializable]
    public class BoolScriptableReference : VarScriptableReference<bool>
    {
        #region Fields
        /// <summary>
        /// Dafault constructor.
        /// </summary>
        public BoolScriptableReference()
        {
            Value = false;
        }

        /// <summary>
        /// Construcor with param.
        /// </summary>
        /// <param name="val">Sets the local value</param>
        public BoolScriptableReference(bool val)
        {
            Value = val;
        }
        #endregion
    }
}