using System;

public interface IPlayerAnimationHandler
{
    void PlayPickupAnimation(Action onAnimationComplete);
    void SetMovementSpeed(float speed);
    void PlaySleepAnimation(Action onAnimationComplete);
}