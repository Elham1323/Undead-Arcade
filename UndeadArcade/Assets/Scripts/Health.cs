using UnityEngine;
using UnityEngine.Events;
// Health is a simple, reusable component for anything that can take damage.
// Put it on the Player, the Zombie, or any future entity that has HP.
// It only knows about its own HP - other scripts decide what to do when it dies.
public class Health : MonoBehaviour
{
    // Maximum HP. Public so I can set different values for the Player and Zombie in the Inspector.
    public float maxHealth = 100f;
    // Current HP. Starts at max when the scene loads.
    private float currentHealth;
    // Event that fires when this entity dies (HP hits 0).
    // Other scripts can subscribe to this to react - e.g. the GameManager can listen
    // to the Player's death event to trigger a Game Over screen.
    // Using UnityEvent so it's also hookable in the Inspector.
    public UnityEvent OnDied;
    void Awake()
    {
        // Start with full HP.
        currentHealth = maxHealth;
    }
    // Called by anything that wants to deal damage to this entity.
    // For example, the Bullet script calls this when it hits something.
    public void TakeDamage(float amount)
    {
        // Don't do anything if we're already dead.
        if (currentHealth <= 0) return;
        currentHealth -= amount;
        // Optional: log the hit for debugging. We can remove this later.
        Debug.Log(gameObject.name + " took " + amount + " damage. HP: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    // Called when HP reaches 0. Fires the death event so other scripts can react.
    void Die()
    {
        Debug.Log(gameObject.name + " died.");
        OnDied?.Invoke();
    }
    // Lets other scripts (like ZombieController reading from a ScriptableObject) set the
    // max HP at runtime. Refills current HP to the new max so we start "fresh".
    public void SetMaxHealth(float newMax)
    {
        maxHealth = newMax;
        currentHealth = newMax;
    }
    // Useful for the HUD later when we add it.
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
}