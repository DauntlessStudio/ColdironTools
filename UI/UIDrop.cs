using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace ColdironTools.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UIDrop : MonoBehaviour, IDropHandler
    {
        #region Fields
        protected RectTransform rectTransform;
        protected bool isOccupied = false;
        [SerializeField] protected string dropTag = "";
        [SerializeField] protected string dropItemType = "";

        [Header("Drop processing")]
        [SerializeField] protected bool destroyOnSuccess = false;
        [SerializeField] protected bool shouldAutoDrop = false;

        [Header("")]
        [SerializeField] protected UnityEvent matchTagEvent = new UnityEvent();
        [SerializeField] protected UnityEvent removedEvent = new UnityEvent();
        protected UIDrag dragRef = null;

        public bool IsOccupied { get => isOccupied; }
        public bool ShouldAutoDrop { get => shouldAutoDrop; }
        #endregion

        #region Methods
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag)
            {
                ProcessDroppedObject(eventData.pointerDrag.GetComponent<UIDrag>());
            }
        }

        public virtual void ProcessDroppedObject(UIDrag dragItem)
        {
            if (dragItem.DragItemType != dropItemType || IsOccupied)
            {
                return;
            }

            dragRef = dragItem;
            dragItem.transform.SetParent(transform);
            dragItem.RectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x / 2, (rectTransform.sizeDelta.y / 2) * -1);
            dragItem.SnapToOrigin = false;
            dragItem.RectTransform.sizeDelta = rectTransform.sizeDelta;

            if (dragItem.DragTag == dropTag)
            {
                matchTagEvent.Invoke();
                if (destroyOnSuccess)
                {
                    Destroy(dragItem.gameObject);
                    isOccupied = false;
                    return;
                }
            }
            dragItem.SetCurrentDropPoint(this);
            isOccupied = true;
        }

        public void ModifyTag(string tag)
        {
            dropTag = tag;
        }

        public void ForceRemove()
        {
            if (dragRef)
            {
                dragRef.OnSnapToOrigin();
                OnRemove();
            }
        }

        public virtual void OnRemove()
        {
            dragRef = null;
            removedEvent.Invoke();
            isOccupied = false;
        }
        #endregion
    }
}