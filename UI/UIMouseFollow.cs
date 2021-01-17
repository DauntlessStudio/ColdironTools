// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;

namespace ColdironTools.UI
{
    /// <summary>
    /// Causes this UI object to follow the mouse position.
    /// </summary>
    public class UIMouseFollow : MonoBehaviour
    {
        #region Fields
        [Tooltip("Should this object be following the mouse?")]
        [SerializeField] private bool shouldFollow = true;
        #endregion

        #region Properties
        public bool ShouldFollow { get => shouldFollow; set => shouldFollow = value; }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the transform position to the mouse position.
        /// </summary>
        private void LateUpdate()
        {
            transform.position = Input.mousePosition;
        }
        #endregion
    }
}