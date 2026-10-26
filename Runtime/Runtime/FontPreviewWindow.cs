using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Put this on the full-screen modal root (with a raycastable Image).
// Pointer clicks on ordinary content close the modal. Button clicks are handled
// by the buttons themselves; ScrollRect dragging does not produce a click.
[RequireComponent(typeof(Image))]
public class FontPreviewWindow : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] TMP_Text fontNameLabel;
    [SerializeField] Button closeButton;

    [Header("Scrollable Previews")]
    [SerializeField] FontPreviewItem itemPrefab;
    [SerializeField] RectTransform entriesRoot;
    [SerializeField] ScrollRect scrollRect;

    [Header("Font Sizes")]
    [SerializeField, Min(1f)] float smallFontSize = 18f;
    [SerializeField, Min(1f)] float mediumFontSize = 30f;
    [SerializeField, Min(1f)] float largeFontSize = 48f;

    [Header("Combinations")]
    [SerializeField] bool skipMatchingTextAndBackgroundColors = true;

    readonly List<FontPreviewItem> _items = new();

    void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);
    }

    public void Open(TMP_FontAsset font, string fullText, Color[] palette)
    {
        if (font == null || itemPrefab == null || entriesRoot == null)
        {
            Debug.LogError("Font Preview: Assign a font, preview item prefab and entries root.", this);
            return;
        }

        // Ensure a usable palette even if the array is empty in the Inspector.
        if (palette == null || palette.Length == 0)
            palette = new[] { Color.white, Color.black };

        gameObject.SetActive(true);
        transform.SetAsLastSibling(); // Keep the modal in front of the gallery.
        ClearItems();

        if (fontNameLabel != null)
            fontNameLabel.text = font.name;

        AddCombinations(font, fullText, palette, "Small", smallFontSize);
        AddCombinations(font, fullText, palette, "Medium", mediumFontSize);
        AddCombinations(font, fullText, palette, "Large", largeFontSize);

        // If every supplied color is identical, still display a usable example.
        if (_items.Count == 0)
            CreateItem(font, fullText, "Medium", mediumFontSize, Color.black, Color.white);

        if (scrollRect != null)
        {
            scrollRect.StopMovement();
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    void AddCombinations(
        TMP_FontAsset font,
        string fullText,
        Color[] palette,
        string sizeName,
        float fontSize)
    {
        foreach (var backgroundColor in palette)
        {
            foreach (var textColor in palette)
            {
                var background = Opaque(backgroundColor);
                var foreground = Opaque(textColor);

                if (skipMatchingTextAndBackgroundColors &&
                    background.Equals(foreground))
                    continue;

                CreateItem(font, fullText, sizeName, fontSize, foreground, background);
            }
        }
    }

    void CreateItem(
        TMP_FontAsset font,
        string fullText,
        string sizeName,
        float fontSize,
        Color textColor,
        Color backgroundColor)
    {
        var item = Instantiate(itemPrefab, entriesRoot);
        item.Initialize(font, fullText, sizeName, fontSize, textColor, backgroundColor);
        _items.Add(item);
    }

    static Color Opaque(Color color)
    {
        color.a = 1f;
        return color;
    }

    void ClearItems()
    {
        foreach (var item in _items)
        {
            if (item == null)
                continue;

            item.gameObject.SetActive(false); // Also hides it before Destroy's end-of-frame cleanup.
            Destroy(item.gameObject);
        }

        _items.Clear();
    }

 
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
