using UnityEngine;
using UnityEngine.UI;
using ColdironTools.Scriptables;

namespace ColdironTools.UI
{
    [RequireComponent(typeof(Image))]
    public class UIImageFill : MonoBehaviour
    {
        #region Fields
        [Tooltip("Image to set the fill amount on. Must be set to type Fill")]
        [SerializeField] Image image;

        [Tooltip("Value to use as the current ")]
        [SerializeField] FloatScriptableReference value = new FloatScriptableReference();

        [Tooltip("Min value that Variable to have no fill on Image.")]
        [SerializeField] FloatScriptableReference min = new FloatScriptableReference();

        [Tooltip("Max value that Variable can be to fill Image.")]
        [SerializeField] FloatScriptableReference max = new FloatScriptableReference();
        #endregion

        #region Methods
        void OnValidate()
        {
            if (!image)
            {
                image = GetComponent<Image>();
                image.type = Image.Type.Filled;
            }
        }

        private void OnEnable()
        {
            value.RegisterListener(UpdateFillAmount);
            UpdateFillAmount(null, System.EventArgs.Empty);
        }

        private void OnDisable()
        {
            value.UnregisterListener(UpdateFillAmount);
        }

        private void UpdateFillAmount(object sender, System.EventArgs e)
        {
            image.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(min, max, value));
        }
        #endregion
    }
}