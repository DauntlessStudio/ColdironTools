using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ColdironTools.UI
{
    public class UITransparencySupportButton : Selectable, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
    {
        [Header("")]
        [SerializeField] private UnityEvent OnClick = new UnityEvent();

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
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
        }
    }
}