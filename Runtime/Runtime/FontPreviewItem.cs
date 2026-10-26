
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// One entry in the modal: a background/text pair at a particular font size.
public class FontPreviewItem : MonoBehaviour
{
    [Header("Sample")]
    [SerializeField] Image backgroundImage;
    [SerializeField] TMP_Text sampleText;
    [SerializeField] TMP_Text variantLabel;

    [Header("Style Buttons")]
    [SerializeField] Button boldButton;
    [SerializeField] Button italicButton;
    [SerializeField] Button underlineButton;
    [SerializeField] Button smallCapsButton;
    [SerializeField] Button uppercaseButton;

    const string Checkmark = "\u2713 ";

    TMP_Text _boldLabel;
    TMP_Text _italicLabel;
    TMP_Text _underlineLabel;
    TMP_Text _smallCapsLabel;
    TMP_Text _uppercaseLabel;

    FontStyles _styles;

    void Awake()
    {
        _boldLabel = FindLabel(boldButton);
        _italicLabel = FindLabel(italicButton);
        _underlineLabel = FindLabel(underlineButton);
        _smallCapsLabel = FindLabel(smallCapsButton);
        _uppercaseLabel = FindLabel(uppercaseButton);

        if (boldButton != null)
            boldButton.onClick.AddListener(ToggleBold);

        if (italicButton != null)
            italicButton.onClick.AddListener(ToggleItalic);

        if (underlineButton != null)
            underlineButton.onClick.AddListener(ToggleUnderline);

        if (smallCapsButton != null)
            smallCapsButton.onClick.AddListener(ToggleSmallCaps);

        if (uppercaseButton != null)
            uppercaseButton.onClick.AddListener(ToggleUppercase);

        UpdateButtonLabels();
    }

    void OnDestroy()
    {
        if (boldButton != null)
            boldButton.onClick.RemoveListener(ToggleBold);

        if (italicButton != null)
            italicButton.onClick.RemoveListener(ToggleItalic);

        if (underlineButton != null)
            underlineButton.onClick.RemoveListener(ToggleUnderline);

        if (smallCapsButton != null)
            smallCapsButton.onClick.RemoveListener(ToggleSmallCaps);

        if (uppercaseButton != null)
            uppercaseButton.onClick.RemoveListener(ToggleUppercase);
    }

    public void Initialize(
        TMP_FontAsset font,
        string fullText,
        string sizeName,
        float fontSize,
        Color textColor,
        Color backgroundColor)
    {
        _styles = FontStyles.Normal;

        if (sampleText != null)
        {
            sampleText.font = font;
            sampleText.text = fullText;
            sampleText.color = textColor;
            sampleText.enableAutoSizing = false;
            sampleText.fontSize = fontSize;
            sampleText.fontStyle = _styles;
        }

        if (backgroundImage != null)
            backgroundImage.color = backgroundColor;

        if (variantLabel != null)
        {
            string fg = ColorUtility.ToHtmlStringRGB(textColor);
            string bg = ColorUtility.ToHtmlStringRGB(backgroundColor);

            variantLabel.text =
                $"• {sizeName} ({fontSize:0})\n• Text #{fg}\n• BG #{bg}";
        }

        UpdateButtonLabels();
    }

    static TMP_Text FindLabel(Button button) =>
        button != null
            ? button.GetComponentInChildren<TMP_Text>(true)
            : null;

    void ToggleBold() =>
        Toggle(FontStyles.Bold);

    void ToggleItalic() =>
        Toggle(FontStyles.Italic);

    void ToggleUnderline() =>
        Toggle(FontStyles.Underline);

    void ToggleSmallCaps() =>
        Toggle(FontStyles.SmallCaps);

    void ToggleUppercase() =>
        Toggle(FontStyles.UpperCase);

    void Toggle(FontStyles flag)
    {
        _styles = (_styles & flag) != 0
            ? _styles & ~flag
            : _styles | flag;

        if (sampleText != null)
            sampleText.fontStyle = _styles;

        UpdateButtonLabels();
    }

    void UpdateButtonLabels()
    {
        SetButtonLabel(_boldLabel, "Bold", FontStyles.Bold);
        SetButtonLabel(_italicLabel, "Italic", FontStyles.Italic);
        SetButtonLabel(_underlineLabel, "Underline", FontStyles.Underline);
        SetButtonLabel(_smallCapsLabel, "Small Caps", FontStyles.SmallCaps);
        SetButtonLabel(_uppercaseLabel, "Uppercase", FontStyles.UpperCase);
    }

    void SetButtonLabel(
        TMP_Text label,
        string caption,
        FontStyles flag)
    {
        if (label != null)
        {
            label.text = ((_styles & flag) != 0
                ? Checkmark
                : "") + caption;
        }
    }
}
