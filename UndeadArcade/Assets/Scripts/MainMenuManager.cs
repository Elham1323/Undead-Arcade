using UnityEngine;
using UnityEngine.SceneManagement;
// MainMenuManager handles the buttons on the main menu scene.
// PlayGame loads the GameScene to start playing.
// QuitGame exits the application (in builds) or stops Play mode (in editor).
public class MainMenuManager : MonoBehaviour
{
    // The name of the gameplay scene to load when Play is pressed.
    // Has to match the scene's filename exactly and the scene has to be
    // added to Build Settings or it won't load.
    public string gameSceneName = "GameScene";

    // Called by the Play button. Loads the main game scene.
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Called by the Quit button. Closes the game.
    // In the editor, this stops Play mode instead since Application.Quit
    // does nothing in the editor.
    public void QuitGame()
    {
        Debug.Log("Quit pressed.");
#if UNITY_EDITOR
        // Stops Play mode when running in the editor.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Closes the game when running in a build (standalone exe or WebGL).
        Application.Quit();
#endif
    }
}