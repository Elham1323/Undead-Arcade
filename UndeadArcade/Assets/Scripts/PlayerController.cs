using UnityEngine;
using UnityEngine.InputSystem;

// PlayerController handles the player's movement and shooting.
// Walk-and-shoot style: the player faces the direction they're moving,
// and bullets fire in that same direction. If the player stops moving,
// they keep facing the last direction so they can still shoot.
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 0.15f;

    private float nextFireTime = 0f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private InputAction fireAction;

    // The direction the player is facing. Updated whenever the player moves,
    // and remembered when they stop so they can still shoot the right way.
    // Starts pointing forward (world +Z) so the first shot doesn't go nowhere.
    private Vector3 facingDirection = Vector3.forward;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Grab the Fire action from the PlayerInput component so we can poll it directly.
        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            fireAction = playerInput.actions["Fire"];
        }
    }

    // Called by the Input System whenever the joystick moves.
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // If the player is actually moving (joystick deflected), update the facing direction.
        // The 0.1f deadzone prevents tiny stick drifts from changing the facing.
        if (moveInput.sqrMagnitude > 0.1f)
        {
            // Build a 3D direction from the 2D input.
            // Joystick X -> world X (left/right), Joystick Y -> world Z (forward/back).
            facingDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            // Rotate the player to face that direction. Using LookRotation so the
            // visual capsule and the bullet spawn point both rotate together.
            transform.rotation = Quaternion.LookRotation(facingDirection);
        }

        // Fire when the button is held AND enough time has passed since the last shot.
        if (fireAction != null && fireAction.IsPressed() && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    // Spawn a bullet at the spawn point, facing whatever direction the player is facing.
    void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null) return;

        // The spawn point's rotation matches the player's rotation (it's a child of the Player),
        // so spawning the bullet with the spawn point's rotation makes it fly that direction.
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    void FixedUpdate()
    {
        // Convert the 2D joystick input into a 3D world direction.
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);

        // Move the Rigidbody by setting its velocity directly.
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);
    }
}