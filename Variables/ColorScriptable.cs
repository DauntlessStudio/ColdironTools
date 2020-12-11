using UnityEngine;
using System.Collections.Generic;

namespace ColdironTools.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptable Variables/Color")]
    public class ColorScriptable : ScriptableObject
    {
        #region Fields
        [SerializeField, Multiline] private string designerDescription = "";

        private Color defaultValue = Color.white;
        [SerializeField] private bool shouldReset = true;
        [SerializeField] private Color value = Color.white;

        private event System.EventHandler valueChanged;
        private event System.Action actionValueChanged;
        private List<System.EventHandler> registeredEvents = new List<System.EventHandler>();
        private List<System.Action> registeredActions = new List<System.Action>();
        #endregion

        #region Properties
        public Color Value
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

        public string DesignerDescription { get => designerDescription; }
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

        public static implicit operator Color(ColorScriptable colorScriptable)
        {
            return colorScriptable.Value;
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