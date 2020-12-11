using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColdironTools.Scriptables
{
    [Serializable]
    struct DateInt
    {
        public int month, day, year;

        public DateInt(int m, int d, int y)
        {
            month = m;
            day = d;
            year = y;
        }
    }

    [CreateAssetMenu(menuName = "Scriptable Variables/Date")]
    public class DateScriptable : ScriptableObject
    {
        #region Fields
        [SerializeField, Multiline] private string designerDescription = "";

        private DateTime defaultValue = new DateTime();
        [SerializeField] private bool shouldReset = true;
        [SerializeField] private DateInt value = new DateInt(0, 0, 0);

        private event EventHandler valueChanged;
        private event Action actionValueChanged;
        private List<EventHandler> registeredEvents = new List<EventHandler>();
        private List<Action> registeredActions = new List<Action>();
        #endregion

        #region Properties
        public DateTime Value
        {
            get
            {
                return ReadDate();
            }
            set
            {
                WriteDate(value);
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

        private DateTime ReadDate()
        {
            if(value.day == 0)
            {
                return new DateTime(1, 1, 1);
            }else
                return new DateTime(value.year, value.month, value.day);
        }

        private void WriteDate(DateTime val)
        {
            value = new DateInt(val.Month, val.Day, val.Year);
        }

        public static implicit operator DateTime(DateScriptable dateScriptable)
        {
            return dateScriptable.Value;
        }

        public override string ToString()
        {
            return ReadDate().ToString();
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