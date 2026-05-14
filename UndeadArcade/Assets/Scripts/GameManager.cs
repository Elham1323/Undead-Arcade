using UnityEngine;

// GameManager handles top-level game state: pausing, game over, win condition.
// For Milestone 1 it only handles the Player's death by freezing the game.
// We'll expand this in Milestone 2 with the wave system, score, and proper UI.
public class GameManager : MonoBehaviour
{
    // The Player's Health component. Assigned in the Inspector.
    public Health playerHealth;

    void Start()
    {
        // Subscribe to the Player's death event.
        // When the Player's Health fires OnDied, our HandlePlayerDeath method runs.
        if (playerHealth != null)
        {
            playerHealth.OnDied.AddListener(HandlePlayerDeath);
        }
        else
        {
            Debug.LogError("GameManager: playerHealth is not assigned in the Inspector!");
        }
    }

    // Called when the Player's Health fires OnDied.
    void HandlePlayerDeath()
    {
        Debug.Log("Game Over! Player has died.");

        // Freeze time so everything stops moving (zombies, bullets, animations).
        // Time.timeScale = 0 means the game world is paused.
        // We'll show a proper Game Over screen in Milestone 2.
        Time.timeScale = 0f;
    }
}