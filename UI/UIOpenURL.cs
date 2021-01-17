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
    /// Opens a URL when clicking on a UI Object. 
    /// </summary>
    public class UIOpenURL : MonoBehaviour, IPointerClickHandler
    {
        #region Fields
        [Tooltip("The URL that should be opened.")]
        [SerializeField] string URL = "";
        #endregion

        #region Methods
        /// <summary>
        /// Opens the URL when clicking.
        /// </summary>
        /// <param name="eventData">Pointer data</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            Open(URL);
        }

        /// <summary>
        /// Opens a URL. Does not work in the editor.
        /// </summary>
        /// <param name="url">The URL to open.</param>
        public void Open(string url)
        {
            Application.OpenURL(url);
        }
        #endregion
    }
}