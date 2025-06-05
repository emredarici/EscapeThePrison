using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenuButton : MonoBehaviour
{
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}