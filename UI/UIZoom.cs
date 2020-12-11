using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ColdironTools.UI
{
    [RequireComponent(typeof(Image))]
    public class UIZoom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image image;

        [Header("Input")]
        [SerializeField] private string zoomAxis = "Mouse ScrollWheel";
        [SerializeField] private string panYAxis = "Mouse Y";
        [SerializeField] private string panXAxis = "Mouse X";
        [SerializeField] private float panSpeed = 10.0f;
        [SerializeField] private float zoomMulitplier = 2.0f;

        private bool shouldPan = false;
        private bool isZoomed = false;
        private Vector3 originialPosition;

        private void OnValidate()
        {
            if (!image)
            {
                image = GetComponent<Image>();
            }
        }

        private void Awake()
        {
            originialPosition = image.transform.localPosition;
        }

        private void OnDisable()
        {
            ZoomOut();
        }

        public void ZoomIn()
        {
            image.transform.localScale = new Vector2(zoomMulitplier, zoomMulitplier);
            isZoomed = true;
        }

        public void ZoomOut()
        {
            image.transform.localScale = new Vector2(1.0f, 1.0f);
            image.transform.localPosition = originialPosition;
            isZoomed = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            shouldPan = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            shouldPan = false;
        }

        private void Update()
        {
            if (shouldPan && isZoomed)
            {
                Pan();
            }

            if (!isZoomed && Input.GetAxis(zoomAxis) > 0.0f)
            {
                ZoomIn();
            }

            if (isZoomed && Input.GetAxis(zoomAxis) < 0.0f)
            {
                ZoomOut();
            }
        }

        private void Pan()
        {
            Vector2 movePos = image.transform.localPosition;
            movePos.x += Input.GetAxis(panXAxis) * panSpeed;
            movePos.y += Input.GetAxis(panYAxis) * panSpeed;

            image.transform.localPosition = movePos;
        }
    }
}