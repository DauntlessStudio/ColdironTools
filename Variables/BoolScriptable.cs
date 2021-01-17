// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace ColdironTools.Scriptables
{
    /// <summary>
    /// Scriptable object containing a boolean value.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Variables/Bool"), System.Serializable]
    public class BoolScriptable : ScriptableObject
    {
        #region Fields
        [Tooltip("A note by the designer describing the purpose of this scriptable. Not used in code.")]
        [SerializeField, Multiline] private string designerDescription = "";

        [Tooltip("Should the value reset when exiting play mode?")]
        [SerializeField] protected bool shouldReset = true;

        [Tooltip("Current value of the scriptable.")]
        [SerializeField] protected bool value = false;

        protected bool defaultValue = false;

        private event System.EventHandler valueChanged;
        private event System.Action actionValueChanged;
        private List<System.EventHandler> registeredEvents = new List<System.EventHandler>();
        private List<System.Action> registeredActions = new List<System.Action>();
        #endregion

        #region Properties
        /// <summary>
        /// The current value of this scriptable.
        /// Calls the registered listeners when changed.
        /// </summary>
        public virtual bool Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Public accessor for the description. Exists mainly to remove the unused variable warning in the editor.
        /// </summary>
        public string DesignerDescription { get => designerDescription; }
        #endregion

        #region Methods
        /// <summary>
        /// Calls OnValueChanged even when directly modifying the field in the editor.
        /// </summary>
        protected virtual void OnValidate()
        {
            OnValueChanged();

            if (!Application.isPlaying && Application.platform == RuntimePlatform.WindowsEditor)
            {
                Init();
            }
        }

        /// <summary>
        /// Prevents this object from unloading when new scenes are loaded.
        /// </summary>
        protected virtual void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        /// <summary>
        /// Sets default value to whatever a designer inputs in the inspector.
        /// </summary>
        public virtual void Init()
        {
            if (shouldReset) defaultValue = Value;
        }

        /// <summary>
        /// Resets to the default value. Called automatically by ScriptableResetter when exiting play mode.
        /// </summary>
        public virtual void Reset()
        {
            if (shouldReset) value = defaultValue;
        }

        /// <summary>
        /// Inverts the bool value.
        /// </summary>
        public virtual void InvertValue()
        {
            Value = !Value;
        }

        /// <summary>
        /// Allows the scriptable to be used as a bool in operators.
        /// </summary>
        /// <param name="boolScriptable"></param>
        public static implicit operator bool(BoolScriptable boolScriptable)
        {
            if(boolScriptable == null) return false;
            return boolScriptable.Value;
        }

        /// <summary>
        /// Registers an event as a listener. Whenever Value is changed, all registered listeners will be called.
        /// Prevents duplicates from being registered.
        /// </summary>
        /// <param name="listener">The event to be registered.</param>
        public void RegisterListener(System.EventHandler listener)
        {
            if(registeredEvents.Contains(listener)) return;

            valueChanged += listener;

            registeredEvents.Add(listener);
        }

        /// <summary>
        /// Registers an action as a listener. Whenever Value is changed, all registered listeners will be called.
        /// Prevents duplicates from being registered.
        /// </summary>
        /// <param name="listener">The action to be registered</param>
        public void RegisterListener(System.Action listener)
        {
            if(registeredActions.Contains(listener)) return;

            actionValueChanged += listener;

            registeredActions.Add(listener);
        }

        /// <summary>
        /// Unregisters an event as a listener. 
        /// Any registered listeners should be unregistered before the object is destroyed or it will cause a null reference exception.
        /// </summary>
        /// <param name="listener">The event to unregister</param>
        public void UnregisterListener(System.EventHandler listener)
        {
            valueChanged -= listener;

            registeredEvents.Remove(listener);
        }

        /// <summary>
        /// Unregisters an event as a listener. 
        /// Any registered listeners should be unregistered before the object is destroyed or it will cause a null reference exception.
        /// </summary>
        /// <param name="listener">The action to unregister</param>
        public void UnregisterListener(System.Action listener)
        {
            actionValueChanged -= listener;

            registeredActions.Remove(listener);
        }

        /// <summary>
        /// Called any time the Value changes. Invokes all of the listeners.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            valueChanged?.Invoke(this, System.EventArgs.Empty);
            actionValueChanged?.Invoke();
        }
        #endregion
    }
}