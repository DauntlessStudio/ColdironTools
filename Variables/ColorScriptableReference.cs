using UnityEngine;
using System;
using System.Collections.Generic;

namespace ColdironTools.Scriptables
{
    [Serializable]
    public class ColorScriptableReference
    {
        #region Fields
        [SerializeField] private bool useLocalValue = true;
        [SerializeField] Color localValue = new Color(1.0f, 1.0f, 1.0f);
        [SerializeField] ColorScriptable referenceValue = null;

        private event EventHandler valueChanged;
        private event Action actionValueChanged;
        private List<EventHandler> registeredEvents = new List<EventHandler>();
        private List<Action> registeredActions = new List<Action>();
        #endregion

        #region Properties
        private Color nullProtectedReferenceValue
        {
            get
            {
                if (referenceValue == null && !useLocalValue)
                {
                    Debug.LogError("No scriptable reference assigned. Did you mean to use local value?");
                    return Color.black;
                }

                return referenceValue;
            }
            set
            {
                if (referenceValue == null && !useLocalValue)
                {
                    Debug.LogError("No scriptable reference assigned. Did you mean to use local value?");
                    return;
                }

                referenceValue.Value = value;
            }
        }

        public Color Value
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

                OnValueChanged();
            }
        }
        #endregion

        #region Methods
        public ColorScriptableReference()
        {
        }

        public ColorScriptableReference(Color val)
        {
            Value = val;
        }

        public static implicit operator Color(ColorScriptableReference scriptableReference)
        {
            return scriptableReference.Value;
        }

        public void RegisterListener(EventHandler listener)
        {
            if (registeredEvents.Contains(listener)) return;

            if (useLocalValue)
            {
                valueChanged += listener;
            }
            else
            {
                referenceValue.RegisterListener(listener);
            }

            registeredEvents.Add(listener);
        }

        public void RegisterListener(Action listener)
        {
            if (registeredActions.Contains(listener)) return;

            if (useLocalValue)
            {
                actionValueChanged += listener;
            }
            else
            {
                referenceValue.RegisterListener(listener);
            }

            registeredActions.Add(listener);
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

            registeredEvents.Remove(listener);
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

            registeredActions.Remove(listener);
        }

        public void OnValueChanged()
        {
            valueChanged?.Invoke(this, EventArgs.Empty);
            actionValueChanged?.Invoke();
        }

        public ColorScriptable ReferenceValue
        {
            get => referenceValue;
            set
            {
                referenceValue = value;
                useLocalValue = false;
            }
        }
        #endregion
    }
}