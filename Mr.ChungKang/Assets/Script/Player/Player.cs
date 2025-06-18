using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Player : Character
{
    public PlayerState stats;
    public SkillManager skillManager;

    private SpriteRenderer spriteRenderer;
    private PlayerAnimation anim;

    private void Awake()
    {
        anim = GetComponent<PlayerAnimation>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
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
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, v).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        anim.UpdateMoveAnimation(direction);

        if (h != 0)
            spriteRenderer.flipX = h > 0;
    }

    private void HandleSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            skillManager.UseSkill(0, gameObject);
    }
}