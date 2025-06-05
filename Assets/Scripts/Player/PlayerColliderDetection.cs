using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

namespace Player
{
    public class PlayerColliderDetection : MonoBehaviour
    {
        private ICollectible currentCollectible;
        private IPlayerAnimationHandler animationHandler;
        private PlayerControls playerControls;
        private DialogueManager dialogueManager;
        private PlayerAnimationHandler playerAnimationHandler;

        private bool canOpenPoliceDoor = false;
        public bool isCollectiblePut = false;
        public int triggerCount = 0;

        private void Awake()
        {
            playerAnimationHandler = this.gameObject.GetComponent<PlayerAnimationHandler>();
            animationHandler = GetComponent<IPlayerAnimationHandler>();
            playerControls = GetComponent<PlayerControls>();
            dialogueManager = FindObjectOfType<DialogueManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleLocationMarker(other);
            HandleDialogueMarker(other);
            HandleRefactoryDetection(other);
            HandleCollectiblePutDetection(other);

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
                        VFXManager.Instance.SpawnDialogueMarker(DailyRoutineManager.Instance.fourDayVFXPosition.position);
                        DailyRoutineManager.Instance.polices.bedTimePolice.SetActive(false);
                        dialogueManager.chasePolice.hasDialogueTriggered = false;
                    }
                }
                else if (!MinigameManager.Instance.policeRoomKey.IsCollected)
                {
                    UIManager.Instance.ChangeText(UIManager.Instance.informationText, "You need the Police Room Key to open this door.");
                }

            }
            if (other.CompareTag("PoliceDetection") && DailyRoutineManager.Instance.dayManager.IsDay(Day.Day5))
            {
                StartCoroutine(WaitForPoliceDet(3f));
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
                    if (DailyRoutineManager.Instance.isEatFood == true)
                    {
                        if (triggerCount == 0)
                        {
                            VFXManager.Instance.DestroyMarker(0f);
                            playerAnimationHandler.PlayerOnTrayAnimation();
                            VFXManager.Instance.SpawnLocationMarker(DailyRoutineManager.Instance.PlayerRandomTablePosition().position);
                        }

                        if (triggerCount > 0)
                        {
                            VFXManager.Instance.DestroyMarker(2f);
                            playerAnimationHandler.PlayerOffTrayAnimation();
                            DailyRoutineManager.Instance.SwitchState(DailyRoutineManager.Instance.rectimeState);
                            triggerCount = 0;
                            return;
                        }
                        triggerCount++;
                    }
                }

                if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.bedtimeState && DailyRoutineManager.Instance.lockPosition.activeSelf)
                {
                    VFXManager.Instance.DestroyMarker();
                }

                if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day5))
                {
                    GameManager.Instance.WinGame();
                }
            }
        }

        private void HandleDialogueMarker(Collider other)
        {
            if (other.CompareTag("DialogueMarkerVFX"))
            {
                DebugToolKit.Log("Player entered the dialogue marker trigger zone.");
                if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.chowtimeState)
                {
                    if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day3) && DailyRoutineManager.Instance.isThirdDayNPCDialouge == false)
                    {
                        DailyRoutineManager.Instance.thirdDayNPC.GetComponent<DialogueTrigger>().StartDialogue();
                        VFXManager.Instance.DestroyDialogueMarker();
                    }
                    if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day4) && DailyRoutineManager.Instance.isFourDayNPCDialouge == false)
                    {
                        DailyRoutineManager.Instance.fourDayNPC.GetComponent<DialogueTrigger>().StartDialogue();
                        VFXManager.Instance.DestroyDialogueMarker();
                    }
                }
                if (DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.rectimeState)
                {
                    if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day2))
                    {
                        DailyRoutineManager.Instance.twoDayNPC.GetComponentInChildren<DialogueTrigger>().StartDialogue();
                        VFXManager.Instance.DestroyDialogueMarker();
                    }
                    if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day3))
                    {
                        DailyRoutineManager.Instance.thirdDay2NPC.GetComponentInChildren<DialogueTrigger>().StartDialogue();
                        VFXManager.Instance.DestroyDialogueMarker();
                    }
                }
            }
        }

        private void HandleRefactoryDetection(Collider other)
        {
            if (other.CompareTag("RefactoryDetection"))
            {
                DailyRoutineManager.Instance.polices.bedTimePolice.SetActive(false);
                if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day4) && DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.chowtimeState)
                {
                    MinigameManager minigameManager = MinigameManager.Instance;
                    if (minigameManager.key.IsCollected && minigameManager.crowbar.IsCollected)
                    {
                        if (!isCollectiblePut)
                        {
                            GameManager.Instance.LoseGame();
                        }
                    }
                    else
                    {
                        UIManager.Instance.ChangeText(UIManager.Instance.informationText, "Go to the guard room and collect the items before the meal.");
                    }
                }
            }
        }

        private void HandleCollectiblePutDetection(Collider other)
        {
            if (other.CompareTag("CollectiblePutDetection"))
            {
                if (DailyRoutineManager.Instance.dayManager.IsDay(Day.Day4) && DailyRoutineManager.Instance.currentState == DailyRoutineManager.Instance.chowtimeState)
                {
                    MinigameManager minigameManager = MinigameManager.Instance;
                    if (minigameManager.key.IsCollected && minigameManager.crowbar.IsCollected)
                    {
                        AudioManager.Instance.PlayAudio(playerControls.audioSource, AudioManager.Instance.colletSouce, 2);
                        isCollectiblePut = true;
                        Debug.Log("Player has hidden the key and crowbar.");
                    }
                }
            }
        }

        private void CollectItem()
        {
            playerControls.DisableInput();
            currentCollectible.CollectAudio();
            animationHandler.PlayPickupAnimation(() =>
            {
                currentCollectible.Collect();
                currentCollectible = null;

                animationHandler.SetMovementSpeed(0);
                playerControls.EnableInput();
            });
        }

        private IEnumerator WaitForPoliceDet(float waitTime)
        {
            UIManager.Instance.ChangeText(UIManager.Instance.informationText, "A police officer is coming, hide behind the locker!");
            yield return new WaitForSeconds(waitTime);
            DailyRoutineManager.Instance.polices.escape1police.SetActive(true);
        }

    }
}
