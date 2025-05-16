using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI informationText;
    public GameObject movementTrailer;
    private Dictionary<TextMeshProUGUI, Coroutine> typeCoroutines = new Dictionary<TextMeshProUGUI, Coroutine>();
    public Image fadeImage;

    public void ChangeText(TextMeshProUGUI text, string message)
    {
        DeleteText(text);
        StartTypeTextEffect(text, message, 0.05f);

    }

    public void DeleteText(TextMeshProUGUI text)
    {
        StartTypeTextEffect(text, "", 0);
    }

    public void FadeCamera(bool fadeIn, float duration)
    {
        StartCoroutine(FadeCoroutine(fadeIn, duration));
    }

    private void StartTypeTextEffect(TextMeshProUGUI text, string message, float delay)
    {
        // Eğer bu text için bir coroutine zaten çalışıyorsa yeni başlatma, mevcut devam etsin
        if (typeCoroutines.ContainsKey(text) && typeCoroutines[text] != null)
        {
            return;
        }
        // Yeni coroutine başlat ve kaydet
        typeCoroutines[text] = StartCoroutine(TypeTextEffect(text, message, delay));
    }

    public IEnumerator TypeTextEffect(TextMeshProUGUI text, string message, float delay)
    {
        text.text = "";
        foreach (char letter in message)
        {
            text.text += letter;
            yield return new WaitForSeconds(delay);
        }
        // Bittiğinde coroutine referansını kaldır
        if (typeCoroutines.ContainsKey(text))
            typeCoroutines[text] = null;
    }

    private IEnumerator FadeCoroutine(bool fadeIn, float duration)
    {
        float startAlpha = fadeIn ? 1f : 0f;
        float endAlpha = fadeIn ? 0f : 1f;
        float elapsed = 0f;

        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;

        if (fadeIn)
            fadeImage.gameObject.SetActive(false);
    }
}
