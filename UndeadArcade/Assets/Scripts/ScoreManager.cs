using UnityEngine;
using UnityEngine.Events;

// ScoreManager tracks the player's score during a run.
// Zombies don't know about score - they just call AddScore() when they die.
// The HUD listens to OnScoreChanged to update the display.
public class ScoreManager : MonoBehaviour
{
    // How many points each zombie kill is worth.
    public int pointsPerZombie = 10;

    // Current score.
    private int currentScore = 0;

    // Fires whenever the score changes. The HUD subscribes to this to update the text.
    public UnityEvent<int> OnScoreChanged;

    // Called by anything that wants to add to the score (e.g. ZombieController on death).
    public void AddScore(int amount)
    {
        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);
    }

    // Helper for code that just wants to add a zombie kill without specifying the value.
    public void AddZombieKill()
    {
        AddScore(pointsPerZombie);
    }

    // Reset back to zero - useful when restarting the game.
    public void ResetScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }

    public int GetCurrentScore() => currentScore;
}