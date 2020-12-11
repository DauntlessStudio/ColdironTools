using UnityEngine;
using System.Collections.Generic;

namespace ColdironTools.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptable Variables/String")]
    public class StringScriptable : ScriptableObject
    {
        #region Fields
        [SerializeField, Multiline] private string designerDescription = "";

        [SerializeField] private bool shouldReset = true;
        [SerializeField] private string value = "";
        private string defaultValue = "";

        private event System.EventHandler valueChanged;
        private event System.Action actionValueChanged;
        private List<System.EventHandler> registeredEvents = new List<System.EventHandler>();
        private List<System.Action> registeredActions = new List<System.Action>();
        #endregion

        #region Properties
        public string Value
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
            if(shouldReset) defaultValue = Value;
        }

        public void Reset()
        {
            if(shouldReset) Value = defaultValue;
        }

        public void AddString(string val)
        {
            Value = Value + val;
        }

        public void AddString(StringScriptableReference val)
        {
            Value = Value + val;
        }

        public void RemoveCharacters(int characters)
        {
            for (int i = 0; i < characters; i++)
            {
                if (Value.Length > 0)
                {
                    Value = Value.Remove(Value.Length - 1);
                }
            }
        }

        public static implicit operator string(StringScriptable stringScriptable)
        {
            return stringScriptable.Value;
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