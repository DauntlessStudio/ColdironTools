// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using UnityEngine.UI;
using ColdironTools.Scriptables;

namespace ColdironTools.UI
{
    /// <summary>
    /// Keeps slider values consistent.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class UIInitializeSlider : MonoBehaviour
    {
        #region Fields
        [Tooltip("The Slider component to initialize.")]
        [SerializeField] private Slider slider;

        [Tooltip("The value the Slider should be initialized to.")]
        [SerializeField] private FloatScriptableReference value = new FloatScriptableReference();
        #endregion

        #region Methods
        /// <summary>
        /// Attempts to automatically assign the slider.
        /// </summary>
        private void OnValidate()
        {
            if (!slider)
            {
                slider = GetComponentInChildren<Slider>();
            }
        }

        /// <summary>
        /// Calls InitSlider when the GameObject is activated.
        /// </summary>
        private void OnEnable()
        {
            InitSlider();
        }

        /// <summary>
        /// Sets the Slider's value to Value.
        /// </summary>
        public void InitSlider()
        {
            if(slider) slider.value = value;
        }
        #endregion
    }
}