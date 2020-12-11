using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ColdironTools.Scriptables;

[RequireComponent(typeof(RectTransform)), CanEditMultipleObjects]
public class UIToggle : Selectable, IPointerClickHandler
{
    [System.Serializable]
    public enum ToggleTransition
    {
        Default, SwapActive, Custom
    }

    [Header("Toggle Options")]
    [SerializeField] private Graphic trueGraphic = null;
    [SerializeField] private ToggleTransition toggleTransition = ToggleTransition.Default;
    [SerializeField] private BoolScriptableReference isOn = new BoolScriptableReference(true);
    [SerializeField] private Toggle.ToggleEvent onValueChanged = new Toggle.ToggleEvent();
    [SerializeField] private string audioDelegate = "SWITCH";

    private RectTransform rectTransform = null;
    private System.Action onValueChangedAction;
    private System.Action<bool> onValueChangedActionParam;

    protected override void OnValidate()
    {
        base.OnValidate();

        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(20.0f, 20.0f);
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

    protected override void OnDestroy()
    {
        isOn.UnregisterListener(ToggleEffects);

        base.OnDestroy();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (toggleTransition != ToggleTransition.Default) return;

        targetGraphic.color = colors.highlightedColor;
        trueGraphic.color = colors.highlightedColor;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if(toggleTransition != ToggleTransition.Default) return;

        targetGraphic.color = colors.normalColor;
        trueGraphic.color = colors.normalColor;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (toggleTransition == ToggleTransition.Default)
        {
            targetGraphic.color = colors.pressedColor;
            trueGraphic.color = colors.pressedColor;
        }

        InternalToggle();
        AudioEventManager.TriggerClip(audioDelegate, this);
    }

    public void OnRegister(System.Action action)
    {
        onValueChangedAction += action;
    }

    public void OnUnregister(System.Action action)
    {
        onValueChangedAction -= action;
    }

    public void OnRegister(System.Action<bool> action)
    {
        onValueChangedActionParam += action;
    }

    public void OnUnregister(System.Action<bool> action)
    {
        onValueChangedActionParam -= action;
    }

    private void Invoke()
    {
        onValueChanged.Invoke(isOn);
        onValueChangedAction?.Invoke();
        onValueChangedActionParam?.Invoke(isOn);
    }

    protected virtual void InternalToggle()
    {
        isOn.Value = !isOn;
        Invoke();
    }

    private void ToggleEffects()
    {
        if(trueGraphic == null || targetGraphic == null) return;

        switch (toggleTransition)
        {
            case ToggleTransition.Default:
                trueGraphic?.CrossFadeAlpha(isOn ? 1.0f : 0.0f, 0.0f, true);
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

    protected virtual void CustomToggleEffect() { }
}
