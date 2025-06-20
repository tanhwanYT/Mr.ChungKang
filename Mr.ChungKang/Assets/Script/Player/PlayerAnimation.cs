using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateMoveAnimation(Vector3 direction)
    {
        bool isMoving = direction.magnitude > 0.1f;
        animator.SetBool("isRunning", isMoving);
    }

    public void SetIsJump(bool isJumping)
    {
        animator.SetBool("jump", isJumping);
    }
    public void SetIsGround(bool grounded)
    {
        animator.SetBool("isground", grounded);
    }

    public bool IsPlaying(string animName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animName) &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
    }

    public void PlayIdle()
    {
        animator.Play("Idle");
    }

    public void TriggerSkill()
    {

    }

    public void TriggerDamage()
    {

    }

    public void TriggerDeath()
    {

    }
}