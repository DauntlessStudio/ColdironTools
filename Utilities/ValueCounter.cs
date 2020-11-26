using System.Collections;
using UnityEngine;
using ColdironTools.Scriptables;

public class ValueCounter : MonoBehaviour
{
    [SerializeField] private FloatScriptable value = null;
    [SerializeField] private FloatScriptableReference maxValue = null;
    [SerializeField] private FloatScriptableReference minValue = null;
    [SerializeField] private FloatScriptableReference incrementValue = null;
    [SerializeField] private FloatScriptableReference updateFrequency = null;
    [SerializeField] private BoolScriptableReference shouldUpdate = null;
    [SerializeField] private bool shouldCycle = false;
    private bool isMaxCycle = false;

    private void OnEnable()
    {
        StartCoroutine(UpdateCount());
    }

    private void OnDisable()
    {
        StopCoroutine(UpdateCount());
    }

    private IEnumerator UpdateCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateFrequency);
            if(shouldUpdate) IncrementValue();
        }
    }

    private void IncrementValue()
    {
        value.Value = Mathf.Clamp(value + incrementValue, minValue, maxValue);

        if (value >= maxValue && shouldCycle)
        {
            if (isMaxCycle)
            {
                value.Value = minValue;
                isMaxCycle = false;
            }
            else
            {
                isMaxCycle = true;
            }
        }
    }

    public void ResetValue()
    {
        value.Value = minValue;
        isMaxCycle = false;
    }
}
