using UnityEngine;

namespace ColdironTools.UI
{
    public class UIMouseFollow : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.position = Input.mousePosition;
        }
    }
}