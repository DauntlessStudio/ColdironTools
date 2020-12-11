using UnityEngine;
using System.Collections.Generic;

namespace ColdironTools.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptable Variables/Float")]
    public class FloatScriptable : ScriptableObject
    {
        #region Fields
        [SerializeField, Multiline] private string designerDescription = "";

        [SerializeField] private bool shouldReset = true;
        [SerializeField] private float value = 0.0f;
        private float defaultValue = 0.0f;

        private event System.EventHandler valueChanged;
        private event System.Action actionValueChanged;
        private List<System.EventHandler> registeredEvents = new List<System.EventHandler>();
        private List<System.Action> registeredActions = new List<System.Action>();
        #endregion

        #region Properties
        public float Value
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

        public string DesignerDescription { get => designerDescription;}
        #endregion

        #region Methods
        private void OnValidate()
        {
            OnValueChanged();

            if (!Application.isPlaying && Application.platform == RuntimePlatform.WindowsEditor)
            {
                Init();
            }
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public void Init()
        {
            if (shouldReset) defaultValue = Value;
        }

        public void Reset()
        {
            if (shouldReset) Value = defaultValue;
        }

        public void ModifyValue(float val)
        {
            Value += val;
        }

        public void ModifyValue(FloatScriptable val)
        {
            Value += val;
        }

        public void IncrementToLimitWithCycle(int maxValue)
        {
            Value++;

            if (value > maxValue)
            {
                Value = 0;
            }
        }

        public void IncrementToLimit(int maxValue)
        {
            Value++;

            if (Value > maxValue)
            {
                Value = maxValue;
            }
        }

        public static implicit operator float(FloatScriptable floatScriptable)
        {
            return floatScriptable.Value;
        }

        public void RegisterListener(System.EventHandler listener)
        {
            if (registeredEvents.Contains(listener)) return;

            valueChanged += listener;

            registeredEvents.Add(listener);
        }

        public void RegisterListener(System.Action listener)
        {
            if (registeredActions.Contains(listener)) return;

            actionValueChanged += listener;

            registeredActions.Add(listener);
        }

        public void UnregisterListener(System.EventHandler listener)
        {
            valueChanged -= listener;

            registeredEvents.Remove(listener);
        }

        public void UnregisterListener(System.Action listener)
        {
            actionValueChanged -= listener;

            registeredActions.Remove(listener);
        }

        protected virtual void OnValueChanged()
        {
            valueChanged?.Invoke(this, System.EventArgs.Empty);
            actionValueChanged?.Invoke();
        }
        #endregion
    }
}