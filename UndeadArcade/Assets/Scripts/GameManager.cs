using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
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

    [Header("Default Selected Buttons")]
    // The button to auto-select when GameOverPanel appears. Drag RestartButton in.
    public GameObject gameOverFirstButton;
    // The button to auto-select when WinPanel appears. Drag RestartButtonWin in.
    public GameObject winFirstButton;

    [Header("Audio")]
    // The death sound played when the player dies. Drag Death in the Inspector.
    public AudioClip deathSound;
    // Volume of the death sound (0 to 1).
    [Range(0f, 1f)]
    public float deathVolume = 0.7f;

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
        // Auto-select the restart button so joystick / arcade navigation can use it
        // without needing a mouse. Must be done after SetActive(true).
        if (gameOverFirstButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(gameOverFirstButton);
        }
        // Play the death sound through the Player's AudioSource, since it's already 2D and exists.
        PlayDeathSound();
        // Freeze the gameplay manually instead of using Time.timeScale = 0.
        FreezeGameplay();
    }
    // Called when the WaveManager fires OnAllWavesCleared.
    void HandleWin()
    {
        if (gameEnded) return;
        gameEnded = true;
        Debug.Log("You survived all waves!");
        // Show the Win panel.
        if (winPanel != null) winPanel.SetActive(true);
        // Auto-select the restart button for joystick / arcade navigation.
        if (winFirstButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(winFirstButton);
        }
        FreezeGameplay();
    }
    // Plays the death sound through the Player's AudioSource (which is 2D, so no spatial delay).
    void PlayDeathSound()
    {
        if (deathSound == null) return;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null && playerObject.TryGetComponent<AudioSource>(out AudioSource playerAudio))
        {
            playerAudio.PlayOneShot(deathSound, deathVolume);
        }
    }
    // Disables all gameplay scripts so the world goes still while a panel is showing.
    void FreezeGameplay()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) player.enabled = false;
        ZombieController[] zombies = FindObjectsByType<ZombieController>(FindObjectsInactive.Exclude);
        foreach (ZombieController z in zombies)
        {
            z.enabled = false;
            UnityEngine.AI.NavMeshAgent agent = z.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.isStopped = true;
        }
        if (waveManager != null) waveManager.enabled = false;
    }
    // Called by the Restart button on the panels. Reloads the current scene.
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}