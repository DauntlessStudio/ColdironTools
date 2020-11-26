using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ColdironTools.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UIMouseRotate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private bool interactable = true;
        [SerializeField, Tooltip("Image must be Read/Write, and the mesh type must be Full Rect")] private Image targetImage;

        [Header("Rotation Values")]
        [SerializeField] string axisName = "Mouse X";
        [SerializeField] float rotationSpeed = 5.0f;
        private bool isRotationDesired = false;
        private RectTransform rect;

        [Header("Color Transition")]
        private bool isMouseOver = false;
        private bool isMouseDown = false;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color highlightedColor = new Color(0.9f, 0.9f, 0.9f);
        [SerializeField] private Color selectedColor = new Color(0.8f, 0.8f, 0.8f);

        [Header("Rotation Events")]
        [SerializeField] bool useRotationEvents = true;
        [SerializeField, ConditionalHide("useRotationEvents")] float desiredMinRotation = 0.0f;
        [SerializeField, ConditionalHide("useRotationEvents")] float desiredMaxRotation = 0.0f;

        [Header("")]
        [SerializeField] private UnityEvent correctRotationEvent = new UnityEvent();
        [SerializeField] private UnityEvent incorrectRotationEvent = new UnityEvent();

        private void OnValidate()
        {
            if (!targetImage)
            {
                targetImage = GetComponent<Image>();
            }
        }

        private void OnEnable()
        {
            if (targetImage && interactable && targetImage.sprite && targetImage.sprite.texture.isReadable)
            {
                targetImage.color = defaultColor;
                targetImage.alphaHitTestMinimumThreshold = 1.0f;
            }
            else if(targetImage.sprite && !targetImage.sprite.texture.isReadable)
            {
                Debug.LogWarning(gameObject.name + "'s Target Image sprite is not set to Read/Write. Please adjust the import settings if you want to have image transparency support");
            }

            rect = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!isMouseDown) targetImage.color = highlightedColor;
            isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!isMouseDown) targetImage.color = defaultColor;
            isMouseOver = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            targetImage.color = selectedColor;
            isMouseDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            targetImage.color = isMouseOver ? highlightedColor : defaultColor;
            isMouseDown = false;
        }

        private void Update()
        {
            if (isMouseDown)
            {
                float rotationAmount = Input.GetAxis(axisName) * rotationSpeed;
                rect.Rotate(new Vector3(0.0f, 0.0f, rotationAmount));

                if(!useRotationEvents) return;

                if (rect.rotation.eulerAngles.z >= desiredMinRotation && rect.rotation.eulerAngles.z <= desiredMaxRotation && !isRotationDesired)
                {
                    isRotationDesired = true;
                    correctRotationEvent.Invoke();
                }
                else if (isRotationDesired && !(rect.rotation.eulerAngles.z >= desiredMinRotation && rect.rotation.eulerAngles.z <= desiredMaxRotation))
                {
                    isRotationDesired = false;
                    incorrectRotationEvent.Invoke();
                }
            }
        }
    }
}