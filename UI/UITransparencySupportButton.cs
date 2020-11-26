using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ColdironTools.UI
{
    public class UITransparencySupportButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
    {
        [SerializeField] private bool interactable = true;
        [SerializeField, Tooltip("Image must be Read/Write, and the mesh type must be Full Rect")] private Image targetImage;

        [Header("Color Transition")]
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color highlightedColor = new Color(0.9f, 0.9f, 0.9f);
        [SerializeField] private Color selectedColor = new Color(0.8f, 0.8f, 0.8f);
        private bool isMouseOver = false;

        [Header("")]
        [SerializeField] private UnityEvent OnClick = new UnityEvent();

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
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            targetImage.color = highlightedColor;
            isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            targetImage.color = defaultColor;
            isMouseOver = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            targetImage.color = isMouseOver ? highlightedColor : defaultColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            targetImage.color = selectedColor;
        }
    }
}