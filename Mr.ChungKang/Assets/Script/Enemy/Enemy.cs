using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : Character
{
    public EnemyAI ai;
    public EnemyData data;

    private void Start()
    {
        characterName = data.enemyName;
        maxHP = data.maxHP;
        currentHP = maxHP;
        moveSpeed = data.moveSpeed;
        GetComponent<Animator>().runtimeAnimatorController = data.animator;
    }
    private void Update()
    {
        ai?.Execute(this);
    }

    public override void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Die();
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}