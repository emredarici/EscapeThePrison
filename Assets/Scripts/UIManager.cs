using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI informationText;

    public void ChangeText(TextMeshProUGUI text, string message)
    {
        text.text = message;
    }
}
