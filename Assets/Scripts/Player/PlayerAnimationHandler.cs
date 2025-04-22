using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour, IPlayerAnimationHandler
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
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

    private IEnumerator WaitForAnimation(float duration, Action onAnimationComplete)
    {
        yield return new WaitForSeconds(duration + 2.0f);
        onAnimationComplete?.Invoke();
    }
}
