using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        SetCursorVisibility();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetCursorVisibility();
    }

    private void SetCursorVisibility()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "PrisonScene" || sceneName == "FirstScene")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LoseGame()
    {
        UIManager.Instance.ShowWastedImage(() =>
        {
            Time.timeScale = 0.1f;
            foreach (var audio in FindObjectsOfType<AudioSource>())
            {
                audio.pitch = 0.3f;
            }
            Debug.Log("Game Over");
            StartCoroutine(GoToLoseScene());
        });
    }

    private System.Collections.IEnumerator GoToLoseScene()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScene");
    }

    public void WinGame()
    {
        Debug.Log("You Win!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            LoseGame();
        }
    }
}
