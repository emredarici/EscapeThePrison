using UnityEngine;

namespace Player
{
    public class PlayerColliderDetection : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LocationMarkerVFX"))
            {
                DebugToolKit.Log("Player entered the location marker trigger zone.");
                if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.headcountState)
                {
                    DailyRoutineManager.Instance.PlayerHeadCount();
                }
            }
        }
    }
}

