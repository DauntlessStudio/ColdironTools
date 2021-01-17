// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System.Collections;
using UnityEngine;
using ColdironTools.Scriptables;

namespace ColdironTools.Utilities
{
    /// <summary>
    /// Increments a value over time.
    /// </summary>
    public class ValueCounter : MonoBehaviour
    {
        #region Fields
        [Tooltip("Float scriptable to be incremented.")]
        [SerializeField] private FloatScriptable value = null;

        [Tooltip("Maximum the value can increment to.")]
        [SerializeField] private FloatScriptableReference maxValue = new FloatScriptableReference(10.0f);

        [Tooltip("Float that Value is set to when cycling.")]
        [SerializeField] private FloatScriptableReference minValue = new FloatScriptableReference(0.0f);

        [Tooltip("Amount by which Value should increment.")]
        [SerializeField] private FloatScriptableReference incrementValue = new FloatScriptableReference(1.0f);

        [Tooltip("Time in seconds between increments.")]
        [SerializeField] private FloatScriptableReference updateFrequency = new FloatScriptableReference(1.0f);

        [Tooltip("Should the counter be on?")]
        [SerializeField] private BoolScriptableReference shouldUpdate = new BoolScriptableReference(true);

        [Tooltip("Should the value loop between min and max?")]
        [SerializeField] private bool shouldCycle = false;

        private bool isMaxCycle = false;
        #endregion

        #region Methods
        /// <summary>
        /// Starts the counter when activated.
        /// </summary>
        private void OnEnable()
        {
            StartCoroutine(UpdateCount());
        }
        
        /// <summary>
        /// Stops the counter when deactiveated.
        /// </summary>
        private void OnDisable()
        {
            StopCoroutine(UpdateCount());
        }

        /// <summary>
        /// Calls Increment value every updateFrequency.
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateCount()
        {
            while (true)
            {
                yield return new WaitForSeconds(updateFrequency);
                if (shouldUpdate) IncrementValue();
            }
        }

        /// <summary>
        /// Increments the value, looping if needed.
        /// </summary>
        private void IncrementValue()
        {
            value.Value = Mathf.Clamp(value + incrementValue, minValue, maxValue);

            if (value >= maxValue && shouldCycle)
            {
                if (isMaxCycle)
                {
                    value.Value = minValue;
                    isMaxCycle = false;
                }
                else
                {
                    isMaxCycle = true;
                }
            }
        }

        /// <summary>
        /// Sets value to minimum.
        /// </summary>
        public void ResetValue()
        {
            value.Value = minValue;
            isMaxCycle = false;
        }
        #endregion
    }
}