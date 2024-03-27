using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes
using UnityEngine.UI; // For accessing Buttons

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;

    public GameObject settingsPanel; // Reference to the settings panel
    public GameObject titlePanel; // Reference to the settings panel

    // Start is called before the first frame update
    void Start()
    {
        // Assign button listeners
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(ToggleSettings);
        quitButton.onClick.AddListener(QuitGame);

        // Initially hide settings panel
        settingsPanel.SetActive(false);
    }

    void PlayGame()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
        SceneManager.LoadScene("Playground");
    }

    void ToggleSettings()
    {
        // This example simply shows/hides the panel. You could animate this for a smoother transition.
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        titlePanel.SetActive(!titlePanel.activeSelf);
    }

    void QuitGame()
    {
        // Note: This will only work in a build, not in the Unity editor
        Application.Quit();
    }
}