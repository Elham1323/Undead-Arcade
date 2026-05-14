using UnityEngine;

// Bullet flies forward, destroys itself after a short time, and deals damage on hit.
// It doesn't care WHAT it hits - if the target has a Health component, that component
// handles the damage. This keeps Bullet simple and reusable.
public class Bullet : MonoBehaviour
{
    // How fast the bullet flies. Tunable in the Inspector.
    public float speed = 20f;

    // How long the bullet lives before destroying itself, in seconds.
    public float lifetime = 2f;

    // How much damage this bullet does on hit. Tunable per bullet type.
    public float damage = 10f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Push the bullet forward as soon as it spawns.
        rb.linearVelocity = transform.forward * speed;

        // Schedule self-destruction after `lifetime` seconds.
        Destroy(gameObject, lifetime);
    }

    // Called when the bullet's trigger collider touches another collider.
    void OnTriggerEnter(Collider other)
    {
        // Ignore the player so the bullet doesn't despawn the moment it leaves the gun.
        if (other.CompareTag("Player")) return;

        // Try to find a Health component on whatever we hit. If it has one, damage it.
        // TryGetComponent is cleaner than GetComponent + null check - it returns true/false.
        if (other.TryGetComponent<Health>(out Health targetHealth))
        {
            targetHealth.TakeDamage(damage);
        }

        // Destroy the bullet on any hit (zombie, wall, anything that isn't the player).
        Destroy(gameObject);
    }
}