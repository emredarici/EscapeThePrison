using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerColliderDetection : MonoBehaviour
    {
        private ICollectible currentCollectible;
        private IPlayerAnimationHandler animationHandler;
        private PlayerControls playerControls;

        private bool canOpenPoliceDoor = false;
        public int triggerCount = 0;

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

            if (other.CompareTag("Lock"))
            {
                DailyRoutineManager.Instance.CloseAllCellDoors();
            }
            if (other.CompareTag("PoliceRoomVFX"))
            {
                if (MinigameManager.Instance.policeRoomKey.IsCollected && canOpenPoliceDoor == false)
                {
                    canOpenPoliceDoor = true;
                }
                else if (MinigameManager.Instance.policeRoomKey.IsCollected && canOpenPoliceDoor == true)
                {
                    DailyRoutineManager.Instance.policeRoomDoor.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    canOpenPoliceDoor = false;
                    if (MinigameManager.Instance.key.IsCollected && MinigameManager.Instance.crowbar.IsCollected)
                    {
                        other.gameObject.SetActive(false);
                        DailyRoutineManager.Instance.fourDayNPC.SetActive(true);
                        VFXManager.Instance.SpawnLocationMarker(DailyRoutineManager.Instance.fourDayVFXPosition.position);

                    }
                }
                else if (!MinigameManager.Instance.policeRoomKey.IsCollected)
                {
                    UIManager.Instance.ChangeText(UIManager.Instance.informationText, "You need the Police Room Key to open this door.");
                }

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
            if (canOpenPoliceDoor && PlayerRaycastHandler.Instance.interactionControl.action.triggered)
            {
                DailyRoutineManager.Instance.policeRoomDoor.transform.rotation = Quaternion.Euler(-90, 90, 0);
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
                    if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day3) && DailyRoutineManager.Instance.isThirdDayNPCDialouge == false)
                    {
                        DailyRoutineManager.Instance.thirdDayNPC.GetComponent<DialogueTrigger>().StartDialogue();
                    }
                    if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day4) && DailyRoutineManager.Instance.isFourDayNPCDialouge == false)
                    {
                        DailyRoutineManager.Instance.fourDayNPC.GetComponent<DialogueTrigger>().StartDialogue();
                    }

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
                if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.bedtimeState && DailyRoutineManager.Instance.lockPosition.activeSelf)
                {
                    VFXManager.Instance.DestroyMarker();
                }
                if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.rectimeState)
                {
                    if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day2))
                    {
                        DailyRoutineManager.Instance.twoDayNPC.GetComponentInChildren<DialogueTrigger>().StartDialogue();
                    }
                    if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day3))
                    {
                        DailyRoutineManager.Instance.thirdDay2NPC.GetComponentInChildren<DialogueTrigger>().StartDialogue();
                    }
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
