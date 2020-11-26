using UnityEngine;

namespace ColdironTools.Scriptables
{
    [CreateAssetMenu(menuName = "Scriptable Variables/Int")]
    public class IntScriptable : ScriptableObject
    {
        #region Fields
        [SerializeField, Multiline] private string designerDescription = "";

        private int defaultValue = 0;
        [SerializeField] private bool shouldReset = true;
        [SerializeField] private int value = 0;

        private event System.EventHandler valueChanged;
        private event System.Action actionValueChanged;
        #endregion

        #region Properties
        public int Value
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

        public void ModifyValue(int val)
        {
            Value += val;
        }

        public void ModifyValue(IntScriptable val)
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

        public static implicit operator int(IntScriptable intScriptable)
        {
            return intScriptable.Value;
        }

        public void RegisterListener(System.EventHandler listener)
        {
            valueChanged += listener;
        }

        public void RegisterListener(System.Action listener)
        {
            actionValueChanged += listener;
        }

        public void UnregisterListener(System.EventHandler listener)
        {
            valueChanged -= listener;
        }

        public void UnregisterListener(System.Action listener)
        {
            actionValueChanged -= listener;
        }

        protected virtual void OnValueChanged()
        {
            valueChanged?.Invoke(this, System.EventArgs.Empty);
            actionValueChanged?.Invoke();
        }
        #endregion
    }
}