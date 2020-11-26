using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ColdironTools.UI
{
    [RequireComponent(typeof(Graphic))]
    public class UIOpenURL : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] Graphic targetGraphic;
        [SerializeField] string URL = "";

        private void OnValidate()
        {
            if (!targetGraphic)
            {
                targetGraphic = GetComponent<Graphic>();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Open(URL);
        }

        public void Open(string url)
        {
            Application.OpenURL(url);
        }
    }
}