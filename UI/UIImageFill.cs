// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using UnityEngine.UI;
using ColdironTools.Scriptables;

namespace ColdironTools.UI
{
    /// <summary>
    /// Sets an Image component's fill amount based on Float Scriptable values.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class UIImageFill : MonoBehaviour
    {
        #region Fields
        [Tooltip("Image to set the fill amount on.")]
        [SerializeField] Image image;

        [Tooltip("Current value. How far the image should be filled.")]
        [SerializeField] FloatScriptableReference value = new FloatScriptableReference();

        [Tooltip("Min value. Where empty should be on the filled image.")]
        [SerializeField] FloatScriptableReference minValue = new FloatScriptableReference();

        [Tooltip("Max value. Where full should be on the filled image.")]
        [SerializeField] FloatScriptableReference maxValue = new FloatScriptableReference();
        #endregion

        #region Methods
        /// <summary>
        /// Attempts to automatically find the image and set it's type to Filled.
        /// </summary>
        void OnValidate()
        {
            if (!image) image = GetComponent<Image>();
            if (image) image.type = Image.Type.Filled;
        }

        /// <summary>
        /// Registers listeners and calls update fill amount when the GameObject becomes active.
        /// </summary>
        private void OnEnable()
        {
            value.RegisterListener(UpdateFillAmount);
            minValue.RegisterListener(UpdateFillAmount);
            maxValue.RegisterListener(UpdateFillAmount);

            UpdateFillAmount();
        }

        /// <summary>
        /// Unregisters listeners when the GameObject becomes inactive. Prevents null references.
        /// </summary>
        private void OnDisable()
        {
            value.UnregisterListener(UpdateFillAmount);
            minValue.UnregisterListener(UpdateFillAmount);
            maxValue.UnregisterListener(UpdateFillAmount);
        }

        /// <summary>
        /// Sets the image fill amount to a the percentage of the value between the min and max.
        /// </summary>
        private void UpdateFillAmount()
        {
            image.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(minValue, maxValue, value));
        }
        #endregion
    }
}