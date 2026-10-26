using TMPro;
using UnityEngine;

public class FontGalleryController : MonoBehaviour
{
    [Header("Fonts and Colors")]
    [SerializeField] TMP_FontAsset systemFont;
    [SerializeField] TMP_FontAsset[] fonts;
    [SerializeField] Color[] colors = { Color.white, Color.black };

    [Header("Text")]
    [SerializeField, TextArea(2, 5)]
    string previewText = "The quick brown fox jumps over the lazy dog.";

    [Tooltip("A .txt file containing the full sample text displayed in every preview combination.")]
    [SerializeField] TextAsset fullTextAsset;

    [Header("Gallery")]
    [SerializeField] FontGalleryEntry entryPrefab;
    [SerializeField] Transform entriesRoot;

    [Header("Full-screen Preview")]
    [SerializeField] FontPreviewWindow previewWindow;

    void Start()
    {
        if (previewWindow != null)
            previewWindow.Close();

        BuildGallery();
    }

    void BuildGallery()
    {
        if (entryPrefab == null || entriesRoot == null)
        {
            Debug.LogError("Font Gallery: Assign an entry prefab and entries root.", this);
            return;
        }

        if (fonts == null)
            return;

        foreach (var font in fonts)
        {
            if (font == null)
                continue;

            var entry = Instantiate(entryPrefab, entriesRoot);
            entry.Initialize(this, font, previewText);
        }
    }

    public void OnEntryClick(FontGalleryEntry entry)
    {
        if (entry == null || entry.Font == null)
            return;

        if (previewWindow == null || fullTextAsset == null)
        {
            Debug.LogError("Font Gallery: Assign a preview window and a full-text TextAsset.", this);
            return;
        }

        previewWindow.Open(entry.Font, fullTextAsset.text, colors);
    }

    public void ClosePreview()
    {
        if (previewWindow != null)
            previewWindow.Close();
    }
}
