using UnityEngine;

public interface IAttackAnimationHandler
{
    void TriggerAttackAnimation();
    void SetRunningState(bool isRunning);
    void ApplyRotation(Quaternion targetRotation, float speed);
}
