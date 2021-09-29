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
    /// Scriptable object containing a float value.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Variables/Float"), System.Serializable]
    public class FloatScriptable : VarScriptable<float>
    {
        #region Fields
        [Tooltip("Should put a minimum and maximum on this value.")]
        [SerializeField] private bool useMinMax = false;

        [ConditionalHide("useMinMax")]
        [Tooltip("The lowest the value can be.")]
        [SerializeField] private FloatScriptableReference minValue = new FloatScriptableReference();

        [ConditionalHide("useMinMax")]
        [Tooltip("The highest the value can be.")]
        [SerializeField] private FloatScriptableReference maxValue = new FloatScriptableReference();
        #endregion

        #region Properties
        /// <summary>
        /// The current value of this scriptable.
        /// Calls the registered listeners when changed.
        /// </summary>
        public override float Value
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
        public void ModifyValue(float val)
        {
            Value += val;
        }
        #endregion
    }
}