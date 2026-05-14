using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// One wave's worth of data. Marked [System.Serializable] so it shows up in the Inspector
// as an editable list. Each wave has a count of zombies and the speed they move at.
[System.Serializable]
public class WaveData
{
    public int zombieCount = 5;
    public float zombieSpeed = 3f;
}

// WaveManager handles spawning zombies in waves and progressing the game.
// Each wave spawns N zombies at the spawn points, waits until they all die,
// then starts the next wave after a short break.
// When the final wave is cleared, fires the OnAllWavesCleared event so the GameManager
// can show a win screen.
public class WaveManager : MonoBehaviour
{
    [Header("Wave Configuration")]
    // List of waves to play through. Configure in the Inspector.
    public List<WaveData> waves = new List<WaveData>();

    // The Zombie prefab to spawn. Drag in the Inspector.
    public GameObject zombiePrefab;

    // The 4 spawn points where zombies appear. Drag them in the Inspector.
    public Transform[] spawnPoints;

    [Header("Timing")]
    // Seconds between zombies spawning in the same wave (prevents them from spawning
    // all on top of each other).
    public float spawnInterval = 0.5f;

    // Seconds of break between waves.
    public float breakBetweenWaves = 3f;

    [Header("Events")]
    // Fired when all waves are cleared. The GameManager listens for this to show the win screen.
    public UnityEvent OnAllWavesCleared;

    // Fired when a new wave starts. Useful for the HUD to update the wave counter.
    public UnityEvent<int> OnWaveStarted;

    // How many zombies are alive right now. We decrement this when a zombie dies.
    private int aliveZombieCount = 0;

    // The current wave index (0-based).
    private int currentWaveIndex = 0;

    void Start()
    {
        // Start the wave loop as a coroutine, since it involves waiting between waves.
        StartCoroutine(RunWaves());
    }

    // The main wave loop. Goes through each wave, spawns it, waits for it to clear,
    // takes a break, then moves to the next.
    IEnumerator RunWaves()
    {
        for (currentWaveIndex = 0; currentWaveIndex < waves.Count; currentWaveIndex++)
        {
            WaveData wave = waves[currentWaveIndex];

            // Tell anyone listening that a new wave is starting (wave number is 1-based for the HUD).
            OnWaveStarted?.Invoke(currentWaveIndex + 1);

            // Spawn all the zombies for this wave, one at a time.
            yield return StartCoroutine(SpawnWave(wave));

            // Wait until every zombie is dead before moving on.
            while (aliveZombieCount > 0)
            {
                yield return null;
            }

            // Short break between waves (skip after the very last one).
            if (currentWaveIndex < waves.Count - 1)
            {
                yield return new WaitForSeconds(breakBetweenWaves);
            }
        }

        // All waves are done. Fire the "you won" event.
        OnAllWavesCleared?.Invoke();
    }

    // Spawn the zombies in a wave one by one with a small interval.
    IEnumerator SpawnWave(WaveData wave)
    {
        for (int i = 0; i < wave.zombieCount; i++)
        {
            SpawnZombie(wave.zombieSpeed);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Create one zombie at the spawn point furthest from the player.
    // This keeps zombies from "popping in" right in front of the player.
    void SpawnZombie(float speed)
    {
        // Find the spawn point furthest from the player.
        Transform spawnPoint = GetFurthestSpawnPoint();

        // Add a small random offset around the spawn point so zombies don't appear
        // stacked on top of each other. Creates a more natural-looking horde.
        Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
        Vector3 spawnPosition = spawnPoint.position + randomOffset;

        // Spawn the zombie prefab at the slightly randomized position.
        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, spawnPoint.rotation);

        // Set its NavMeshAgent speed to this wave's speed.
        NavMeshAgent agent = zombie.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = speed;
        }

        // Subscribe to the zombie's death event so we know when it dies.
        Health zombieHealth = zombie.GetComponent<Health>();
        if (zombieHealth != null)
        {
            zombieHealth.OnDied.AddListener(OnZombieDied);
        }

        // Track this zombie as alive.
        aliveZombieCount++;
    }

    // Helper: returns the spawn point that's the furthest away from the player right now.
    // This is so zombies always spawn off-screen.
    Transform GetFurthestSpawnPoint()
    {
        // Find the player by tag (same way the zombie does).
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            // If we can't find the player, just pick a random one.
            return spawnPoints[Random.Range(0, spawnPoints.Length)];
        }

        Transform furthest = spawnPoints[0];
        float furthestDistance = 0f;

        // Loop through every spawn point, keep the one with the highest distance.
        foreach (Transform sp in spawnPoints)
        {
            float distance = Vector3.Distance(sp.position, player.transform.position);
            if (distance > furthestDistance)
            {
                furthestDistance = distance;
                furthest = sp;
            }
        }

        return furthest;
    }

    // Called when any zombie's Health fires OnDied.
    void OnZombieDied()
    {
        aliveZombieCount--;
    }
}