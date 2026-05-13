using UnityEngine;
using UnityEngine.InputSystem;

// PlayerController handles the player's movement based on joystick input.
// Uses the new Input System package - "Send Messages" behavior means Unity
// automatically calls OnMove() when the player moves the joystick.
public class PlayerController : MonoBehaviour
{
    // How fast the player moves. Public so I can tweak it in the Inspector.
    public float moveSpeed = 8f;

    // The Rigidbody attached to the player. We move it using physics.
    private Rigidbody rb;

    // Stores the latest joystick input (-1 to 1 on each axis).
    // Vector2 because the joystick has X and Y, but no Z.
    private Vector2 moveInput;

    // Awake runs once when the script first loads. I grab the Rigidbody
    // here so I don't have to search for it every frame.
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Called automatically by the Input System whenever the joystick moves.
    // The InputValue contains the current stick direction as a Vector2.
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // FixedUpdate runs at a fixed rate (50 times per second by default).
    // I use it for Rigidbody movement because that's what the Unity docs recommend
    // for physics-based movement - it keeps things smooth and consistent.
    void FixedUpdate()
    {
        // Convert the 2D joystick input into a 3D world direction.
        // Joystick X -> world X (left/right), joystick Y -> world Z (forward/back).
        // Y stays at 0 because we don't want the player flying up.
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        // Move the Rigidbody by setting its velocity directly.
        // This gives instant, responsive control - perfect for an arcade game.
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);
    }
}