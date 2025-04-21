using Player;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    bool MessageSend = false;

    void Update()
    {
        if (PlayerRaycastHandler.Instance.HasHit && PlayerRaycastHandler.Instance.interactionControl.action.triggered)
        {
            RaycastHit hit = PlayerRaycastHandler.Instance.CurrentHit;

            if (hit.collider.CompareTag("DialogueTriggerNpc") && !MessageSend)
            {
                MessageSend = true;
                dialogueTrigger.StartDialogue();
            }
        }
    }
}
