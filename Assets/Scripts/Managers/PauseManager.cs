using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Player;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Slider volumeSlider;
    public Slider sensSlider;
    public Toggle fullscreenToggle;
    public Button playMenuButton;
    public Button backMenuButton;

    private PlayerControls playerControls;
    private bool isPaused = false;

    private InputAction pauseAction;

    void Start()
    {
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerControls = playerObj.GetComponent<PlayerControls>();
            if (playerControls == null)
                Debug.LogError("PlayerControls component not found on Player tagged object!");
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene!");
        }

        pausePanel.SetActive(false);
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(SetVolume);
        if (sensSlider != null)
            sensSlider.onValueChanged.AddListener(SetSensitivity);
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        if (playMenuButton != null)
            playMenuButton.onClick.AddListener(ResumeGame);
        if (backMenuButton != null)
            backMenuButton.onClick.AddListener(BackToMenu);


        pauseAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
        pauseAction.performed += ctx => TogglePause();
        pauseAction.Enable();
    }

    private void OnDestroy()
    {
        if (pauseAction != null)
            pauseAction.Disable();
    }

    private void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void SetSensitivity(float value)
    {
        Debug.Log("SetSensitivity çağrıldı: " + value + " | playerControls: " + playerControls);
        if (playerControls != null)
        {
            playerControls.Sensitivity = value;
            PlayerPrefs.SetFloat("Sensitivity", value);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}