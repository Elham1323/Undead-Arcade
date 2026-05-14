using UnityEngine;
using UnityEngine.AI;

// ZombieController makes the zombie chase the player using Unity's NavMesh.
// Uses a basic Finite State Machine (FSM) with three states:
// Chase = walking toward the player
// Attack = close enough to damage them on a timer
// Dead = no longer active, scheduled for destruction
public class ZombieController : MonoBehaviour
{
    private enum ZombieState { Chase, Attack, Dead }
    private ZombieState currentState = ZombieState.Chase;

    private NavMeshAgent agent;
    private Transform player;
    private Health playerHealth;
    private Health myHealth;

    [Header("Attack")]
    // How close the zombie has to be to start attacking.
    public float attackRange = 1.5f;

    // How much damage one bite does.
    public float damagePerHit = 20f;

    // How many seconds between bites.
    public float attackInterval = 1f;

    // When the next attack is allowed.
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myHealth = GetComponent<Health>();

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
                break;

            case ZombieState.Attack:
                AttackPlayer();
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
        if (distance <= attackRange)
        {
            currentState = ZombieState.Attack;
        }
    }

    void AttackPlayer()
    {
        // If the player walked away, go back to chasing.
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            currentState = ZombieState.Chase;
            return;
        }

        // Damage the player on a timer.
        if (Time.time >= nextAttackTime && playerHealth != null)
        {
            playerHealth.TakeDamage(damagePerHit);
            nextAttackTime = Time.time + attackInterval;
        }
    }

    // Called when our Health component fires its OnDied event.
    void OnDied()
    {
        currentState = ZombieState.Dead;

        // Stop pathfinding so we don't keep walking.
        if (agent != null) agent.isStopped = true;

        // Remove the zombie from the scene.
        // Slight delay so any "death sound" or particle effect we add later has time to play.
        Destroy(gameObject, 0.2f);
    }
}