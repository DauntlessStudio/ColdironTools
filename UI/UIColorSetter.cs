using UnityEngine;
using UnityEngine.UI;
using ColdironTools.Scriptables;

namespace ColdironTools.UI
{
    [RequireComponent(typeof(Graphic))]
    public class UIColorSetter : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Graphic graphic;
        [SerializeField] private ColorScriptableReference colorReference = new ColorScriptableReference();
        #endregion

        #region Methods
        private void OnValidate()
        {
            if (!graphic)
            {
                graphic = GetComponent<Graphic>();
                UpdateColor();
            }
        }

        private void OnEnable()
        {
            if(colorReference != null) colorReference.RegisterListener(UpdateColor);
        }

        private void OnDisable()
        {
            if(colorReference != null) colorReference.UnregisterListener(UpdateColor);
        }

        private void UpdateColor()
        {
            if (colorReference != null) graphic.color = colorReference;
        }

        public void SetColor(ColorScriptable color)
        {
            graphic.color = color;
        }

        public void SetColor(Color color)
        {
            graphic.color = color;
        }

        public void SetRed(float red)
        {
            Color newColor = graphic.color;
            newColor.r = red;
            graphic.color = newColor;
        }

        public void SetBlue(float blue)
        {
            Color newColor = graphic.color;
            newColor.b = blue;
            graphic.color = newColor;
        }

        public void SetGreen(float green)
        {
            Color newColor = graphic.color;
            newColor.g = green;
            graphic.color = newColor;
        }

        public void SetAlpha(float alpha)
        {
            Color newColor = graphic.color;
            newColor.a = alpha;
            graphic.color = newColor;
        }
        #endregion
    }
}