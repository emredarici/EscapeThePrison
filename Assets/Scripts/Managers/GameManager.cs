using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
}
