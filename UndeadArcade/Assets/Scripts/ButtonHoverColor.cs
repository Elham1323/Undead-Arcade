using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// Changes a TextMeshPro text's color when the mouse hovers over its parent button
// OR when the button gets selected via keyboard/gamepad navigation.
// Attach this to a Button GameObject that has a TMP text as a child.
// The Button still handles clicks normally - this script only controls the color.
//
// IPointerEnter / IPointerExit fire for mouse hover.
// ISelectHandler / IDeselectHandler fire for keyboard/gamepad navigation.
// Both fire to the same color so the visual feedback is consistent.
public class ButtonHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    // The TMP text whose color we want to change. Drag the Text (TMP) child in the Inspector.
    public TextMeshProUGUI targetText;

    // Color when the button is NOT highlighted (the default).
    public Color normalColor = Color.white;

    // Color when the button IS highlighted (mouse hover OR navigation select).
    public Color hoverColor = Color.red;

    void Start()
    {
        if (targetText != null)
        {
            targetText.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Unhighlight();
    }

    // Called when the button becomes the selected GameObject (gamepad/keyboard nav).
    public void OnSelect(BaseEventData eventData)
    {
        Highlight();
    }

    // Called when the button stops being the selected GameObject.
    public void OnDeselect(BaseEventData eventData)
    {
        Unhighlight();
    }

    void Highlight()
    {
        if (targetText != null)
        {
            targetText.color = hoverColor;
        }
    }

    void Unhighlight()
    {
        if (targetText != null)
        {
            targetText.color = normalColor;
        }
    }
}