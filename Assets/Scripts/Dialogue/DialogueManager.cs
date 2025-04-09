using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;

    List<Message> cureentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        cureentMessages = new List<Message>(messages);
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        DebugToolKit.Log("Started conversation! Loaded messages: " + messages.Length);
        SplitLongMessages();
        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f);
    }

    void DisplayMessage()
    {
        Message messageToDisplay = cureentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorImage.sprite = actorToDisplay.sprite;
        actorName.text = actorToDisplay.name;

        AnimateTextColor();
    }

    void NextMessage()
    {
        activeMessage++;
        if (activeMessage < cureentMessages.Count)
        {
            DisplayMessage();
        }
        else
        {
            DebugToolKit.Log("End of conversation!");
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            isActive = false;
        }
    }

    void SplitLongMessages()
    {
        for (int i = 0; i < cureentMessages.Count; i++)
        {
            while (cureentMessages[i].message.Length > 370)
            {
                string longMessage = cureentMessages[i].message;
                string firstPart = longMessage.Substring(0, 370);
                string secondPart = longMessage.Substring(370, longMessage.Length - 370);

                cureentMessages[i].message = firstPart;
                Message newMessage = new Message { message = secondPart, actorId = cureentMessages[i].actorId };
                cureentMessages.Insert(i + 1, newMessage);
                i++;
            }
        }
    }

    void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0f, 0f);
        LeanTween.textAlpha(messageText.rectTransform, 1f, .5f);
    }

    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isActive == true)
        {
            NextMessage();
        }
    }
}
