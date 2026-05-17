using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
// GamepadMenuNav navigates menus from the arcade joystick.
// Reads the left stick once per push (no repeat). The stick must fully
// return to neutral before another push registers, which prevents
// fast multi-skip when the arcade stick rapidly crosses thresholds.
public class GamepadMenuNav : MonoBehaviour
{
    // The stick must exceed this value to count as a push.
    public float deadzone = 0.7f;
    // The stick must fall below this to be considered "released".
    public float releaseThreshold = 0.3f;
    // Tracks whether the stick is currently in a "pushed" state on each axis.
    private bool verticalPushed = false;
    private bool horizontalPushed = false;

    void Update()
    {
        if (Gamepad.current == null) return;
        Vector2 stick = Gamepad.current.leftStick.ReadValue();

        // Vertical handling.
        if (!verticalPushed)
        {
            // Waiting for a fresh push.
            if (stick.y > deadzone)
            {
                SelectNextInDirection(Direction.Up);
                verticalPushed = true;
            }
            else if (stick.y < -deadzone)
            {
                SelectNextInDirection(Direction.Down);
                verticalPushed = true;
            }
        }
        else
        {
            // Currently pushed. Wait for release.
            if (Mathf.Abs(stick.y) < releaseThreshold)
            {
                verticalPushed = false;
            }
        }

        // Horizontal handling.
        if (!horizontalPushed)
        {
            if (stick.x > deadzone)
            {
                SelectNextInDirection(Direction.Right);
                horizontalPushed = true;
            }
            else if (stick.x < -deadzone)
            {
                SelectNextInDirection(Direction.Left);
                horizontalPushed = true;
            }
        }
        else
        {
            if (Mathf.Abs(stick.x) < releaseThreshold)
            {
                horizontalPushed = false;
            }
        }
    }

    enum Direction { Up, Down, Left, Right }

    void SelectNextInDirection(Direction dir)
    {
        if (EventSystem.current == null) return;
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        if (currentSelected == null) return;
        Selectable currentSelectable = currentSelected.GetComponent<Selectable>();
        if (currentSelectable == null) return;

        Selectable next = null;
        switch (dir)
        {
            case Direction.Up: next = currentSelectable.FindSelectableOnUp(); break;
            case Direction.Down: next = currentSelectable.FindSelectableOnDown(); break;
            case Direction.Left: next = currentSelectable.FindSelectableOnLeft(); break;
            case Direction.Right: next = currentSelectable.FindSelectableOnRight(); break;
        }
        if (next != null)
        {
            Debug.Log("Nav from " + currentSelected.name + " to " + next.gameObject.name + " (" + dir + ")");
            EventSystem.current.SetSelectedGameObject(next.gameObject);
        }
    }
}