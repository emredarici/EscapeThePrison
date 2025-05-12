using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerColliderDetection : MonoBehaviour
    {
        private ICollectible currentCollectible;
        private IPlayerAnimationHandler animationHandler;
        private PlayerControls playerControls;

        private int triggerCount = 0;

        private void Awake()
        {
            animationHandler = GetComponent<IPlayerAnimationHandler>();
            playerControls = GetComponent<PlayerControls>();
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleLocationMarker(other);

            if (other.TryGetComponent<ICollectible>(out var collectible))
            {
                currentCollectible = collectible;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<ICollectible>(out var collectible) && collectible == currentCollectible)
            {
                currentCollectible = null;
            }

        }

        private void Update()
        {
            if (currentCollectible != null && PlayerRaycastHandler.Instance.interactionControl.action.triggered)
            {
                CollectItem();
            }
        }

        private void HandleLocationMarker(Collider other)
        {
            if (other.CompareTag("LocationMarkerVFX"))
            {
                DebugToolKit.Log("Player entered the location marker trigger zone.");
                if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.headcountState)
                {
                    DailyRoutineManager.Instance.PlayerHeadCount();
                }

                if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.chowtimeState)
                {
                    if (triggerCount == 0)
                    {
                        VFXManager.Instance.DestroyMarker(2f);
                        VFXManager.Instance.SpawnLocationMarker(DailyRoutineManager.Instance.PlayerRandomTablePosition().position);
                    }

                    if (triggerCount > 0)
                    {
                        VFXManager.Instance.DestroyMarker(2f);
                        DailyRoutineManager.Instance.SwitchState(DailyRoutineManager.Instance.rectimeState);
                        triggerCount = 0;
                        return;
                    }
                    triggerCount++;

                }
            }
        }

        private void CollectItem()
        {
            playerControls.DisableInput();
            animationHandler.PlayPickupAnimation(() =>
            {
                currentCollectible.Collect();
                currentCollectible = null;

                animationHandler.SetMovementSpeed(0);
                playerControls.EnableInput();
            });
        }


    }
}
