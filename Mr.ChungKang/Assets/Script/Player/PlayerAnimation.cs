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