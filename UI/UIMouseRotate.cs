using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ColdironTools.UI
{
    public class UIMouseRotate : Selectable, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Rotation Values")]
        [SerializeField] string axisName = "Mouse X";
        [SerializeField] float rotationSpeed = 5.0f;
        private bool isRotationDesired = false;
        private RectTransform rect;

        private bool isMouseDown = false;

        [Header("Rotation Events")]
        [SerializeField] bool useRotationEvents = true;
        [SerializeField, ConditionalHide("useRotationEvents")] float desiredMinRotation = 0.0f;
        [SerializeField, ConditionalHide("useRotationEvents")] float desiredMaxRotation = 0.0f;

        [Header("")]
        [SerializeField] private UnityEvent correctRotationEvent = new UnityEvent();
        [SerializeField] private UnityEvent incorrectRotationEvent = new UnityEvent();

        protected override void OnEnable()
        {
            base.OnEnable();

            Image targetImage = (Image)targetGraphic;

            if (targetImage && interactable && targetImage.sprite && targetImage.sprite.texture.isReadable)
            {
                targetImage.color = colors.normalColor;
                targetImage.alphaHitTestMinimumThreshold = 1.0f;
            }
            else if (targetImage.sprite && !targetImage.sprite.texture.isReadable)
            {
                Debug.LogWarning(gameObject.name + "'s Target Image sprite is not set to Read/Write. Please adjust the import settings if you want to have image transparency support");
            }

            rect = GetComponent<RectTransform>();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            isMouseDown = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            isMouseDown = false;
        }

        private void Update()
        {
            if (isMouseDown)
            {
                float rotationAmount = Input.GetAxis(axisName) * rotationSpeed;
                rect.Rotate(new Vector3(0.0f, 0.0f, rotationAmount));

                if (!useRotationEvents) return;

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