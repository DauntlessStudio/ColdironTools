using UnityEngine;
using System;

namespace ColdironTools.Scriptables
{
    [Serializable]
    public class IntScriptableReference
    {
        #region Fields
        [SerializeField] private bool useLocalValue = true;
        [SerializeField] private int localValue = 0;
        [SerializeField] private IntScriptable referenceValue = null;
        private event EventHandler valueChanged;
        private event Action actionValueChanged;
        #endregion

        #region Properties
        private int nullProtectedReferenceValue
        {
            get
            {
                if (!referenceValue && !useLocalValue)
                {
                    Debug.LogError("No scriptable reference assigned. Did you mean to use local value?");
                    return 0;
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

        public int Value
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

                OnValueChanged(null, EventArgs.Empty);
            }
        }
        #endregion

        #region Methods
        public static implicit operator int(IntScriptableReference scriptableReference)
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