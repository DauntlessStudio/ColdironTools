using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ColdironTools.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Fields
        [SerializeField] private string dragTag;
        [SerializeField] private string dragItemType;
        private Canvas canvas;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Vector2 originPoint;
        private bool snapToOrigin;
        private Vector2 originSize;
        private Transform originTransform;
        private UIDrop currentDropPoint;
        public event System.EventHandler BeginDrag;
        public event System.EventHandler EndDrag;

        public RectTransform RectTransform { get => rectTransform;}
        public bool SnapToOrigin { get => snapToOrigin; set => snapToOrigin = value; }
        public string DragTag { get => dragTag;}
        public string DragItemType { get => dragItemType; }
        #endregion

        #region Methods
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponentInParent<Canvas>();
            originPoint = rectTransform.anchoredPosition;
            originTransform = transform.parent;
            originSize = rectTransform.sizeDelta;

            //Attempt alpha set will only work on sprites that have read/write enabled
            Image image = GetComponent<Image>();
            if (image)
            {
                if (image.sprite.texture.isReadable)
                {
                    image.alphaHitTestMinimumThreshold = 1.0f;
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
            snapToOrigin = true;
            transform.SetParent(canvas.transform);
            if (currentDropPoint)
            {
                currentDropPoint.OnRemove();
                currentDropPoint = null;
            }
            BeginDrag?.Invoke(this, System.EventArgs.Empty);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            CheckDropBelow();
        }

        private void CheckDropBelow()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current) { pointerId = -1};

            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (RaycastResult item in results)
            {
                UIDrop drop = item.gameObject.GetComponent<UIDrop>();
                if (drop != null && drop.ShouldAutoDrop)
                {
                    drop.ProcessDroppedObject(this);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1.0f;
            if (snapToOrigin)
            {
                transform.SetParent(originTransform);
                rectTransform.anchoredPosition = originPoint;
                rectTransform.sizeDelta = originSize;
            }
            EndDrag?.Invoke(this, System.EventArgs.Empty);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void ModifyTag(string tag)
        {
            dragTag = tag;
        }

        public void ModifyType(string type)
        {
            dragItemType = type;
        }

        public void SetCurrentDropPoint(UIDrop drop)
        {
            currentDropPoint = drop;
        }

        public void OnSnapToOrigin()
        {
            transform.SetParent(originTransform);
            rectTransform.anchoredPosition = originPoint;
            rectTransform.sizeDelta = originSize;

            if (!originTransform.gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
        #endregion
    }
}