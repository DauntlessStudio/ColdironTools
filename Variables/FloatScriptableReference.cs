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
    public class FloatScriptableReference : VarScriptableReference<float>
    {
        #region Fields
        /// <summary>
        /// Dafault constructor.
        /// </summary>
        public FloatScriptableReference()
        {
            Value = 0.0f;
        }

        /// <summary>
        /// Construcor with param.
        /// </summary>
        /// <param name="val">Sets the local value</param>
        public FloatScriptableReference(float val)
        {
            Value = val;
        }
        #endregion
    }
}