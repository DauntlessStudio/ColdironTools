// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace ColdironTools.UI
{
    /// <summary>
    /// A simple and extendable component to allow drag and drop functionality in the UI. The partner component to UIDrag.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UIDrop : MonoBehaviour, IDropHandler
    {
        #region Fields
        [Tooltip("A string tagging this object. Should match the UIDrag's Drag Tag.")]
        [SerializeField] protected string dropTag = "";

        [Tooltip("Should a dropped item be destroyed when the tags match?")]
        [SerializeField] protected bool destroyDroppedItem = false;

        protected RectTransform rectTransform;
        protected bool isOccupied = false;

        protected UIDrag dragRef = null;
        #endregion

        #region Methods
        /// <summary>
        /// Assigns the rectTransform and the alpha hit test.
        /// </summary>
        protected void Awake()
        {
            rectTransform = GetComponent<RectTransform>(); 

            AlphaHitTest.AttemptTransparencyHitTest(gameObject);
        }

        /// <summary>
        /// Checks if the pointer is currently dragging a UIDrag component.
        /// </summary>
        /// <param name="eventData">The pointer data, including the GameObject being dragged.</param>
        public virtual void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag)
            {
                UIDrag drag = eventData.pointerDrag.GetComponent<UIDrag>();

                if (drag)
                {
                    ProcessDroppedObject(drag);
                }
            }
        }

        /// <summary>
        /// Assigns the UIDrag. Setting it's new transform data.
        /// </summary>
        /// <param name="dragItem">The UIDrag to be proccessed.</param>
        public virtual void ProcessDroppedObject(UIDrag dragItem)
        {
            if (dragItem.DragTag != dropTag || isOccupied) return;

            if (destroyDroppedItem)
            {
                Destroy(dragItem.gameObject);
                return;
            }

            dragRef = dragItem;

            dragItem.transform.SetParent(transform);
            dragItem.RectTransform.anchorMax = new Vector2(0.0f, 1.0f);
            dragItem.RectTransform.anchorMin = new Vector2(0.0f, 1.0f);
            dragItem.RectTransform.sizeDelta = rectTransform.sizeDelta;
            dragItem.RectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x / 2, (rectTransform.sizeDelta.y / 2) * -1);
            dragItem.SetCurrentDropPoint(this);

            isOccupied = true;
        }

        /// <summary>
        /// Called externally to force this component to remove the UIDrag.
        /// </summary>
        public virtual void ForceRemove()
        {
            if (dragRef)
            {
                dragRef.SetCurrentDropPoint(null);
                dragRef.OnSnapToOrigin();
                OnRemove();
            }
        }

        /// <summary>
        /// Removes the UIDrag reference and unoccupies this component.
        /// </summary>
        public virtual void OnRemove()
        {
            dragRef = null;
            isOccupied = false;
        }
        #endregion
    }
}