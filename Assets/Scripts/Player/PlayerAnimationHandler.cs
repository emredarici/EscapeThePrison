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

        private void Awake()
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

        public void SleepPlayer()
        {
            playerControls.DisableInput();
            PlaySleepAnimation(() =>
            {
                animator.SetBool("Sleep", false);
                SetMovementSpeed(0);
                playerControls.EnableInput();

                if (characterController != null)
                {
                    characterController.enabled = true;
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
