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
    [RequireComponent(typeof(Graphic))]
    public class UIColorSetter : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Graphic graphic;
        [SerializeField] private ColorScriptableReference colorReference = new ColorScriptableReference();
        #endregion

        #region Methods
        /// <summary>
        /// Assigns the graphic component and assigns the color.
        /// </summary>
        private void OnValidate()
        {
            if (!graphic)
            {
                graphic = GetComponent<Graphic>();
                UpdateColor();
            }
        }

        /// <summary>
        /// Registers this as a listner to the ColorScriptable when the GameObject becomes active.
        /// </summary>
        private void OnEnable()
        {
            if(colorReference != null) colorReference.RegisterListener(UpdateColor);
            UpdateColor();
        }

        /// <summary>
        /// Unregisters this as a listener to the ColorScriptable when the GameObject becomes inactive.
        /// </summary>
        private void OnDisable()
        {
            if(colorReference != null) colorReference.UnregisterListener(UpdateColor);
        }

        /// <summary>
        /// Sets the Graphic component's color to match the colorReference.
        /// </summary>
        private void UpdateColor()
        {
            if (colorReference != null) graphic.color = colorReference;
        }

        /// <summary>
        /// Sets the Graphic component's color to the ColorScriptable. Can be assigned with Unity Events.
        /// </summary>
        /// <param name="color">The ColorScriptable to set to</param>
        public void SetColor(ColorScriptable color)
        {
            graphic.color = color;
        }

        /// <summary>
        /// Sets the Graphic component's color to the color param. Cannot be assigned with Unity Events.
        /// </summary>
        /// <param name="color">The color to be set to</param>
        public void SetColor(Color color)
        {
            graphic.color = color;
        }

        /// <summary>
        /// Sets the red value of the Graphic component's color.
        /// </summary>
        /// <param name="red">A float for the red value</param>
        public void SetRed(float red)
        {
            Color newColor = graphic.color;
            newColor.r = red;
            if (colorReference != null) colorReference.Value = newColor;
        }

        /// <summary>
        /// Sets the blue value of the Graphic component's color.
        /// </summary>
        /// <param name="blue">A float for the blue value</param>
        public void SetBlue(float blue)
        {
            Color newColor = graphic.color;
            newColor.b = blue;
            if (colorReference != null) colorReference.Value = newColor;
        }

        /// <summary>
        /// Sets the green value of the Graphic component's color.
        /// </summary>
        /// <param name="green">A float for the green value</param>
        public void SetGreen(float green)
        {
            Color newColor = graphic.color;
            newColor.g = green;
            if (colorReference != null) colorReference.Value = newColor;
        }

        /// <summary>
        /// Sets the alpha (transparency) value of the Graphic component's color.
        /// </summary>
        /// <param name="alpha">A float for the alpha value. 0 is fully tranparent, 1 is fully opaque</param>
        public void SetAlpha(float alpha)
        {
            Color newColor = graphic.color;
            newColor.a = alpha;
            if (colorReference != null) colorReference.Value = newColor;
        }
        #endregion
    }
}