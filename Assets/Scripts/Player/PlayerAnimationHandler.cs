using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationHandler : Singleton<PlayerAnimationHandler>, IPlayerAnimationHandler
    {
        private Animator animator;
        private PlayerControls playerControls;
        public Transform bedPosition;
        private CharacterController characterController;

        protected override void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            characterController = GetComponent<CharacterController>();
            playerControls = GetComponent<PlayerControls>();
        }

        public void PlayPickupAnimation(Action onAnimationComplete)
        {
            animator.SetTrigger("Pickup");

            float animationDuration = GetAnimationClipLength("Pickup");
            StartCoroutine(WaitForAnimation(animationDuration, onAnimationComplete));
        }

        public void SetMovementSpeed(float speed)
        {
            animator.SetFloat("Speed", speed);
        }

        public void PlaySleepAnimation(Action onAnimationComplete)
        {
            if (characterController != null)
            {
                characterController.enabled = false;
            }
            animator.SetBool("Sleep", true);
            playerControls.transform.position = bedPosition.position;
            playerControls.transform.rotation = bedPosition.rotation;




            float animationDuration = GetAnimationClipLength("Sleep");
            StartCoroutine(WaitForAnimation(animationDuration, onAnimationComplete));
        }

        private float GetAnimationClipLength(string clipName)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }
            return 0f;
        }

        public void PlayHittingAnimation(Action onAnimationComplete)
        {
            animator.SetTrigger("Hitting");
            float animationDuration = GetAnimationClipLength("Hitting");
            StartCoroutine(WaitForAnimation(animationDuration, () =>
            {
                onAnimationComplete?.Invoke();
            }));
        }

        public void SleepPlayer()
        {
            playerControls.DisableInput();
            UIManager.Instance.FadeCamera(false, 1.0f);
            PlaySleepAnimation(() =>
            {
                animator.SetBool("Sleep", false);
                SetMovementSpeed(0);
                playerControls.transform.position = new Vector3(bedPosition.position.x + 1.7f, playerControls.transform.position.y, playerControls.transform.position.z);
                playerControls.EnableInput();

                if (characterController != null)
                {
                    characterController.enabled = true;
                }
                UIManager.Instance.FadeCamera(true, 1.0f);
                DailyRoutineManager.Instance.dayManager.NextDay();
                if (!DailyRoutineManager.Instance.dayManager.IsDay(Day.Day5))
                {
                    DailyRoutineManager.Instance.SwitchState(DailyRoutineManager.Instance.headcountState);
                }
                else
                {
                    DailyRoutineManager.Instance.ResetAllNpcPositions();
                    UIManager.Instance.DeleteText(UIManager.Instance.informationText);
                }
            });
        }

        private IEnumerator WaitForAnimation(float duration, Action onAnimationComplete)
        {
            yield return new WaitForSeconds(duration + 4.0f);
            onAnimationComplete?.Invoke();
        }
    }
}
