using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// Changes a TextMeshPro text's color when the mouse hovers over its parent button.
// Attach this to a Button GameObject that has a TMP text as a child.
// The Button still handles clicks normally - this script only controls the color.
// We use IPointerEnterHandler and IPointerExitHandler so the UI event system tells us
// when the mouse goes in and out, instead of polling every frame.
public class ButtonHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // The TMP text whose color we want to change. Drag the Text (TMP) child in the Inspector.
    public TextMeshProUGUI targetText;

    // Color when the mouse is NOT over the button (the default).
    public Color normalColor = Color.white;

    // Color when the mouse IS over the button (the highlight).
    public Color hoverColor = Color.red;

    void Start()
    {
        // Make sure the text starts in the normal color.
        if (targetText != null)
        {
            targetText.color = normalColor;
        }
    }

    // Called by the UI event system when the mouse enters the button.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetText != null)
        {
            targetText.color = hoverColor;
        }
    }

    // Called by the UI event system when the mouse leaves the button.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetText != null)
        {
            targetText.color = normalColor;
        }
    }
}