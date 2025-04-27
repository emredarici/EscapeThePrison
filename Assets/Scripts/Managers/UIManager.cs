using UnityEngine;
using System.Collections;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI informationText;
    public GameObject movementTrailer;

    public void ChangeText(TextMeshProUGUI text, string message)
    {
        StartCoroutine(TypeTextEffect(text, message, 0.05f));
    }

    public IEnumerator TypeTextEffect(TextMeshProUGUI text, string message, float delay)
    {
        text.text = "";
        foreach (char letter in message)
        {
            text.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }
}
