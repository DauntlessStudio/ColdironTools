// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ColdironTools.Scriptables;

/// <summary>
/// Expands functionality of Unity Toggles. 
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class UIToggle : Selectable, IPointerClickHandler
{
    #region StructsAndEnums
    /// <summary>
    /// Different transition types for UIToggles.
    /// </summary>
    [System.Serializable]
    public enum ToggleTransition
    {
        Default, SwapActive, Custom
    }
    #endregion

    #region Fields
    [Header("Toggle Options")]

    [Tooltip("The graphic component used when the toggle is set to true.")]
    [SerializeField] private Graphic trueGraphic = null;

    [Tooltip("The transition style.")]
    [SerializeField] private ToggleTransition toggleTransition = ToggleTransition.Default;

    [Tooltip("The value of the toggle.")]
    [SerializeField] private BoolScriptableReference isOn = new BoolScriptableReference(true);

    [Tooltip("The event invoked when the value changes.")]
    [SerializeField] private Toggle.ToggleEvent onValueChanged = new Toggle.ToggleEvent();

    private System.Action onValueChangedAction;
    private System.Action<bool> onValueChangedActionParam;
    #endregion

    #region Methods
#if UNITY_EDITOR
    /// <summary>
    /// Initializes the references.
    /// Generates default objects.
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();

        RectTransform rectTransform = null;

        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (targetGraphic == null)
        {
            targetGraphic = Instantiate(new GameObject(), transform).AddComponent<Image>();
            targetGraphic.gameObject.name = "False Graphic";
            targetGraphic.rectTransform.sizeDelta = rectTransform.sizeDelta;

            Image targetImage = (Image)targetGraphic;
            if (targetImage)
            {
                targetImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            }
        }

        if (trueGraphic == null)
        {
            trueGraphic = Instantiate(new GameObject(), transform).AddComponent<Image>();
            trueGraphic.gameObject.name = "True Graphic";
            trueGraphic.rectTransform.sizeDelta = rectTransform.sizeDelta;

            Image targetImage = (Image)trueGraphic;
            if (targetImage)
            {
                targetImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");
            }
        }

        isOn.RegisterListener(ToggleEffects);

        ToggleEffects();
    }
#endif

    /// <summary>
    /// Registers listener.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        isOn.RegisterListener(ToggleEffects);
    }

    /// <summary>
    /// Sets toggle state to match bool scriptable value when object is activated.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        if (Application.isPlaying)
        {
            ToggleEffects();
        }
    }

    /// <summary>
    /// Unregisters listener.
    /// </summary>
    protected override void OnDestroy()
    {
        isOn.UnregisterListener(ToggleEffects);

        base.OnDestroy();
    }

    /// <summary>
    /// Sets highlight colors for default transition.
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (toggleTransition != ToggleTransition.Default) return;

        targetGraphic.color = colors.highlightedColor;
        trueGraphic.color = colors.highlightedColor;
    }

    /// <summary>
    /// Sets unhighlighted colors for default transition.
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerExit(PointerEventData eventData)
    {
        if(toggleTransition != ToggleTransition.Default) return;

        targetGraphic.color = colors.normalColor;
        trueGraphic.color = colors.normalColor;
    }

    /// <summary>
    /// Updates colors and calls InternalToggle.
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (toggleTransition == ToggleTransition.Default)
        {
            targetGraphic.color = colors.pressedColor;
            trueGraphic.color = colors.pressedColor;
        }

        InternalToggle();
    }

    /// <summary>
    /// Registers an action as a listener.
    /// </summary>
    /// <param name="action">Action to be registered</param>
    public void RegisterListener(System.Action action)
    {
        onValueChangedAction += action;
    }

    /// <summary>
    /// Ungregisters an action as a listener.
    /// </summary>
    /// <param name="action">Action to be unregistered</param>
    public void UnregisterListener(System.Action action)
    {
        onValueChangedAction -= action;
    }

    /// <summary>
    /// Registers an action with a bool parameter as a listener.
    /// </summary>
    /// <param name="action">Action with a bool param to be registered.</param>
    public void RegisterListener(System.Action<bool> action)
    {
        onValueChangedActionParam += action;
    }

    /// <summary>
    /// Unregisters an action with a bool parameter as a listener.
    /// </summary>
    /// <param name="action">Action with a bool param to be registered.</param>
    public void UnregisterListener(System.Action<bool> action)
    {
        onValueChangedActionParam -= action;
    }

    /// <summary>
    /// Invokes toggle event and registered actions.
    /// </summary>
    private void Invoke()
    {
        onValueChanged.Invoke(isOn);
        onValueChangedAction?.Invoke();
        onValueChangedActionParam?.Invoke(isOn);
    }

    /// <summary>
    /// Toggles the value and calls Invoke.
    /// Automatically calls ToggleEffects since it is registered as a listener to isOn.
    /// </summary>
    protected virtual void InternalToggle()
    {
        isOn.Value = !isOn;
        Invoke();
    }

    /// <summary>
    /// Sets the transition effects.
    /// </summary>
    private void ToggleEffects()
    {
        if(trueGraphic == null || targetGraphic == null) return;

        switch (toggleTransition)
        {
            case ToggleTransition.Default:
                trueGraphic.canvasRenderer.SetAlpha(isOn ? 1.0f : 0.0f);
                break;
            case ToggleTransition.SwapActive:
                trueGraphic?.gameObject.SetActive(isOn);
                targetGraphic?.gameObject.SetActive(!isOn);
                break;
            case ToggleTransition.Custom:
                CustomToggleEffect();
                break;
            default:
                trueGraphic?.CrossFadeAlpha(isOn ? 1.0f : 0.0f, 0.0f, true);
                break;
        }
    }

    /// <summary>
    /// Override to add custom transition effects.
    /// </summary>
    protected virtual void CustomToggleEffect() { }
    #endregion
}
