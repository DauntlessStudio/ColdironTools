// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using ColdironTools.Scriptables;
using ColdironTools.EditorExtensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColdironTools.Events
{
    #region StructsAndEnum
    /// <summary>
    /// Enumerates equality operators. Used in switch statements.
    /// </summary>
    public enum EqualityOperators
    {
        EqualTo, NotEqualTo, GreaterThan, LessThan, GreaterThanOrEqualTo, LessThanOrEqualTo, IsBetween
    }

    /// <summary>
    /// Evaluates a desired Bool value against a reference Bool Scriptable.
    /// </summary>
    [System.Serializable]
    public class ScriptableBoolPairing
    {
        public bool desiredValue;
        public BoolScriptable referencedValue;

        /// <summary>
        /// Returns false if the Referenced Value is null, or it does not match the Desired Value. Otherwise returns true.
        /// </summary>
        public bool Value
        {
            get
            {
                if (referencedValue == null) return false;
                return desiredValue == referencedValue;
            }
        }

        /// <summary>
        /// Allows this class to evaluate as a Bool, using the Value property.
        /// </summary>
        /// <param name="scriptableBoolPairing"></param>
        public static implicit operator bool(ScriptableBoolPairing scriptableBoolPairing)
        {
            return scriptableBoolPairing.Value;
        }
    }

    /// <summary>
    /// Evaluates a desired String against a referenced String Scriptable.
    /// </summary>
    [System.Serializable]
    public class ScriptableStringPairing
    {
        public string desiredValue;
        public StringScriptable referencedValue;
        public bool isCaseSensitive;

        /// <summary>
        /// Returns false if the Referenced Value is null, or it does not match the Desired Value. Otherwise returns true.
        /// </summary>
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

        /// <summary>
        /// Allows this class to evaluate as a Bool, using the Value property.
        /// </summary>
        /// <param name="scriptableStringPairing"></param>
        public static implicit operator bool(ScriptableStringPairing scriptableStringPairing)
        {
            return scriptableStringPairing.Value;
        }
    }

    /// <summary>
    /// Evaluates a desired Float against a referenced Float Scriptable using an Equality Operator Enum.
    /// </summary>
    [System.Serializable]
    public class ScriptableFloatPairing
    {
        [ConditionalHide("useDesiredValue")] public float desiredValue;
        [ConditionalHide("useMinMax")] public float desiredMinValue;
        [ConditionalHide("useMinMax")] public float desiredMaxValue;

        public FloatScriptable referencedValue;
        public EqualityOperators comparedOperator;

        [HideInInspector, SerializeField] private bool useMinMax;
        [HideInInspector, SerializeField] private bool useDesiredValue;

        /// <summary>
        /// Uses the Conditional Hide editor scripts to determine which values to show in the Inspector.
        /// </summary>
        public void UpdateInspector()
        {
            useMinMax = (comparedOperator == EqualityOperators.IsBetween);
            useDesiredValue = (comparedOperator != EqualityOperators.IsBetween);
        }

        /// <summary>
        /// Returns false if the Referenced Value is null, or it does evalute to the the Desired Value using the Equality Operator. Otherwise returns true.
        /// </summary>
        public bool Value
        {
            get
            {
                if (referencedValue == null) return false;

                switch (comparedOperator)
                {
                    case EqualityOperators.EqualTo:
                        return desiredValue == referencedValue;
                    case EqualityOperators.NotEqualTo:
                        return desiredValue != referencedValue;
                    case EqualityOperators.GreaterThan:
                        return desiredValue > referencedValue;
                    case EqualityOperators.LessThan:
                        return desiredValue < referencedValue;
                    case EqualityOperators.GreaterThanOrEqualTo:
                        return desiredValue >= referencedValue;
                    case EqualityOperators.LessThanOrEqualTo:
                        return desiredValue <= referencedValue;
                    case EqualityOperators.IsBetween:
                        return referencedValue > desiredMinValue && referencedValue < desiredMaxValue;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Allows this class to evaluate as a Bool, using the Value property.
        /// </summary>
        /// <param name="scriptableFloatPairing"></param>
        public static implicit operator bool(ScriptableFloatPairing scriptableFloatPairing)
        {
            return scriptableFloatPairing.Value;
        }
    }


    /// <summary>
    /// Evaluates a desired Float against a referenced Float Scriptable using an Equality Operator Enum.
    /// </summary>
    [System.Serializable]
    public class ScriptableIntPairing
    {
        [ConditionalHide("useDesiredValue")] public int desiredValue;
        [ConditionalHide("useMinMax")] public float desiredMinValue;
        [ConditionalHide("useMinMax")] public float desiredMaxValue;

        public IntScriptable referencedValue;
        public EqualityOperators comparedOperator;

        [HideInInspector, SerializeField] private bool useMinMax;
        [HideInInspector, SerializeField] private bool useDesiredValue;

        /// <summary>
        /// Uses the Conditional Hide editor scripts to determine which values to show in the Inspector.
        /// </summary>
        public void UpdateInspector()
        {
            useMinMax = (comparedOperator == EqualityOperators.IsBetween);
            useDesiredValue = (comparedOperator != EqualityOperators.IsBetween);
        }

        /// <summary>
        /// Returns false if the Referenced Value is null, or it does evalute to the the Desired Value using the Equality Operator. Otherwise returns true.
        /// </summary>
        public bool Value
        {
            get
            {
                if (referencedValue == null) return false;

                switch (comparedOperator)
                {
                    case EqualityOperators.EqualTo:
                        return desiredValue == referencedValue;
                    case EqualityOperators.NotEqualTo:
                        return desiredValue != referencedValue;
                    case EqualityOperators.GreaterThan:
                        return desiredValue > referencedValue;
                    case EqualityOperators.LessThan:
                        return desiredValue < referencedValue;
                    case EqualityOperators.GreaterThanOrEqualTo:
                        return desiredValue >= referencedValue;
                    case EqualityOperators.LessThanOrEqualTo:
                        return desiredValue <= referencedValue;
                    case EqualityOperators.IsBetween:
                        return referencedValue > desiredMinValue && referencedValue < desiredMaxValue;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Allows this class to evaluate as a Bool, using the Value property.
        /// </summary>
        /// <param name="scriptableIntPairing"></param>
        public static implicit operator bool(ScriptableIntPairing scriptableIntPairing)
        {
            return scriptableIntPairing.Value;
        }
    }
    #endregion

    /// <summary>
    /// Allows Unity Events to be invoked based on conditions.
    /// </summary>
    public class ConditionalEventListener : MonoBehaviour
    {
        #region Fields
        [SerializeField, Tooltip("Whether all values must be true for it to succeed, or just one")] private bool shouldAllBeTrue = true;
        [SerializeField] private bool isAutomatic = true;
        [SerializeField] private bool callOnEnable = false;

        [Header("Conditions")]
        [SerializeField, NonReorderable] private List<ScriptableBoolPairing> boolPairings = new List<ScriptableBoolPairing>();
        [SerializeField, NonReorderable] private List<ScriptableStringPairing> stringPairings = new List<ScriptableStringPairing>();
        [SerializeField, NonReorderable] private List<ScriptableFloatPairing> floatPairings = new List<ScriptableFloatPairing>();
        [SerializeField, NonReorderable] private List<ScriptableIntPairing> intPairings = new List<ScriptableIntPairing>();

        [Header("")]
        [SerializeField] private UnityEvent SuccessEvent = new UnityEvent();
        [SerializeField] private UnityEvent FailEvent = new UnityEvent();
        #endregion

        #region Methods
        /// <summary>
        /// Registers listeners to all conditions, allowing them to be automatically evaluated whenever changed.
        /// </summary>
        private void Awake()
        {
            if (!isAutomatic) return;

            foreach (ScriptableBoolPairing pairing in boolPairings)
            {
                pairing.referencedValue.RegisterListener(Invoke);
            }

            foreach (ScriptableStringPairing pairing in stringPairings)
            {
                pairing.referencedValue.RegisterListener(Invoke);
            }

            foreach (ScriptableFloatPairing pairing in floatPairings)
            {
                pairing.referencedValue.RegisterListener(Invoke);
            }

            foreach (ScriptableIntPairing pairing in intPairings)
            {
                pairing.referencedValue.RegisterListener(Invoke);
            }
        }

        /// <summary>
        /// Updates the Inspector to show the correct fields needed for the selected Equality Operator.
        /// </summary>
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

        /// <summary>
        /// Calls Invoke when the GameObject is set to active if isAutomatic = true;.
        /// </summary>
        private void OnEnable()
        {
            if(isAutomatic || callOnEnable) Invoke();
        }

        /// <summary>
        /// Unregisters listeners when the GameObject is destroyed to prevent null references.
        /// </summary>
        private void OnDestroy()
        {
            if (!isAutomatic) return;

            foreach (ScriptableBoolPairing pairing in boolPairings)
            {
                pairing.referencedValue.UnregisterListener(Invoke);
            }

            foreach (ScriptableStringPairing pairing in stringPairings)
            {
                pairing.referencedValue.UnregisterListener(Invoke);
            }

            foreach (ScriptableFloatPairing pairing in floatPairings)
            {
                pairing.referencedValue.UnregisterListener(Invoke);
            }

            foreach (ScriptableIntPairing pairing in intPairings)
            {
                pairing.referencedValue.UnregisterListener(Invoke);
            }
        }

        /// <summary>
        /// Invokes the Success or Fail Unity Event based on conditions.
        /// </summary>
        public void Invoke()
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

        /// <summary>
        /// Evaluates conditions.
        /// </summary>
        /// <returns>If shouldAllBeTrue = true, returns false if one conditional pair is false, returns true if all are true.
        /// If shouldAllBeTrue = false, returns true if one conditional pair is true, returns false if all are false.</returns>
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