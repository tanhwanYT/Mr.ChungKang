using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Player : Character
{
    public PlayerState stats;
    public SkillManager skillManager;
    public bool is_jump = false;
    public bool is_ground = false;

    private SpriteRenderer spriteRenderer;
    private PlayerAnimation anim;
    private Rigidbody2D rigid;
    private Animator animator;

    [SerializeField] private Sprite dashSprite;
    private Sprite defaultSprite;

    [Header("Dash Settings")]
    public bool isAbleDash = true;
    public bool isDashing = false;
    public float dashPower = 10f;
    public float dashCooldown = 1f;

    float jump_power = 5.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        anim = GetComponent<PlayerAnimation>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        rigid = GetComponent<Rigidbody2D>();
        defaultSprite = spriteRenderer.sprite; 
    }

    void Update()
    {
        HandleMovement();
        HandleSkillInput();
    }

    public override void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Die();
    }

    public override void Die()
    {
        Debug.Log("Player Dead");
        // GameOver 처리
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 direction = new Vector3(h, 0, 0);
        if (!isDashing)
        {
            rigid.velocity = new Vector2(h * moveSpeed, rigid.velocity.y);
        }

        anim.UpdateMoveAnimation(direction);

        if (Input.GetKeyDown(KeyCode.Space) && !is_jump && is_ground)
        {
            rigid.AddForce(Vector2.up * jump_power, ForceMode2D.Impulse);
            is_jump = true;
            is_ground = false;

            anim.SetIsJump(is_jump);
            anim.SetIsGround(is_ground);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isAbleDash)
        {
            isDashing = true;
            StartCoroutine(DashStart());
        }
        if (h != 0)
            spriteRenderer.flipX = h > 0;
    }

    private void HandleSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            skillManager.UseSkill(0, gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            is_ground = true;
            is_jump = false;

            anim.SetIsJump(is_jump);
            anim.SetIsGround(is_ground);
        }
    }

    private IEnumerator DashStart()
    {
        Debug.Log("Dash!");
        isAbleDash = false;

        var originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f;

        float dashDir = spriteRenderer.flipX ? 1f : -1f;
        float dashDuration = 0.2f;
        rigid.velocity = new Vector2(dashDir * dashPower, 0);

        animator.enabled = false;
        spriteRenderer.sprite = dashSprite;

        yield return new WaitForSeconds(dashDuration);

        if (is_ground)
            anim.PlayIdle();
        else
            StartCoroutine(JumpAnimation());

        animator.enabled = true;
        spriteRenderer.sprite = defaultSprite;

        isDashing = false;
        rigid.gravityScale = originalGravity;
        rigid.velocity = Vector2.zero;

        yield return new WaitForSeconds(dashCooldown);
        isAbleDash = true;
        Debug.Log("Dash Cool Time Finished");
    }

    private IEnumerator JumpAnimation()
    {
        anim.SetIsJump(true);
        anim.SetIsGround(false);
        yield return new WaitForSeconds(0.2f);
    }
}