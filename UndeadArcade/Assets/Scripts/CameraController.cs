using UnityEngine;

// CameraController makes the camera follow the player smoothly from a top-down angle.
// Uses an offset (set in the Inspector) so we can adjust the camera height and angle without code.
public class CameraController : MonoBehaviour
{
    // The target to follow. Set this to the Player in the Inspector.
    public Transform target;

    // The offset from the target to the camera (e.g. (0, 18, -12) for a top-down 65 degree angle).
    // Set this in the Inspector or it gets calculated automatically on Start.
    public Vector3 offset;

    // How quickly the camera catches up to the player.
    // Lower = smoother but laggier, higher = snappier but jitter on fast moves.
    public float followSpeed = 5f;

    void Start()
    {
        // If no offset was set in the Inspector, calculate it from the camera's current position.
        // This way I can just position the camera where I want in the editor and it "just works."
        if (offset == Vector3.zero && target != null)
        {
            offset = transform.position - target.position;
        }
    }

    // Using LateUpdate so the camera moves AFTER the player moves each frame.
    // This avoids the camera lagging one frame behind the player.
    void LateUpdate()
    {
        if (target == null) return;

        // The position the camera wants to be at.
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move toward the desired position instead of snapping.
        // Lerp interpolates between the camera's current position and the target.
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}