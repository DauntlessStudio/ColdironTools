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
    public class StringScriptableReference : VarScriptableReference<string>
    {
        #region Fields
        /// <summary>
        /// Dafault constructor.
        /// </summary>
        public StringScriptableReference()
        {
            Value = "";
        }

        /// <summary>
        /// Construcor with param.
        /// </summary>
        /// <param name="val">Sets the local value</param>
        public StringScriptableReference(string val)
        {
            Value = val;
        }
        #endregion
    }
}