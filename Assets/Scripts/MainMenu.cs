using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button playButton;
    public Button quitButton;
    public Button creditButton;
    public Button githubButton;
    public Button itchioButton;

    [Header("Backgrounds")]
    public RawImage backgroundImage;
    public Texture[] backgrounds;
    public float fadeDuration = 1.5f;
    public float showDuration = 10f;

    private int currentBg = 0;

    void Start()
    {
        playButton.onClick.AddListener(OnPlay);
        quitButton.onClick.AddListener(OnQuit);
        creditButton.onClick.AddListener(OnCredit);
        githubButton.onClick.AddListener(OnGithub);
        itchioButton.onClick.AddListener(OnItchio);

        if (backgrounds.Length > 0)
        {
            backgroundImage.texture = backgrounds[0];
            StartCoroutine(BackgroundFadeLoop());
        }
    }

    void OnPlay()
    {
        SceneManager.LoadScene("FirstScene");
    }

    void OnQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void OnCredit()
    {
        // Kredi paneli açılacaksa burada aktif et
    }

    void OnGithub()
    {
        Application.OpenURL("https://github.com/kendi-linkin");
    }

    void OnItchio()
    {
        Application.OpenURL("https://itch.io/profile/kendi-linkin");
    }

    IEnumerator BackgroundFadeLoop()
    {
        while (true)
        {
            int nextBg = (currentBg + 1) % backgrounds.Length;
            yield return StartCoroutine(FadeToBackground(nextBg));
            currentBg = nextBg;
            yield return new WaitForSeconds(showDuration);
        }
    }

    IEnumerator FadeToBackground(int nextBg)
    {
        float t = 0f;
        backgroundImage.texture = backgrounds[nextBg];
        Color c = backgroundImage.color;
        c.a = 0f;
        backgroundImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeDuration);
            backgroundImage.color = c;
            yield return null;
        }
        backgroundImage.color = Color.white;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
