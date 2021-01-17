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
    /// Allows simple UI zoom functionality.
    /// Should have a Mask component attached to the parent for best effect.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class UIZoom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields
        [Tooltip("The image to have zoom support.")]
        [SerializeField] private Image image;

        [Header("Input")]

        [Tooltip("The input axis for zooming.")]
        [SerializeField] private string zoomAxis = "Mouse ScrollWheel";

        [Tooltip("The input axis for panning vertically.")]
        [SerializeField] private string panYAxis = "Mouse Y";

        [Tooltip("The input axis for panning horizontally.")]
        [SerializeField] private string panXAxis = "Mouse X";

        [Tooltip("The image's panning speed.")]
        [SerializeField] private float panSpeed = 20.0f;

        [Tooltip("The number of zoom steps.")]
        [SerializeField, Range(0, 5)] private int zoomMaxStep = 1;

        [Tooltip("The amount by which the image should be enlarged.")]
        [SerializeField] private float zoomMulitplier = 2.0f;

        private int zoomCurrentStep = 0;
        private bool shouldPan = false;
        private Vector3 originialPosition;
        #endregion

        #region Methods
        /// <summary>
        /// Attempts to automatically assign the image.
        /// </summary>
        private void OnValidate()
        {
            if (!image)
            {
                image = GetComponent<Image>();
            }
        }

        /// <summary>
        /// Sets the original position of the image.
        /// </summary>
        private void Awake()
        {
            if(image) originialPosition = image.transform.localPosition;
        }

        /// <summary>
        /// Resets the zoom amount when the object becomes inactive.
        /// </summary>
        private void OnDisable()
        {
            int steps = zoomCurrentStep;
            for (int i = 0; i < steps; i++)
            {
                ZoomOut();
            }
        }

        /// <summary>
        /// Enlarges the image by the zoom amount.
        /// </summary>
        public void ZoomIn()
        {
            if(zoomCurrentStep >= zoomMaxStep) return;

            float zoomVal = zoomMulitplier * (zoomCurrentStep + 1);
            image.transform.localScale = new Vector2(zoomVal, zoomVal);
            zoomCurrentStep = Mathf.Clamp(zoomCurrentStep + 1, 0, zoomMaxStep);
        }

        /// <summary>
        /// Reduces the image by the zoom amount.
        /// </summary>
        public void ZoomOut()
        {
            if(zoomCurrentStep <= 0) return;

            float zoomVal = zoomMulitplier * (zoomCurrentStep - 1);
            if(zoomVal <= 0) zoomVal = 1.0f;
            image.transform.localScale = new Vector2(zoomVal, zoomVal);
            image.transform.localPosition = originialPosition;
            zoomCurrentStep = Mathf.Clamp(zoomCurrentStep - 1, 0, zoomMaxStep);
        }

        /// <summary>
        /// Sets shouldPan to true when the pointer is down.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            shouldPan = true;
        }

        /// <summary>
        /// Sets shouldPan to false when the pointer is up.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            shouldPan = false;
        }

        /// <summary>
        /// Processes the inputs to zoom and pan.
        /// </summary>
        private void Update()
        {
            if (shouldPan && zoomCurrentStep >= 1)
            {
                Pan();
            }

            if (Input.GetAxis(zoomAxis) > 0.0f)
            {
                ZoomIn();
            }

            if (Input.GetAxis(zoomAxis) < 0.0f)
            {
                ZoomOut();
            }
        }

        /// <summary>
        /// Updates position for panning.
        /// </summary>
        private void Pan()
        {
            Vector2 movePos = image.transform.localPosition;
            movePos.x += Input.GetAxis(panXAxis) * panSpeed;
            movePos.y += Input.GetAxis(panYAxis) * panSpeed;

            image.transform.localPosition = movePos;
        }
    }
    #endregion
}