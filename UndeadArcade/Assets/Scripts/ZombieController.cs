using UnityEngine;
using UnityEngine.AI;
// ZombieController makes the zombie chase the player using Unity's NavMesh.
// Uses a basic Finite State Machine (FSM) with three states:
// Chase = walking toward the player
// Attack = close enough to damage them on a timer
// Dead = no longer active, scheduled for destruction
//
// Stats (health, attack range, damage, attack interval) come from a ZombieData
// ScriptableObject so they can be tweaked or swapped without editing the prefab.
// Movement speed is set by the WaveManager when spawning, so each wave can ramp
// the difficulty independently of the zombie type.
public class ZombieController : MonoBehaviour
{
    private enum ZombieState { Chase, Attack, Dead }
    private ZombieState currentState = ZombieState.Chase;
    private NavMeshAgent agent;
    private Transform player;
    private Health playerHealth;
    private Health myHealth;
    // Reference to the ScoreManager. We find it automatically at Start so we don't have
    // to drag it into every single zombie's prefab.
    private ScoreManager scoreManager;

    [Header("Stats")]
    // The ScriptableObject that holds this zombie's stats. Drag DefaultZombie in the Inspector.
    public ZombieData data;

    // When the next attack is allowed.
    private float nextAttackTime = 0f;

    [Header("Audio")]
    // The groan sound played randomly while chasing. Drag ZombieGroan in the Inspector.
    public AudioClip groanSound;
    // Minimum and maximum seconds between groans. The zombie picks a random time in this range.
    public float minGroanInterval = 3f;
    public float maxGroanInterval = 8f;
    // When the next groan should play.
    private float nextGroanTime = 0f;
    // The AudioSource on this GameObject.
    private AudioSource audioSource;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myHealth = GetComponent<Health>();
        // Grab the AudioSource we attached in the Inspector.
        audioSource = GetComponent<AudioSource>();

        // Apply the stats from the ScriptableObject to this zombie.
        // We do this in Start so the Health component is ready to receive its max value.
        if (data != null && myHealth != null)
        {
            myHealth.SetMaxHealth(data.maxHealth);
        }
        else if (data == null)
        {
            Debug.LogError("ZombieController: No ZombieData assigned! Using fallback defaults.");
        }

        // The first groan happens quickly after spawn so the zombie is audibly "alive" right away.
        // Subsequent groans use the normal random interval.
        nextGroanTime = Time.time + Random.Range(0.2f, 1.5f);

        // Find the player by tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<Health>();
        }
        else
        {
            Debug.LogError("ZombieController could not find a GameObject tagged 'Player'!");
        }
        // Find the ScoreManager in the scene. There's only one so FindAnyObjectByType is fine.
        scoreManager = FindAnyObjectByType<ScoreManager>();
        // Listen for our own death event. When we die, run the OnDied method.
        // This is the Observer pattern - we subscribe to an event and react to it.
        if (myHealth != null)
        {
            myHealth.OnDied.AddListener(OnDied);
        }
    }
    void Update()
    {
        if (player == null) return;
        switch (currentState)
        {
            case ZombieState.Chase:
                ChasePlayer();
                TryGroan();
                break;
            case ZombieState.Attack:
                AttackPlayer();
                TryGroan();
                break;
            case ZombieState.Dead:
                // Dead zombies do nothing. Destruction is handled by OnDied.
                break;
        }
    }
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(transform.position, player.position);
        // Read attack range from the ScriptableObject so the zombie variant decides what's "close enough".
        float range = (data != null) ? data.attackRange : 1.5f;
        if (distance <= range)
        {
            currentState = ZombieState.Attack;
        }
    }
    void AttackPlayer()
    {
        // If the player walked away, go back to chasing.
        float distance = Vector3.Distance(transform.position, player.position);
        float range = (data != null) ? data.attackRange : 1.5f;
        if (distance > range)
        {
            currentState = ZombieState.Chase;
            return;
        }
        // Damage the player on a timer.
        if (Time.time >= nextAttackTime && playerHealth != null)
        {
            float damage = (data != null) ? data.damagePerHit : 20f;
            float interval = (data != null) ? data.attackInterval : 1f;
            playerHealth.TakeDamage(damage);
            nextAttackTime = Time.time + interval;
        }
    }
    // Plays the groan sound when the timer hits, then picks a new random delay.
    void TryGroan()
    {
        if (Time.time < nextGroanTime) return;
        if (audioSource != null && groanSound != null)
        {
            audioSource.PlayOneShot(groanSound);
        }
        // Schedule the next groan at a random time so it feels natural.
        nextGroanTime = Time.time + Random.Range(minGroanInterval, maxGroanInterval);
    }
    // Called when our Health component fires its OnDied event.
    void OnDied()
    {
        currentState = ZombieState.Dead;
        // Stop pathfinding so we don't keep walking.
        if (agent != null) agent.isStopped = true;
        // Award score for the kill, if there's a ScoreManager in the scene.
        if (scoreManager != null)
        {
            scoreManager.AddZombieKill();
        }
        // Remove the zombie from the scene.
        // Slight delay so any "death sound" or particle effect we add later has time to play.
        Destroy(gameObject, 0.2f);
    }
}