using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
// PauseManager handles pausing and resuming the game. The pause menu shows
// when the player presses Escape (keyboard) or Start (gamepad), and offers
// Resume, Restart, and Main Menu buttons.
//
// We do NOT use Time.timeScale = 0 because it breaks UI input with the new
// Input System. Instead we disable the individual components that produce
// motion or sound (PlayerController, ZombieController, NavMeshAgent, AudioSource)
// while keeping the GameObjects active so the camera can still follow the
// player and zombies can still be found by tag.
public class PauseManager : MonoBehaviour
{
    [Header("References")]
    // The Pause panel to show/hide. Drag PausePanel in the Inspector.
    public GameObject pausePanel;
    // The WaveManager so we can disable it during pause.
    public WaveManager waveManager;
    // The GameManager so we can check if the game has already ended.
    public GameManager gameManager;

    // Current pause state.
    private bool isPaused = false;

    void Start()
    {
        // Make sure the panel is hidden at the start of the game.
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        bool pauseKeyPressed = false;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            pauseKeyPressed = true;
        }
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            pauseKeyPressed = true;
        }

        if (pauseKeyPressed)
        {
            // Don't pause if the game has already ended (death or win).
            if (gameManager != null && (IsPanelActive(gameManager.gameOverPanel) || IsPanelActive(gameManager.winPanel)))
            {
                return;
            }
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    bool IsPanelActive(GameObject panel)
    {
        return panel != null && panel.activeSelf;
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);

        // Freeze the player: stop input, stop velocity, mute audio.
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.enabled = false;
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = Vector3.zero;
            AudioSource audio = player.GetComponent<AudioSource>();
            if (audio != null)
            {
                // Stop any one-shot sounds already playing (gunshot, hit, etc.) before muting.
                audio.Stop();
                audio.enabled = false;
            }
        }

        // Freeze every active zombie. We do this carefully because new zombies
        // spawning on the same frame as the pause can sneak through if we don't
        // also explicitly stop their NavMeshAgent and AudioSource.
        ZombieController[] zombies = FindObjectsByType<ZombieController>(FindObjectsInactive.Exclude);
        foreach (ZombieController z in zombies)
        {
            z.enabled = false;
            NavMeshAgent agent = z.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                // Stop the agent first so it can't take another step, then disable it.
                if (agent.isOnNavMesh) agent.isStopped = true;
                agent.velocity = Vector3.zero;
                agent.enabled = false;
            }
            AudioSource audio = z.GetComponent<AudioSource>();
            if (audio != null)
            {
                // Stop any currently-playing groan so it doesn't keep playing after pause.
                audio.Stop();
                audio.enabled = false;
            }
        }

        if (waveManager != null) waveManager.enabled = false;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);

        // Unfreeze the player.
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.enabled = true;
            AudioSource audio = player.GetComponent<AudioSource>();
            if (audio != null) audio.enabled = true;
        }

        // Unfreeze every zombie.
        ZombieController[] zombies = FindObjectsByType<ZombieController>(FindObjectsInactive.Exclude);
        foreach (ZombieController z in zombies)
        {
            z.enabled = true;
            NavMeshAgent agent = z.GetComponent<NavMeshAgent>();
            if (agent != null) agent.enabled = true;
            AudioSource audio = z.GetComponent<AudioSource>();
            if (audio != null) audio.enabled = true;
        }

        if (waveManager != null) waveManager.enabled = true;
    }

    public void RestartFromPause()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}