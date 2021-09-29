// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using ColdironTools.EditorExtensions;

namespace ColdironTools.Scriptables
{   
    /// <summary>
    /// Scriptable object containing an int value.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Variables/Int")]
    public class IntScriptable : VarScriptable<int>
    {
        #region Fields
        [Tooltip("Should put a minimum and maximum on this value.")]
        [SerializeField] private bool useMinMax = false;

        [ConditionalHide("useMinMax")]
        [Tooltip("The lowest the value can be.")]
        [SerializeField] private IntScriptableReference minValue = new IntScriptableReference();

        [ConditionalHide("useMinMax")]
        [Tooltip("The highest the value can be.")]
        [SerializeField] private IntScriptableReference maxValue = new IntScriptableReference();
        #endregion

        #region Properties
        /// <summary>
        /// The current value of this scriptable.
        /// Calls the registered listeners when changed.
        /// </summary>
        public override int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = useMinMax ? Mathf.Clamp(value, minValue, maxValue) : value;
                OnValueChanged();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calls OnValueChanged even when directly modifying the field in the editor.
        /// </summary>
        protected override void OnValidate()
        {
            OnValueChanged();

            if (!Application.isPlaying && Application.platform == RuntimePlatform.WindowsEditor)
            {
                Init();
            }

            if (useMinMax)
            {
                value = Mathf.Clamp(value, minValue, maxValue);
            }
        }

        /// <summary>
        /// Adds the parameter to the scriptable's value. Use negative numbers to subtract.
        /// </summary>
        /// <param name="val">The value to increment by</param>
        public void ModifyValue(int val)
        {
            Value += val;
        }
        #endregion
    }
}