using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
// PauseManager handles pausing and resuming the game. The pause menu shows
// when the player presses Escape (keyboard) or the north button (gamepad/arcade),
// and offers Resume, Restart, and Main Menu buttons.
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

    [Header("Default Selected Button")]
    // The button to auto-select when the pause panel opens. Drag ResumeButton in.
    public GameObject firstButton;

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
        // The VIA Arcade does not have a Start button, so we use the north (Y) button
        // which maps to one of the colored buttons on the arcade panel via Steam Input.
        if (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
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
        // Auto-select the resume button so joystick / arcade navigation can use it.
        if (firstButton != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }

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
                audio.Stop();
                audio.enabled = false;
            }
        }

        ZombieController[] zombies = FindObjectsByType<ZombieController>(FindObjectsInactive.Exclude);
        foreach (ZombieController z in zombies)
        {
            z.enabled = false;
            NavMeshAgent agent = z.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                if (agent.isOnNavMesh) agent.isStopped = true;
                agent.velocity = Vector3.zero;
                agent.enabled = false;
            }
            AudioSource audio = z.GetComponent<AudioSource>();
            if (audio != null)
            {
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

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.enabled = true;
            AudioSource audio = player.GetComponent<AudioSource>();
            if (audio != null) audio.enabled = true;
        }

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