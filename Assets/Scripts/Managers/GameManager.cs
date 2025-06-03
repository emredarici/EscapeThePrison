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
            // Tüm aktif AudioSource'ların pitch'ini düşür
            foreach (var audio in FindObjectsOfType<AudioSource>())
            {
                audio.pitch = 0.3f;
            }
            Debug.Log("Game Over");
        });
    }

    public void WinGame()
    {
        Debug.Log("You Win!");
    }
}
