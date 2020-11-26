using UnityEngine;
using UnityEngine.UI;
using ColdironTools.Scriptables;

namespace ColdironTools.UI
{
    [RequireComponent(typeof(Slider))]
    public class UIInitializeSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private FloatScriptableReference value = new FloatScriptableReference();

        private void OnValidate()
        {
            if (!slider)
            {
                slider = GetComponentInChildren<Slider>();
            }
        }

        private void OnEnable()
        {
            InitSlider();
        }

        private void InitSlider()
        {
            slider.value = value;
        }
    }
}