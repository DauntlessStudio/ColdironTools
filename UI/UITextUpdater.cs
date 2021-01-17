// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using UnityEngine.UI;
using ColdironTools.Scriptables;
using ColdironTools.EditorExtensions;

namespace ColdironTools.UI
{
    /// <summary>
    /// Updates the Text component based on external values assigned in the editor.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class UITextUpdater : MonoBehaviour
    {
        #region Fields
        [Tooltip("The text component to be edited.")]
        [SerializeField] private Text text;

        [Header("Contents")]
        [Tooltip("A prefix that appears before the content.")]
        [SerializeField] private StringScriptableReference prefix = new StringScriptableReference();

        [Tooltip("The content as a string.")]
        [SerializeField] private StringScriptableReference content = new StringScriptableReference();

        [Tooltip("The content as a float.")]
        [ConditionalHide("showFloat")]
        [SerializeField] private FloatScriptableReference floatContent = new FloatScriptableReference();

        [Tooltip("The content as an int.")]
        [ConditionalHide("showInt")]
        [SerializeField] private IntScriptableReference intContent = new IntScriptableReference();

        [Tooltip("The suffix that appears after the content.")]
        [SerializeField] private StringScriptableReference suffix = new StringScriptableReference();

        [Header("Display Content Settings")]
        [Tooltip("Should float contents be shown?")]
        [SerializeField] private bool showFloat = false;

        [Tooltip("Should int contents be shown?")]
        [SerializeField] private bool showInt = false;
        #endregion

        #region Methods
        /// <summary>
        /// Attempts to assign Text component automatically.
        /// </summary>
        private void OnValidate()
        {
            if (!text)
            {
                text = GetComponent<Text>();
            }
        }

        /// <summary>
        /// Registers listeners and edits the text when the object becomes active.
        /// </summary>
        private void OnEnable()
        {
            prefix.RegisterListener(EditText);
            content.RegisterListener(EditText);
            floatContent.RegisterListener(EditText);
            intContent.RegisterListener(EditText);
            suffix.RegisterListener(EditText);

            EditText();
        }

        /// <summary>
        /// Unregisters listeners when the object becomes inactive.
        /// </summary>
        private void OnDisable()
        {
            prefix.UnregisterListener(EditText);
            content.UnregisterListener(EditText);
            floatContent.UnregisterListener(EditText);
            intContent.UnregisterListener(EditText);
            suffix.UnregisterListener(EditText);
        }

        /// <summary>
        /// Edits the text to match the fields.
        /// </summary>
        private void EditText()
        {
            if (!text) return;

            string floatContentMessage = showFloat ? floatContent.Value.ToString() : "";
            string intContentMessage = showInt ? intContent.Value.ToString() : "";

            if (intContent < 10 && intContent >= 0 && showInt) 
                intContentMessage = intContentMessage.Insert(0, "0");

            text.text = prefix + content + floatContentMessage + intContentMessage + suffix;
        }
        #endregion
    }
}