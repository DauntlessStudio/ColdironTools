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
    /// A simple and extendable component to allow drag and drop functionality in the UI. Partner component to UIDrop.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Fields
        [Tooltip("A string tagging this object. Should match the UIDrop's Drop Tag.")]
        [SerializeField] private string dragTag;

        [Tooltip("How transparent this object should become while dragging.")]
        [SerializeField] private float dragTransparency = 0.6f;

        private Canvas canvas;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Vector2 originPoint;

        [Tooltip("Should this object snap back to it's original parent and position when released?")]
        [SerializeField] private bool snapToOrigin = true;
        private Vector2 originSize;
        private Vector2 originAnchorMin;
        private Vector2 originAnchorMax;
        private Transform originParent;

        private UIDrop currentDropPoint;
        public event System.EventHandler BeginDrag;
        public event System.EventHandler EndDrag;

        public RectTransform RectTransform { get => rectTransform;}
        public string DragTag { get => dragTag; set => dragTag = value; }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes values on Awake.
        /// </summary>
        protected virtual void Awake()
        {
            AssignReferences();

            SetOriginValues();

            AlphaHitTest.AttemptTransparencyHitTest(gameObject);
        }

        /// <summary>
        /// Assigns needed references.
        /// </summary>
        protected virtual void AssignReferences()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponentInParent<Canvas>();
        }

        /// <summary>
        /// Stores the default values that will be used later in OnSnapToOrigin.
        /// </summary>
        protected virtual void SetOriginValues()
        {
            originParent = transform.parent;
            originPoint = rectTransform.anchoredPosition;
            originSize = rectTransform.sizeDelta;
            originAnchorMax = rectTransform.anchorMax;
            originAnchorMin = rectTransform.anchorMin;
        }

        /// <summary>
        /// Implementation of IBeginDragHandler.
        /// </summary>
        /// <param name="eventData">Data about the pointer</param>
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(canvas.transform);
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = dragTransparency;

            SetCurrentDropPoint(null);

            BeginDrag?.Invoke(this, System.EventArgs.Empty);
        }

        /// <summary>
        /// Implementation of IDragHandler.
        /// </summary>
        /// <param name="eventData">Data about the pointer</param>
        public virtual void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        /// <summary>
        /// Implementation of IDragEndHandler
        /// </summary>
        /// <param name="eventData">Data about the pointer</param>
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1.0f;

            OnSnapToOrigin();

            EndDrag?.Invoke(this, System.EventArgs.Empty);
        }

        /// <summary>
        /// Removes this object from the previous UIDrop and sets a new currentDropPoint.
        /// </summary>
        /// <param name="drop"></param>
        public virtual void SetCurrentDropPoint(UIDrop drop)
        {
            if (currentDropPoint != null)
            {
                currentDropPoint.OnRemove();
            }

            currentDropPoint = drop;
        }

        /// <summary>
        /// Snaps to origin if snapToOrigin is true and currentDropPoint is not assigned.
        /// </summary>
        public virtual void OnSnapToOrigin()
        {
            if (!snapToOrigin || currentDropPoint != null) return;

            rectTransform.anchorMin = originAnchorMin;
            rectTransform.anchorMax = originAnchorMax;

            transform.SetParent(originParent);

            rectTransform.sizeDelta = originSize;
            rectTransform.anchoredPosition = originPoint;

            if (!originParent.gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}