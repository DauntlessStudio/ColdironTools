using ColdironTools.Scriptables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColdironTools.Events
{
    #region StructsAndEnum
    public enum RelationalEqualityOperators
    {
        EqualTo, NotEqualTo, GreaterThan, LessThan, GreaterThanOrEqualTo, LessThanOrEqualTo, IsBetween
    }

    [System.Serializable]
    public class ScriptableBoolPairing
    {
        public bool desiredValue;
        public BoolScriptable referencedValue;

        public bool Value
        {
            get
            {
                if (referencedValue == null) return false;
                return desiredValue == referencedValue;
            }
        }

        public static implicit operator bool(ScriptableBoolPairing scriptableBoolPairing)
        {
            return scriptableBoolPairing.Value;
        }
    }

    [System.Serializable]
    public class ScriptableStringPairing
    {
        public string desiredValue;
        public StringScriptable referencedValue;
        public bool isCaseSensitive;

        public bool Value
        {
            get
            {
                if (referencedValue == null) return false;

                string tmpDesiredValue = desiredValue;
                string tmpReferenceValue = referencedValue;

                if (!isCaseSensitive)
                {
                    tmpDesiredValue = tmpDesiredValue.ToLower();
                    tmpReferenceValue = tmpReferenceValue.ToLower();
                }

                return tmpDesiredValue == tmpReferenceValue;
            }
        }

        public static implicit operator bool(ScriptableStringPairing scriptableStringPairing)
        {
            return scriptableStringPairing.Value;
        }
    }

    [System.Serializable]
    public class ScriptableFloatPairing
    {
        [ConditionalHide("useDesiredValue", true)] public float desiredValue;
        [ConditionalHide("useMinMax", true)] public float desiredMinValue;
        [ConditionalHide("useMinMax", true)] public float desiredMaxValue;
        public FloatScriptable referencedValue;
        public RelationalEqualityOperators comparedOperator;
        [HideInInspector, SerializeField] private bool useMinMax;
        [HideInInspector, SerializeField] private bool useDesiredValue;

        public void UpdateInspector()
        {
            useMinMax = (comparedOperator == RelationalEqualityOperators.IsBetween);
            useDesiredValue = (comparedOperator != RelationalEqualityOperators.IsBetween);
        }


        public bool Value
        {
            get
            {
                if (referencedValue == null) return false;

                switch (comparedOperator)
                {
                    case RelationalEqualityOperators.EqualTo:
                        return desiredValue == referencedValue;
                    case RelationalEqualityOperators.NotEqualTo:
                        return desiredValue != referencedValue;
                    case RelationalEqualityOperators.GreaterThan:
                        return desiredValue > referencedValue;
                    case RelationalEqualityOperators.LessThan:
                        return desiredValue < referencedValue;
                    case RelationalEqualityOperators.GreaterThanOrEqualTo:
                        return desiredValue >= referencedValue;
                    case RelationalEqualityOperators.LessThanOrEqualTo:
                        return desiredValue <= referencedValue;
                    case RelationalEqualityOperators.IsBetween:
                        return referencedValue > desiredMinValue && referencedValue < desiredMaxValue;
                    default:
                        return false;
                }
            }
        }

        public static implicit operator bool(ScriptableFloatPairing scriptableFloatPairing)
        {
            return scriptableFloatPairing.Value;
        }
    }

    [System.Serializable]
    public class ScriptableIntPairing
    {
        [ConditionalHide("useDesiredValue", true)] public int desiredValue;
        [ConditionalHide("useMinMax", true)] public float desiredMinValue;
        [ConditionalHide("useMinMax", true)] public float desiredMaxValue;
        public IntScriptable referencedValue;
        public RelationalEqualityOperators comparedOperator;
        [HideInInspector, SerializeField] private bool useMinMax;
        [HideInInspector, SerializeField] private bool useDesiredValue;

        public void UpdateInspector()
        {
            useMinMax = (comparedOperator == RelationalEqualityOperators.IsBetween);
            useDesiredValue = (comparedOperator != RelationalEqualityOperators.IsBetween);
        }

        public bool Value
        {
            get
            {
                if (referencedValue == null) return false;

                switch (comparedOperator)
                {
                    case RelationalEqualityOperators.EqualTo:
                        return desiredValue == referencedValue;
                    case RelationalEqualityOperators.NotEqualTo:
                        return desiredValue != referencedValue;
                    case RelationalEqualityOperators.GreaterThan:
                        return desiredValue > referencedValue;
                    case RelationalEqualityOperators.LessThan:
                        return desiredValue < referencedValue;
                    case RelationalEqualityOperators.GreaterThanOrEqualTo:
                        return desiredValue >= referencedValue;
                    case RelationalEqualityOperators.LessThanOrEqualTo:
                        return desiredValue <= referencedValue;
                    case RelationalEqualityOperators.IsBetween:
                        return referencedValue > desiredMinValue && referencedValue < desiredMaxValue;
                    default:
                        return false;
                }
            }
        }

        public static implicit operator bool(ScriptableIntPairing scriptableIntPairing)
        {
            return scriptableIntPairing.Value;
        }
    }
    #endregion

    public class ConditionalEventListener : MonoBehaviour
    {
        #region Fields
        [SerializeField, Tooltip("Whether all values must be true for it to succeed, or just one")] private bool shouldAllBeTrue = true;
        [SerializeField] private bool isAutomatic = true;

        [Header("Conditions")]
        [SerializeField] private List<ScriptableBoolPairing> boolPairings = new List<ScriptableBoolPairing>();
        [SerializeField] private List<ScriptableStringPairing> stringPairings = new List<ScriptableStringPairing>();
        [SerializeField] private List<ScriptableFloatPairing> floatPairings = new List<ScriptableFloatPairing>();
        [SerializeField] private List<ScriptableIntPairing> intPairings = new List<ScriptableIntPairing>();

        [Header("")]
        [SerializeField] private UnityEvent SuccessEvent = new UnityEvent();
        [SerializeField] private UnityEvent FailEvent = new UnityEvent();
        #endregion

        #region Methods
        private void Awake()
        {
            if (!isAutomatic) return;

            foreach (ScriptableBoolPairing pairing in boolPairings)
            {
                pairing.referencedValue.RegisterListener(OnValueChanged);
            }

            foreach (ScriptableStringPairing pairing in stringPairings)
            {
                pairing.referencedValue.RegisterListener(OnValueChanged);
            }

            foreach (ScriptableFloatPairing pairing in floatPairings)
            {
                pairing.referencedValue.RegisterListener(OnValueChanged);
            }

            foreach (ScriptableIntPairing pairing in intPairings)
            {
                pairing.referencedValue.RegisterListener(OnValueChanged);
            }
        }

        private void OnValidate()
        {
            foreach (ScriptableFloatPairing pairing in floatPairings)
            {
                pairing.UpdateInspector();
            }

            foreach(ScriptableIntPairing pairing in intPairings)
            {
                pairing.UpdateInspector();
            }
        }

        private void OnEnable()
        {
            if(isAutomatic) AttemptInvoke();
        }

        private void OnDestroy()
        {
            if (!isAutomatic) return;

            foreach (ScriptableBoolPairing pairing in boolPairings)
            {
                pairing.referencedValue.UnregisterListener(OnValueChanged);
            }

            foreach (ScriptableStringPairing pairing in stringPairings)
            {
                pairing.referencedValue.UnregisterListener(OnValueChanged);
            }

            foreach (ScriptableFloatPairing pairing in floatPairings)
            {
                pairing.referencedValue.UnregisterListener(OnValueChanged);
            }

            foreach (ScriptableIntPairing pairing in intPairings)
            {
                pairing.referencedValue.UnregisterListener(OnValueChanged);
            }
        }

        private void OnValueChanged(object sender, System.EventArgs e)
        {
            AttemptInvoke();
        }

        public void AttemptInvoke()
        {
            if (CheckConditions())
            {
                SuccessEvent.Invoke();
            }
            else
            {
                FailEvent.Invoke();
            }
        }

        private bool CheckConditions()
        {
            foreach (ScriptableBoolPairing pairing in boolPairings)
            {
                if (!pairing)
                {
                    if (shouldAllBeTrue) return false;
                }
                else
                {
                    if(!shouldAllBeTrue) return true;
                }
            }

            foreach (ScriptableStringPairing pairing in stringPairings)
            {
                if (!pairing)
                {
                    if (shouldAllBeTrue) return false;
                }
                else
                {
                    if (!shouldAllBeTrue) return true;
                }
            }

            foreach (ScriptableFloatPairing pairing in floatPairings)
            {
                if (!pairing)
                {
                    if (shouldAllBeTrue) return false;
                }
                else
                {
                    if (!shouldAllBeTrue) return true;
                }
            }

            foreach (ScriptableIntPairing pairing in intPairings)
            {
                if (!pairing)
                {
                    if (shouldAllBeTrue) return false;
                }
                else
                {
                    if (!shouldAllBeTrue) return true;
                }
            }

            return shouldAllBeTrue;
        }
        #endregion
    }
}