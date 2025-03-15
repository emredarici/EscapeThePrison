using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    bool MessageSend = false;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) && MessageSend == false)
        {
            if (hit.collider.CompareTag("Player"))
            {
                dialogueTrigger.StartDialogue();
                MessageSend = true;
            }
        }
    }
}
