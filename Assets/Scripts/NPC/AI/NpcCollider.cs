using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCollider : MonoBehaviour
{
    private NPCController npcController;

    private void Start()
    {
        npcController = this.GetComponent<NPCController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CafetariaTable"))
        {
            if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.chowtimeState)
            {
                if (other.transform == this.npcController.targetExitPosition)
                {
                    this.npcController.agent.Warp(other.transform.position);
                    this.gameObject.transform.localRotation = other.transform.localRotation;
                    this.npcController.animator.SetBool("isSitting", true);
                }
            }
        }
    }
}
