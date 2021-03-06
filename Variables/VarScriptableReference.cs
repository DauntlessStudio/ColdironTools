﻿// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColdironTools.Scriptables
{
    /// <summary>
    /// Contains both a local value and a scriptable reference.
    /// Using a scriptable reference instead of scriptable bool extends greater freedom to designers.
    /// </summary>
    [Serializable]
    public class VarScriptableReference<T>
    {
        #region Fields
        [Tooltip("Should the local value be used?")]
        [SerializeField] protected bool useLocalValue = true;

        [Tooltip("The current local value.")]
        [SerializeField] protected T localValue;

        [Tooltip("The references bool scriptable value.")]
        [SerializeField] protected VarScriptable<T> referenceValue = null;

        protected event EventHandler valueChanged;
        protected event Action actionValueChanged;
        protected List<EventHandler> registeredEvents = new List<EventHandler>();
        protected List<Action> registeredActions = new List<Action>();
        #endregion

        #region Properties
        /// <summary>
        /// Returns false if no reference value is included and logs an error to the console.
        /// Otherwise returns the value for the the scriptable.
        /// </summary>
        protected virtual T nullProtectedReferenceValue
        {
            get
            {
                if (referenceValue == null && !useLocalValue)
                {
                    Debug.LogError("No scriptable reference assigned. Did you mean to use local value?");
                    return default(T);
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

        /// <summary>
        /// Gets either the local value or the protected reference value based on the useLocalValue condition.
        /// Sets these values as well, along with invoking all listeners.
        /// </summary>
        public virtual T Value
        {
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
        /// <summary>
        /// Dafault constructor.
        /// </summary>
        public VarScriptableReference()
        {
            Value = default(T);
        }

        /// <summary>
        /// Construcor with param.
        /// </summary>
        /// <param name="val">Sets the local value</param>
        public VarScriptableReference(T val)
        {
            Value = val;
        }

        /// <summary>
        /// Allows the scriptable to be used as a bool in operators.
        /// </summary>
        /// <param name="scriptableReference"></param>
        public static implicit operator T(VarScriptableReference<T> scriptableReference)
        {
            return scriptableReference.Value;
        }

        /// <summary>
        /// Registers an event as a listener. Whenever Value is changed, all registered listeners will be called.
        /// Prevents duplicates from being registered.
        /// </summary>
        /// <param name="listener">The event to be registered.</param>
        public virtual void RegisterListener(EventHandler listener)
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

        /// <summary>
        /// Registers an action as a listener. Whenever Value is changed, all registered listeners will be called.
        /// Prevents duplicates from being registered.
        /// </summary>
        /// <param name="listener">The action to be registered</param>
        public virtual void RegisterListener(Action listener)
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

        /// <summary>
        /// Unregisters an event as a listener. 
        /// Any registered listeners should be unregistered before the object is destroyed or it will cause a null reference exception.
        /// </summary>
        /// <param name="listener">The event to unregister</param>
        public virtual void UnregisterListener(EventHandler listener)
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

        /// <summary>
        /// Unregisters an event as a listener. 
        /// Any registered listeners should be unregistered before the object is destroyed or it will cause a null reference exception.
        /// </summary>
        /// <param name="listener">The action to unregister</param>
        public virtual void UnregisterListener(Action listener)
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

        /// <summary>
        /// Called any time the Value changes. Invokes all of the listeners.
        /// </summary>
        public virtual void OnValueChanged()
        {
            valueChanged?.Invoke(this, EventArgs.Empty);
            actionValueChanged?.Invoke();
        }
        #endregion
    }
}