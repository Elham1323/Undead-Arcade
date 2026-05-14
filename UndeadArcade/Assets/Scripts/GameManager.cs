using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager handles top-level game state: showing the Game Over and Win panels,
// listening to player death and wave completion events, and reloading the scene
// when the player restarts.
public class GameManager : MonoBehaviour
{
    [Header("References")]
    // The Player's Health component. Drag the Player in the Inspector.
    public Health playerHealth;

    // The WaveManager so we can listen for "all waves cleared" (the win condition).
    public WaveManager waveManager;

    [Header("UI Panels")]
    // The Game Over panel - shown when the player dies. Drag GameOverPanel in.
    public GameObject gameOverPanel;

    // The Win panel - shown when all waves are cleared. Drag WinPanel in.
    public GameObject winPanel;

    // Tracks whether the game has already ended, so death + win don't fire at the same time.
    private bool gameEnded = false;

    void Start()
    {
        // Listen for the player's death.
        if (playerHealth != null)
        {
            playerHealth.OnDied.AddListener(HandlePlayerDeath);
        }
        else
        {
            Debug.LogError("GameManager: playerHealth is not assigned in the Inspector!");
        }

        // Listen for "all waves cleared" = win condition.
        if (waveManager != null)
        {
            waveManager.OnAllWavesCleared.AddListener(HandleWin);
        }
        else
        {
            Debug.LogError("GameManager: waveManager is not assigned in the Inspector!");
        }

        // Make sure both panels are hidden at the start.
        // Defensive - in case someone forgets to disable them in the editor.
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    // Called when the Player's Health fires OnDied.
    void HandlePlayerDeath()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("Game Over! Player has died.");

        // Show the Game Over panel.
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // Freeze the game so zombies stop, bullets stop, etc.
        // Time.timeScale = 0 pauses the entire physics and animation system.
        Time.timeScale = 0f;
    }

    // Called when the WaveManager fires OnAllWavesCleared.
    void HandleWin()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("You survived all waves!");

        // Show the Win panel.
        if (winPanel != null) winPanel.SetActive(true);

        // Freeze the game.
        Time.timeScale = 0f;
    }

    // Called by the Restart button on the panels. Reloads the current scene.
    public void RestartGame()
    {
        // Important: reset timeScale before reloading, otherwise the new scene
        // starts frozen and nothing works.
        Time.timeScale = 1f;

        // Reload the current scene. This resets everything to its initial state.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}