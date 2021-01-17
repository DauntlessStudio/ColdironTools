// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ColdironTools.UI
{
    /// <summary>
    /// Rotates the UI Object based on an input axis.
    /// </summary>
    public class UIMouseRotate : Selectable, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields
        [Tooltip("The axis that controls rotation.")]
        [SerializeField] string axisName = "Mouse X";

        [Tooltip("The speed with which the object should rotate.")]
        [SerializeField] float rotationSpeed = 5.0f;

        private RectTransform rectTransform;
        private bool isPointerDown = false;
        #endregion

        #region Methods
        /// <summary>
        /// Assigns the rectTransform and performs the alpha hit test.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            rectTransform = GetComponent<RectTransform>();

            AlphaHitTest.AttemptTransparencyHitTest(gameObject);
        }

        /// <summary>
        /// Sets isPointerDown to true.
        /// </summary>
        /// <param name="eventData">Data about the pointer</param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            isPointerDown = true;
        }

        /// <summary>
        /// Sets isPointerDown to false.
        /// </summary>
        /// <param name="eventData">Data about the pointer</param>
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            isPointerDown = false;
        }

        /// <summary>
        /// Rotates the GameObject.
        /// </summary>
        protected virtual void LateUpdate()
        {
            if (isPointerDown)
            {
                float rotationAmount = Input.GetAxis(axisName) * rotationSpeed;
                rectTransform.Rotate(new Vector3(0.0f, 0.0f, rotationAmount));
            }
        }
        #endregion
    }
}