using UnityEngine;
using System;

namespace ColdironTools.Scriptables
{
    [Serializable]
    public class FloatScriptableReference
    {
        #region Fields
        [SerializeField] private bool useLocalValue = true;
        [SerializeField] float localValue = 0.0f;
        [SerializeField] FloatScriptable referenceValue = null;
        private event EventHandler valueChanged;
        private event Action actionValueChanged;
        #endregion

        #region Properties
        private float nullProtectedReferenceValue
        {
            get
            {
                if (!referenceValue && !useLocalValue)
                {
                    Debug.LogError("No scriptable reference assigned. Did you mean to use local value?");
                    return 0f;
                }

                return referenceValue;
            }
            set
            {
                if (!referenceValue && !useLocalValue)
                {
                    Debug.LogError("No scriptable reference assigned. Did you mean to use local value?");
                    return;
                }

                referenceValue.Value = value;
            }
        }

        public float Value
        {
            //gets and sets either the local value or the scriptable's value based on useLocalValue
            get
            {
                return useLocalValue ? localValue : nullProtectedReferenceValue;
            }
            set
            {
                if (useLocalValue)
                {
                    localValue = value;
                }
                else
                {
                    nullProtectedReferenceValue = value;
                }

                OnValueChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Methods
        public static implicit operator float(FloatScriptableReference scriptableReference)
        {
            return scriptableReference.Value;
        }

        public void RegisterListener(EventHandler listener)
        {
            if (useLocalValue)
            {
                valueChanged += listener;
            }
            else
            {
                referenceValue.RegisterListener(listener);
            }
        }

        public void RegisterListener(Action listener)
        {
            if (useLocalValue)
            {
                actionValueChanged += listener;
            }
            else
            {
                referenceValue.RegisterListener(listener);
            }
        }

        public void UnregisterListener(EventHandler listener)
        {
            if (useLocalValue)
            {
                valueChanged -= listener;
            }
            else
            {
                referenceValue.UnregisterListener(listener);
            }
        }

        public void UnregisterListener(Action listener)
        {
            if (useLocalValue)
            {
                actionValueChanged -= listener;
            }
            else
            {
                referenceValue.UnregisterListener(listener);
            }
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            valueChanged?.Invoke(this, EventArgs.Empty);
            actionValueChanged?.Invoke();
        }
        #endregion
    }
}