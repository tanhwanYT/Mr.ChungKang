using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public enum AttackType
{
    Melee,
    Ranged
}

public class Player : Character
{
    public PlayerState stats;
    public SkillManager skillManager;
    public bool is_jump = false;
    public bool is_ground = false;
    public bool is_punch = false;

    [Header("UI")]
    public Image hpFill;
    public Image epFill;
    public Image expFill;

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

    public AttackType currentAttackType = AttackType.Melee;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private void Awake()
    {
        if (stats == null)
            stats = new PlayerState();

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
        HandleAttackInput();
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentAttackType = (currentAttackType == AttackType.Melee) ? AttackType.Ranged : AttackType.Melee;
            Debug.Log("폼 전환: " + currentAttackType);
        }
    }

    public override void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Die();
    }

    public override void Die()
    {
        Debug.Log("Player Dead");
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

    private void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Z)) // 기본 공격 키
        {
            if (currentAttackType == AttackType.Melee)
                MeleeAttack();
            else if (currentAttackType == AttackType.Ranged)
                RangedAttack();
        }
    }

    private void MeleeAttack()
    {
        if (!is_ground || is_punch)
        {
            return;
        }
        is_punch = true;
        anim.TriggerPunch();

        Vector2 dir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.5f, LayerMask.GetMask("Enemy"));

        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(5f);
        }

        StartCoroutine(ResetAttack(0.4f));
    }
    private IEnumerator ResetAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        is_punch = false;
    }

    private void RangedAttack()
    {
        Vector3 dir = spriteRenderer.flipX ? Vector3.right : Vector3.left;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * 20f;
    }

    public void GainExp(float amount)
    {
        stats.currentExp += amount;

        while (stats.currentExp >= stats.maxExp)
        {
            stats.currentExp -= stats.maxExp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        stats.level++;
        Debug.Log("레벨 업! 현재 레벨: " + stats.level);

        stats.maxHP += 20;
        stats.maxEP += 10;
        stats.attackPower += 5;

        stats.currentHP = stats.maxHP;
        stats.currentEP = stats.maxEP;

        stats.maxExp += 50;

    }

    private void UpdateUI()
    {
        if (hpFill != null)
            hpFill.fillAmount = stats.currentHP / stats.maxHP;

        if (epFill != null)
            epFill.fillAmount = stats.currentEP / stats.maxEP;

        if (expFill != null)
            expFill.fillAmount = stats.currentExp / stats.maxExp;
    }
}