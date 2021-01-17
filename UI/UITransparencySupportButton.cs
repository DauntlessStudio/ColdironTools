// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine.UI;

namespace ColdironTools.UI
{
    /// <summary>
    /// Simple class to allow buttons to only work when clicking on opaque pixels.
    /// </summary>
    public class UITransparencySupportButton : Button
    {
        #region Methods
        /// <summary>
        /// Adds the alpha hit test call to OnEnable.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            AlphaHitTest.AttemptTransparencyHitTest(gameObject);
        }
        #endregion
    }
}