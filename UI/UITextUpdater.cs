using UnityEngine;
using UnityEngine.UI;
using ColdironTools.Scriptables;

namespace ColdironTools.UI
{
    [RequireComponent(typeof(Text))]
    public class UITextUpdater : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Text text;

        [Header("Contents")]
        [SerializeField] private StringScriptableReference prefix = new StringScriptableReference();
        [SerializeField] private StringScriptableReference content = new StringScriptableReference();
        [SerializeField] private FloatScriptableReference floatContent = new FloatScriptableReference();
        [SerializeField] private IntScriptableReference intContent = new IntScriptableReference();
        [SerializeField] private StringScriptableReference suffix = new StringScriptableReference();

        [Header("Display Content Settings")]
        [SerializeField] private bool showFloat = false;
        [SerializeField] private bool showInt = false;
        #endregion

        #region Methods
        private void OnValidate()
        {
            if (!text)
            {
                text = GetComponent<Text>();
            }
        }

        private void OnEnable()
        {
            prefix.RegisterListener(EditText);
            content.RegisterListener(EditText);
            floatContent.RegisterListener(EditText);
            intContent.RegisterListener(EditText);
            suffix.RegisterListener(EditText);

            EditText();
        }

        private void OnDisable()
        {
            prefix.UnregisterListener(EditText);
            content.UnregisterListener(EditText);
            floatContent.UnregisterListener(EditText);
            intContent.UnregisterListener(EditText);
            suffix.UnregisterListener(EditText);
        }

        public void ChangeContent(string newContent)
        {
            content.Value = newContent;
        }

        public void ChangeIntContent(int newContent)
        {
            intContent.Value = newContent;
        }

        public void ChangeFloatContent(float newContent)
        {
            floatContent.Value = newContent;
        }

        private void EditText()
        {
            if (!text) return;

            string floatContentMessage = showFloat ? floatContent.Value.ToString() : "";
            string intContentMessage = showInt ? intContent.Value.ToString() : "";
            if (intContent < 10 && intContent >= 0 && showInt) intContentMessage = intContentMessage.Insert(0, "0");

            text.text = prefix + content + floatContentMessage + intContentMessage + suffix;
        }
        #endregion
    }
}