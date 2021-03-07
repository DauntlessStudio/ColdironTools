using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ColdironTools.Scriptables;

public class UITextExtractor : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private GameObject source = null;

    [SelectableList("textList", "textIndex")]
    #pragma warning disable CS0414
    [SerializeField] private string textDisplay = "";

    [SerializeField, HideInInspector] private List<string> textList = new List<string>();
    private List<StringScriptableReference> useList = new List<StringScriptableReference>();
    [SerializeField, HideInInspector] private int textIndex = 0;

    private void OnValidate()
    {
        UpdateTextList();

        foreach (StringScriptableReference reference in useList)
        {
            reference.RegisterListener(UpdateTextDisplay);
        }

        if (textIndex > textList.Count)
        {
            textIndex = textList.Count;
        }

        if (text == null)
        {
            text = GetComponent<Text>();
        }

        UpdateTextDisplay();
    }

    private void OnDestroy()
    {
        foreach (StringScriptableReference reference in useList)
        {
            reference.UnregisterListener(UpdateTextDisplay);
        }
    }

    private void UpdateTextList()
    {
        textList.Clear();
        useList.Clear();

        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        foreach (MonoBehaviour monoBehaviour in source.GetComponents(typeof(MonoBehaviour)))
        {
            foreach (FieldInfo field in monoBehaviour.GetType().GetFields(bindingFlags))
            {
                if (field.FieldType == typeof(StringScriptableReference))
                {
                    textList.Add(field.Name);
                    useList.Add(field.GetValue(monoBehaviour) as StringScriptableReference);
                }
            }
        }
    }

    private void UpdateTextDisplay()
    {
        if(text != null && useList.Count > 0)
        {
            text.text = useList[textIndex];
        }
    }
}
