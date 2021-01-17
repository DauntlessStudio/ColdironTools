// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace ColdironTools.Events
{
    /// <summary>
    /// Calls UnityEvents on common Unity Messages, allowing designers to execute simple behaviours without needing to write a new script.
    /// </summary>
    public class EventUtility : MonoBehaviour
    {
        #region Fields
        [SerializeField] private UnityEvent enableEvent = new UnityEvent();
        [SerializeField] private UnityEvent disableEvent = new UnityEvent();
        [SerializeField] private UnityEvent startEvent = new UnityEvent();
        [SerializeField] private UnityEvent destroyEvent = new UnityEvent();
        #endregion

        #region Methods
        /// <summary>
        /// Called when the GameObject becomes active. Invokes the enableEvent.
        /// </summary>
        void OnEnable()
        {
            enableEvent.Invoke();
        }

        /// <summary>
        /// Called when the GameObject becomes inactive. Invoke the disableEvent.
        /// </summary>
        private void OnDisable()
        {
            disableEvent.Invoke();
        }

        /// <summary>
        /// Called on Start. Invokes the startEvent.
        /// </summary>
        void Start()
        {
            startEvent.Invoke();
        }

        /// <summary>
        /// Called when the GameObject is destoyed (including when the level is unloaded). Invokes the destroyEvent.
        /// </summary>
        void OnDestroy()
        {
            destroyEvent.Invoke();
        }
        #endregion
    }
}