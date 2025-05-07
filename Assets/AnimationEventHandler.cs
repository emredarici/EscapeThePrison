using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public BrakeDoorMG brakeDoorMinigame; // Reference to the BrakeDoorMG script

    public void TriggerCameraShake()
    {
        brakeDoorMinigame.CameraShake();
    }
}
