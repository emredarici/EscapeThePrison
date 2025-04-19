using Player;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    bool MessageSend = false;

    void Update()
    {
        if (PlayerRaycastHandler.Instance.HasHit)
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
