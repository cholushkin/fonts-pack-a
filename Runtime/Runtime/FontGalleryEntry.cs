using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FontGalleryEntry : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text previewLabel;
    [SerializeField] TMP_Text nameLabel;

    FontGalleryController _controller;

    public TMP_FontAsset Font { get; private set; }

    public void Initialize(
        FontGalleryController controller,
        TMP_FontAsset font,
        string previewText)
    {
        _controller = controller;
        Font = font;

        if (previewLabel != null)
        {
            previewLabel.font = font;
            previewLabel.text = previewText;
        }

        if (nameLabel != null)
            nameLabel.text = font.name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            _controller?.OnEntryClick(this);
    }
}
