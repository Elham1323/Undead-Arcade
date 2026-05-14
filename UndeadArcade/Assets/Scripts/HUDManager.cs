using UnityEngine;
using UnityEngine.UI;
using TMPro;

// HUDManager updates the on-screen HUD: health bar, wave counter, score.
// It listens to events from Health, WaveManager, and ScoreManager so it
// only updates the UI when something actually changes - cleaner than polling
// every frame.
public class HUDManager : MonoBehaviour
{
    [Header("UI References")]
    // Drag the HealthBar slider here in the Inspector.
    public Slider healthBar;

    // Drag the WaveText TextMeshPro here in the Inspector.
    public TextMeshProUGUI waveText;

    // Drag the ScoreText TextMeshPro here in the Inspector.
    public TextMeshProUGUI scoreText;

    [Header("Game References")]
    // The Player's Health. Drag the Player in - Unity grabs the Health component automatically.
    public Health playerHealth;

    // The WaveManager so we can listen to wave-changed events.
    public WaveManager waveManager;

    // The ScoreManager so we can listen to score-changed events.
    public ScoreManager scoreManager;

    void Start()
    {
        // Initialize the HUD with starting values so it doesn't show "nothing" before the first event fires.
        if (playerHealth != null)
        {
            UpdateHealthBar();
        }
        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }

        // Subscribe to all the events we care about.
        // The HUD doesn't poll - it just reacts when something changes.

        // When the score changes, update the score text.
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged.AddListener(UpdateScore);
        }

        // When a new wave starts, update the wave text.
        if (waveManager != null)
        {
            waveManager.OnWaveStarted.AddListener(UpdateWave);
        }
    }

    void Update()
    {
        // The Health component doesn't fire an event on damage (only on death),
        // so we update the health bar every frame. Cheap operation.
        if (playerHealth != null && healthBar != null)
        {
            UpdateHealthBar();
        }
    }

    // Calculate the slider's value (0 to 1) based on the player's current and max HP.
    void UpdateHealthBar()
    {
        float ratio = playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth();
        healthBar.value = ratio;
    }

    // Called when ScoreManager fires OnScoreChanged with the new total.
    void UpdateScore(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + newScore;
        }
    }

    // Called when WaveManager fires OnWaveStarted with the wave number (1-based).
    void UpdateWave(int waveNumber)
    {
        if (waveText != null && waveManager != null)
        {
            int totalWaves = waveManager.waves.Count;
            waveText.text = "Wave " + waveNumber + " / " + totalWaves;
        }
    }
}